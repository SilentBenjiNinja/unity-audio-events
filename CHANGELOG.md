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
