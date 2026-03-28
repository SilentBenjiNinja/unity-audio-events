using System;
using UnityEngine;

namespace bnj.audio_events.Runtime
{
    /// <summary>
    /// Static message bus for audio communication across the scene.
    /// Invoke these actions to request audio operations from an <see cref="MB_AudioSourceSpawner"/>.
    /// </summary>
    public static class AudioEventsMessageBus
    {
        /// <summary>
        /// Request playback of an audio event at a world-space position.
        /// </summary>
        /// <remarks>
        /// Parameters: <c>audioEvent</c>, <c>worldPosition</c>, <c>source</c> (optional owner —
        /// pass <see langword="this"/> to enable stopping audio from this object later via
        /// <see cref="StopAudioEventsFromSource"/>).
        /// </remarks>
        public static Action<SO_AudioEvent, Vector3, object> PlayAudioEvent;

        /// <summary>
        /// Stop all audio events currently playing that were started by the given source object.
        /// </summary>
        public static Action<object> StopAudioEventsFromSource;
    }
}
