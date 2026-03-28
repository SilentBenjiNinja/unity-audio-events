using System;
using System.Collections.Generic;
using bnj.so_manager.Runtime;
using bnj.utility_toolkit.Runtime;
using bnj.utility_toolkit.Runtime.Audio;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

// TODO: add button to preview audio event (toggles play and stop)
// (Instantiate audio source, play and remove it again)
// https://discussions.unity.com/t/way-to-play-audio-in-editor-using-an-editor-script/473638/20
// Also check out https://docs.unity3d.com/6000.0/Documentation/Manual/AudioRandomContainer-UI.html
namespace bnj.audio_events.Runtime
{
    /// <summary>
    /// ScriptableObject that defines an audio event with randomized variation, concurrency limits, and pooled playback.
    /// Create via <c>Create &gt; BNJ &gt; Audio Event</c> and trigger via <see cref="AudioEventsMessageBus.PlayAudioEvent"/>.
    /// </summary>
    [HideMonoScript, ManageableData("Audio Events", Order = 1000)]
    [CreateAssetMenu(menuName = "BNJ/Audio Event", fileName = "AE_", order = 90)]
    public class SO_AudioEvent : ScriptableObject
    {
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

        [BoxGroup("Variations")]
        [SerializeField, LabelWidth(60), MinMaxSlider(0, 1, true)]
        Vector2 _volume = new(.8f, 1);
        /// <summary>Returns a random volume value sampled from the configured min/max range, clamped to 0–1.</summary>
        public float RandomizedVolume => RandomUtils.RandomFloatBetween(_volume.x, _volume.y).Clamp01();

        [BoxGroup("Variations")]
        [SerializeField, LabelWidth(60), MinMaxSlider(-3, 3, true)]
        Vector2 _pitch = new(.9f, 1.1f);
        /// <summary>Returns a random pitch value sampled from the configured min/max range, clamped to valid pitch bounds.</summary>
        public float RandomizedPitch => RandomUtils.RandomFloatBetween(_pitch.x, _pitch.y).Clamp(AudioUtils.MIN_PITCH, AudioUtils.MAX_PITCH);

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

        [ListDrawerSettings(ShowFoldout = false, ShowPaging = false, ShowItemCount = false)]
        [SerializeField] List<AudioClip> _audioClipPool;
        /// <summary>Returns a randomly selected <see cref="AudioClip"/> from the configured clip pool.</summary>
        public AudioClip RandomClipFromPool => _audioClipPool[RandomUtils.RandomIntBetween(0, _audioClipPool.Count)];
    }
}