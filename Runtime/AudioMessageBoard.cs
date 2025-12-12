using System;
using UnityEngine;

namespace bnj.audio_events.Runtime
{
    public static class AudioMessageBoard
    {
        public static Action<SO_AudioEvent, Vector3, object> PlayAudioEvent;
        public static Action<object> StopAudioEventsFromSource;
        public static Action<string, float> SetMixerGroupVolumeAndCache;
        public static Action<string, float> SetMixerGroupVolume;
        public static Action<string, bool> ToggleMixerGroupMuteAndCache;
    }
}
