using System;
using System.Collections.Generic;
using bnj.so_manager.Runtime;
using bnj.utility_toolkit.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

// TODO: add button to preview audio event (toggles play and stop)
// (Instantiate audio source, play and remove it again)
// https://discussions.unity.com/t/way-to-play-audio-in-editor-using-an-editor-script/473638/20
// Also check out https://docs.unity3d.com/6000.0/Documentation/Manual/AudioRandomContainer-UI.html
namespace bnj.audio_events.Runtime
{
    [HideMonoScript, ManageableData("Audio Events", Order = 1000)]
    [CreateAssetMenu(menuName = "BNJ/Audio Event", fileName = "AE_", order = 90)]
    public class SO_AudioEvent : ScriptableObject
    {
        [BoxGroup("Mix")]
        [HorizontalGroup("Mix/H", Width = 90)]
        [SerializeField, ToggleLeft, LabelText("Spatial (3D)")]
        bool _isSpatial = true;
        public bool IsSpatial => _isSpatial;

        [HorizontalGroup("Mix/H", Width = 60)]
        [SerializeField, ToggleLeft, LabelText("Loop")]
        bool _loop;
        public bool Loop => _loop;

        [HorizontalGroup("Mix/H")]
        [SerializeField, HideLabel]
        AudioMixerGroup _mixerChannel;
        public AudioMixerGroup MixerChannel => _mixerChannel;

        [BoxGroup("Variations")]
        [SerializeField, LabelWidth(60), MinMaxSlider(0, 1, true)]
        Vector2 _volume = new(.8f, 1);
        public float RandomizedVolume => RandomUtils.RandomFloatBetween(_volume.x, _volume.y).Clamp01();

        [BoxGroup("Variations")]
        [SerializeField, LabelWidth(60), MinMaxSlider(-3, 3, true)]
        Vector2 _pitch = new(.9f, 1.1f);
        public float RandomizedPitch => RandomUtils.RandomFloatBetween(_pitch.x, _pitch.y).Clamp(AudioUtils.MIN_PITCH, AudioUtils.MAX_PITCH);

        [BoxGroup("Limitations")]
        [HorizontalGroup("Limitations/H")]
        [BoxGroup("Limitations/H/Priority")]
        [SerializeField, HideLabel, Range(0, 256), Tooltip("Lower values have higher priority")]
        int _priority = 128;
        public int Priority => _priority;

        [BoxGroup("Limitations/H/Max Active")]
        [SerializeField, HideLabel, MinValue(1), Tooltip("How many instances of this event are allowed to play at once")]
        int _maxPlayingSimultaneously = 3;
        public int MaxPlayingSimultaneously => _maxPlayingSimultaneously;

        [BoxGroup("Limitations/H/Cooldown")]
        [SerializeField, HideLabel, MinValue(0), Unit(Units.Second)]
        float _cooldown;
        public float Cooldown => _cooldown;

        [ListDrawerSettings(ShowFoldout = false, ShowPaging = false, ShowItemCount = false)]
        [SerializeField] List<AudioClip> _audioClipPool;
        public AudioClip RandomClipFromPool => _audioClipPool[RandomUtils.RandomIntBetween(0, _audioClipPool.Count)];
    }
}