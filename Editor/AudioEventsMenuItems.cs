using bnj.audio_events.Runtime;
using UnityEditor;
using UnityEngine;

namespace bnj.audio_events.Editor
{
    public static class AudioEventsMenuItems
    {
        const string PrefabPath = "Packages/com.bnj.audio-events/Prefabs/PooledAudioSource.prefab";

        [MenuItem("GameObject/BNJ/Audio Events/Audio Source Spawner", false, 1000)]
        static void CreateAudioSourceSpawner(MenuCommand menuCommand)
        {
            var go = new GameObject("AudioSourceSpawner");
            var spawner = go.AddComponent<MB_AudioSourceSpawner>();
            var prefab = AssetDatabase.LoadAssetAtPath<AudioSource>(PrefabPath);

            if (prefab == null)
                Debug.LogWarning($"Prefab not found at: {PrefabPath}. Please assign manually.");
            else
            {
                var serializedSpawner = new SerializedObject(spawner);
                var prefabProperty = serializedSpawner.FindProperty("_prefab");

                if (prefabProperty == null)
                    Debug.LogWarning("Could not find _prefab property on MB_AudioSourceSpawner");
                else
                {
                    prefabProperty.objectReferenceValue = prefab;
                    serializedSpawner.ApplyModifiedProperties();
                }
            }

            // Standard setup
            Undo.RegisterCreatedObjectUndo(go, "Create Audio Source Spawner");
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Selection.activeObject = go;
        }
    }
}
