# 5.0.0

### Breaking Changes

* `com.bnj.so-manager` is no longer a hard dependency — `[ManageableData]` on `SO_AudioEvent` is now guarded by `#if SO_MANAGER`, which is defined automatically when so-manager is installed

---

# 4.0.0

### Breaking Changes

* Removed public properties `IsSpatial`, `MixerChannel`, and `Priority` from `SO_AudioEvent` — these are internal to `Apply` and no longer part of the public API

### Improvements

* Odin Inspector is now optional — all Odin attributes in `SO_AudioEvent` are guarded by `#if ODIN_INSPECTOR`

---

# 3.0.0

> Requires Unity 6 (2023.2+). For Unity 2020.3–2022.3 support stay on 2.x.

### Breaking Changes

* Removed volume/pitch randomization and clip pool from `SO_AudioEvent` — use `AudioRandomContainer` for variation
* `SO_AudioEvent` now exposes a single `AudioResource` field (accepts `AudioClip` or `AudioRandomContainer`)
* Spawner cleanup now polls `AudioSource.isPlaying` instead of estimating clip duration

### New

* `SO_AudioEvent.Apply(AudioSource)` centralises all `AudioSource` configuration

---

# 2.0.0

### Breaking Changes

* Renamed `AudioMessageBoard` → `AudioEventsMessageBus` to align with the Message Bus pattern and clarify scope
* Removed mixer volume/mute actions (`ToggleMixerGroupVolumeAndCache`, `SetMixerGroupVolume`, `SetMixerGroupMuteAndCache`) — mixer management is now a separate package

### Improvements

* Added XML documentation to all public APIs (`AudioMessageBus`, `SO_AudioEvent`, `MB_AudioSourceSpawner`)
* Expanded README with setup table, mixer control section, and a full usage example
* Improved `package.json` description

---

# 1.0.3

* Bump dependency versions
* Update to utility-toolkit 1.1.0

---

# 1.0.2

* Updated README & package description
* Move attribution to third party notices
* Add game object menu + prefab for easier setup

---

# 1.0.1

* Added icon for audio event SOs
