using System.Collections.Generic;
using System.Linq;
using bnj.object_pooler.Runtime;
using bnj.utility_toolkit.Runtime.Coroutines;
using UnityEngine;

namespace bnj.audio_events.Runtime
{
    /// <summary>
    /// Scene-level MonoBehaviour that listens to <see cref="AudioEventsMessageBus"/> and manages
    /// a pool of <see cref="AudioSource"/> instances for decoupled audio playback.
    /// Add one per scene via <c>[+] &gt; BNJ &gt; Audio Events &gt; Audio Source Spawner</c>.
    /// </summary>
    [AddComponentMenu("BNJ/Audio Source Spawner")]
    public class MB_AudioSourceSpawner : MB_ObjectPooler<AudioSource>
    {
        List<int> _currentlyPlayingAudioEvents = new();
        List<int> _audioEventsOnCooldown = new();

        Dictionary<object, List<(AudioSource, int)>> _sourceLookup = new();

        void OnEnable()
        {
            AudioEventsMessageBus.PlayAudioEvent += PlayAudioEvent;
            AudioEventsMessageBus.StopAudioEventsFromSource += StopAudioEventsFromSource;
        }

        void OnDisable()
        {
            AudioEventsMessageBus.PlayAudioEvent -= PlayAudioEvent;
            AudioEventsMessageBus.StopAudioEventsFromSource -= StopAudioEventsFromSource;
        }

        void PlayAudioEvent(SO_AudioEvent audioEvent, Vector3 location, object source)
        {
            if (audioEvent == null)
            {
                Debug.LogWarning("Audio event was null, no sound playing");
                return;
            }

            var audioEventHashed = audioEvent.GetHashCode();

            if (_audioEventsOnCooldown.Contains(audioEventHashed)) return;

            if (_currentlyPlayingAudioEvents.Count(x => x == audioEventHashed) >= audioEvent.MaxPlayingSimultaneously)
                return;

            if (audioEvent.Cooldown > 0)
            {
                _audioEventsOnCooldown.Add(audioEventHashed);

                this.DelayInvoke(
                    () => _audioEventsOnCooldown.Remove(audioEventHashed),
                    audioEvent.Cooldown
                );
            }

            var audioSource = NextFromPool;
            audioSource.transform.position = location;

            PopulateAndPlayAudioSource();

            if (!audioEvent.Loop)
                this.DelayInvokeUntil(
                    () => StopAudioSource((audioSource, audioEventHashed), source),
                    () => !audioSource.isPlaying
                );

            if (source == null) return;

            if (!_sourceLookup.ContainsKey(source)) _sourceLookup[source] = new();

            _sourceLookup[source].Add((audioSource, audioEventHashed));

            void PopulateAndPlayAudioSource()
            {
                audioEvent.Apply(audioSource);
                audioSource.gameObject.SetActive(true);
                audioSource.Play();
                _currentlyPlayingAudioEvents.Add(audioEventHashed);
            }
        }

        void StopAudioEventsFromSource(object source)
        {
            if (!_sourceLookup.ContainsKey(source)) return;

            var audioList = new List<(AudioSource, int)>(_sourceLookup[source]);

            foreach (var audio in audioList)
                StopAudioSource((audio.Item1, audio.Item2), source);
        }

        void StopAudioSource((AudioSource source, int eventHash) audio, object source)
        {
            audio.source.Stop();
            audio.source.gameObject.SetActive(false);

            _currentlyPlayingAudioEvents.Remove(audio.eventHash);

            if (source == null) return;

            _sourceLookup[source].Remove(audio);
        }
    }
}
