# Audio Events for Unity

* Provides object pooling for audio sources
* Use audio event SOs to set up variety in pitch, volume & audio clip for events
* Limits how many sounds of the same origin can be played at once and at which interval

## Setup

* Add an audio source spawner to your scene: [+] > BNJ > Audio Events > Audio Source Spawner
* Create audio events in your project: Create > BNJ > Audio Event
* Assign audio clips to the events and adjust their mixing, variation & limits
* To trigger an audio event:
```
// get a reference to the audio event, e.g. via an exposed field
[SerializeField] SO_AudioEvent _audioEvent;

// could be any position you want the audio source to spawn at
var audioPosition = transform.position;

// Add reference to event source so playing audio can be stopped      V
AudioMessageBoard.PlayAudioEvent?.Invoke(_audioEvent, audioPosition, this);

// To stop playing audio events coming from this source
AudioMessageBoard.StopAudioEventsFromSource?.Invoke(this);
```