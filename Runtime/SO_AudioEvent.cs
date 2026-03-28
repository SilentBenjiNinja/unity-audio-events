using UnityEngine;
using UnityEngine.Audio;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
#if SO_MANAGER
using bnj.so_manager.Runtime;
#endif

namespace bnj.audio_events.Runtime
{
    /// <summary>
    /// ScriptableObject that defines an audio event with concurrency limits and pooled playback.
    /// Accepts any <see cref="AudioResource"/> — assign an <see cref="AudioClip"/> or an
    /// <see cref="AudioRandomContainer"/> for built-in variation.
    /// Create via <c>Create &gt; BNJ &gt; Audio Event</c> and trigger via <see cref="AudioEventsMessageBus.PlayAudioEvent"/>.
    /// </summary>
#if ODIN_INSPECTOR
    [HideMonoScript]
#endif
    #if SO_MANAGER
    [ManageableData("Audio Events", Order = 1000)]
#endif
    [CreateAssetMenu(menuName = "BNJ/Audio Event", fileName = "AE_", order = 90)]
    public class SO_AudioEvent : ScriptableObject
    {
#if ODIN_INSPECTOR
        [Required, LabelText("Audio")]
#endif
        [SerializeField] AudioResource _audioResource;

#if ODIN_INSPECTOR
        [BoxGroup("Mix")]
        [HorizontalGroup("Mix/H", Width = 90)]
        [ToggleLeft, LabelText("Spatial (3D)")]
#endif
        [SerializeField] bool _isSpatial = true;

#if ODIN_INSPECTOR
        [HorizontalGroup("Mix/H", Width = 60)]
        [ToggleLeft, LabelText("Loop")]
#endif
        [SerializeField] bool _loop;

#if ODIN_INSPECTOR
        [HorizontalGroup("Mix/H")]
        [HideLabel]
#endif
        [SerializeField] AudioMixerGroup _mixerChannel;

#if ODIN_INSPECTOR
        [BoxGroup("Limitations")]
        [HorizontalGroup("Limitations/H")]
        [BoxGroup("Limitations/H/Priority")]
        [HideLabel]
#endif
        [SerializeField, Range(0, 256), Tooltip("Lower values have higher priority")]
        int _priority = 128;

#if ODIN_INSPECTOR
        [BoxGroup("Limitations/H/Max Active")]
        [HideLabel, MinValue(1)]
#endif
        [SerializeField, Range(1, 256), Tooltip("How many instances of this event are allowed to play at once")]
        int _maxPlayingSimultaneously = 3;
        /// <summary>
        /// Maximum number of instances of this event allowed to play at the same time.
        /// </summary>
        public int MaxPlayingSimultaneously => _maxPlayingSimultaneously;

#if ODIN_INSPECTOR
        [BoxGroup("Limitations/H/Cooldown")]
        [HideLabel, MinValue(0), Unit(Units.Second)]
#endif
        [SerializeField] float _cooldown;
        /// <summary>
        /// Minimum seconds that must pass before this event can be triggered again.
        /// </summary>
        public float Cooldown => _cooldown;

        /// <summary>
        /// Whether the audio source loops instead of playing once.
        /// </summary>
        public bool Loop => _loop;

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