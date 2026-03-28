using bnj.so_manager.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

// https://discussions.unity.com/t/way-to-play-audio-in-editor-using-an-editor-script/473638/20
namespace bnj.audio_events.Runtime
{
    /// <summary>
    /// ScriptableObject that defines an audio event with concurrency limits and pooled playback.
    /// Accepts any <see cref="AudioResource"/> — assign an <see cref="AudioClip"/> or an
    /// <see cref="AudioRandomContainer"/> for built-in variation.
    /// Create via <c>Create &gt; BNJ &gt; Audio Event</c> and trigger via <see cref="AudioEventsMessageBus.PlayAudioEvent"/>.
    /// </summary>
    [HideMonoScript, ManageableData("Audio Events", Order = 1000)]
    [CreateAssetMenu(menuName = "BNJ/Audio Event", fileName = "AE_", order = 90)]
    public class SO_AudioEvent : ScriptableObject
    {
        [SerializeField, Required, LabelText("Audio")]
        AudioResource _audioResource;

        [BoxGroup("Mix")]
        [HorizontalGroup("Mix/H", Width = 90)]
        [SerializeField, ToggleLeft, LabelText("Spatial (3D)")]
        bool _isSpatial = true;
        /// <summary>Whether this event uses 3D spatial blend (attenuates with distance).</summary>
        public bool IsSpatial => _isSpatial;

        [HorizontalGroup("Mix/H", Width = 60)]
        [SerializeField, ToggleLeft, LabelText("Loop")]
        bool _loop;
        /// <summary>Whether the audio source loops instead of playing once.</summary>
        public bool Loop => _loop;

        [HorizontalGroup("Mix/H")]
        [SerializeField, HideLabel]
        AudioMixerGroup _mixerChannel;
        /// <summary>The <see cref="AudioMixerGroup"/> this event is routed through.</summary>
        public AudioMixerGroup MixerChannel => _mixerChannel;

        [BoxGroup("Limitations")]
        [HorizontalGroup("Limitations/H")]
        [BoxGroup("Limitations/H/Priority")]
        [SerializeField, HideLabel, Range(0, 256), Tooltip("Lower values have higher priority")]
        int _priority = 128;
        /// <summary>Audio source priority (0 = highest, 256 = lowest). Lower values are less likely to be stolen by other sources.</summary>
        public int Priority => _priority;

        [BoxGroup("Limitations/H/Max Active")]
        [SerializeField, HideLabel, MinValue(1), Tooltip("How many instances of this event are allowed to play at once")]
        int _maxPlayingSimultaneously = 3;
        /// <summary>Maximum number of instances of this event allowed to play at the same time.</summary>
        public int MaxPlayingSimultaneously => _maxPlayingSimultaneously;

        [BoxGroup("Limitations/H/Cooldown")]
        [SerializeField, HideLabel, MinValue(0), Unit(Units.Second)]
        float _cooldown;
        /// <summary>Minimum seconds that must pass before this event can be triggered again.</summary>
        public float Cooldown => _cooldown;

        /// <summary>
        /// Applies this event's <see cref="AudioResource"/> and all mix settings to the given <see cref="AudioSource"/>.
        /// Called by <see cref="MB_AudioSourceSpawner"/> before playing.
        /// </summary>
        public void Apply(AudioSource source)
        {
            source.resource = _audioResource;
            source.outputAudioMixerGroup = _mixerChannel;
            source.spatialBlend = _isSpatial ? 1 : 0;
            source.reverbZoneMix = _isSpatial ? 1 : 0;
            source.priority = _priority;
            source.loop = _loop;
        }
    }
}