# Audio Events for Unity

Provides a lightweight, decoupled audio system built on ScriptableObject events and pooled audio sources.

## Features

- **Pooled audio sources** — no `Instantiate`/`Destroy` overhead per sound
- **SO-based audio events** — configure pitch/volume variation, clip pool, mixing, looping, and priority in one asset
- **Concurrency limiting** — cap how many instances of the same event play simultaneously
- **Cooldown support** — enforce a minimum re-trigger interval per event
- **Source tracking** — stop all audio from a specific owner with one call

## Setup

### 1. Add an Audio Source Spawner to your scene

```
[+] > BNJ > Audio Events > Audio Source Spawner
```

The spawner listens to `AudioEventsMessageBus` and manages the pooled `AudioSource` instances.
There should be **one spawner per scene**.

### 2. Create an Audio Event asset

```
Create > BNJ > Audio Event
```

Configure the event in the Inspector:

| Field | Description |
|---|---|
| **Spatial (3D)** | Whether audio attenuates with distance |
| **Loop** | Whether the audio source loops |
| **Mixer Channel** | The `AudioMixerGroup` to route through |
| **Volume** | Min/max range — a random value is sampled per play |
| **Pitch** | Min/max range — a random value is sampled per play |
| **Priority** | Source priority (0 = highest, 256 = lowest) |
| **Max Active** | Maximum simultaneous instances of this event |
| **Cooldown** | Minimum seconds between re-triggers |
| **Clip Pool** | List of clips — one is chosen at random each play |

### 3. Trigger events at runtime

```csharp
[SerializeField] SO_AudioEvent _audioEvent;

// Play at this object's world position, tracked to this source
AudioEventsMessageBus.PlayAudioEvent?.Invoke(_audioEvent, transform.position, this);

// Stop all audio currently playing from this object
AudioEventsMessageBus.StopAudioEventsFromSource?.Invoke(this);
```

Pass `this` as the third argument to enable source tracking. Pass `null` if cleanup is not needed.

## Full Example

```csharp
public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] SO_AudioEvent _shootEvent;
    [SerializeField] SO_AudioEvent _deathEvent;

    public void Shoot()
    {
        // Plays at the weapon's position; tracked to this object for cleanup
        AudioEventsMessageBus.PlayAudioEvent?.Invoke(_shootEvent, transform.position, this);
    }

    public void Die()
    {
        // Stop any looping audio still running from this object
        AudioEventsMessageBus.StopAudioEventsFromSource?.Invoke(this);

        // Play a one-shot death sound (no source tracking needed)
        AudioEventsMessageBus.PlayAudioEvent?.Invoke(_deathEvent, transform.position, null);
    }
}
```
