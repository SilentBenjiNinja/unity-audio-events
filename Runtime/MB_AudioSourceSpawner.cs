using System.Collections.Generic;
using System.Linq;
using bnj.object_pooler.Runtime;
using bnj.utility_toolkit.Runtime;
using UnityEngine;

namespace bnj.audio_events.Runtime
{
    [AddComponentMenu("BNJ/Audio Source Spawner")]
    public class MB_AudioSourceSpawner : MB_ObjectPooler<AudioSource>
    {
        List<int> _currentlyPlayingAudioEvents = new();
        List<int> _audioEventsOnCooldown = new();

        Dictionary<object, List<(AudioSource, int)>> _sourceLookup = new();

        void OnEnable()
        {
            AudioMessageBoard.PlayAudioEvent += PlayAudioEvent;
            AudioMessageBoard.StopAudioEventsFromSource += StopAudioEventsFromSource;
        }

        void OnDisable()
        {
            AudioMessageBoard.PlayAudioEvent -= PlayAudioEvent;
            AudioMessageBoard.StopAudioEventsFromSource -= StopAudioEventsFromSource;
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

            Coroutine coroutine;

            PopulateAndPlayAudioSource();

            if (audioEvent.Loop)
            {
                //coroutine = RepeatInvoke(
                //    () => PopulateAndPlayAudioSource(true),
                //    new WaitForSeconds(GetClipDuration())
                //);
            }
            else
                coroutine = this.DelayInvoke(
                    () => StopAudioSource((audioSource, audioEventHashed), source),
                    GetClipDuration()
                );

            if (source == null) return;

            if (!_sourceLookup.ContainsKey(source)) _sourceLookup[source] = new();

            _sourceLookup[source].Add((audioSource, audioEventHashed/*, coroutine*/));

            void PopulateAndPlayAudioSource(bool isRepeating = false)
            {
                audioSource.clip = audioEvent.RandomClipFromPool;
                audioSource.outputAudioMixerGroup = audioEvent.MixerChannel;
                audioSource.spatialBlend = audioEvent.IsSpatial ? 1 : 0;
                audioSource.reverbZoneMix = audioEvent.IsSpatial ? 1 : 0;
                audioSource.volume = audioEvent.RandomizedVolume;
                audioSource.pitch = audioEvent.RandomizedPitch;
                audioSource.priority = audioEvent.Priority;
                audioSource.loop = audioEvent.Loop;
                //audioSource.gameObject.name = audioEvent.GetHashCode().ToString();

                audioSource.gameObject.SetActive(true);
                audioSource.Play();

                if (isRepeating) return;

                _currentlyPlayingAudioEvents.Add(audioEventHashed);
            }

            // TODO: on loop play different clip from pool?
            float GetClipDuration() =>
                audioSource.clip.length / audioSource.pitch;
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
