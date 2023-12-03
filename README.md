# Shard Police

## Features

- Limit players to 1 shard buff at a time. Activating a shard replaces any previously held shard buff with the new one.
  - Optional, can be enabled/disabled via config. Enabled by default.
- Commands (admin only) to remove shard buffs from players.
  - `shard-buffs-remove-everyone` removes the shard buffs from all players. (`sbre` for short)
  - affected players are notified when their buffs are removed.

## TODO (not implemented)

- mitigate sharing shard buffs outside the clan that holds the shard. strip shard buffs on clan changes. think about potential workarounds
  - e.g. shard holder joins a clan, they all grab the buff, then the shard holder leaves. simply removing the buff from the holder when they leave wouldn't be enough.
  - optional, can be enabled/disabled. default enabled
- strip shard buff from players when the placed shard is dropped on the ground
  - optional, can be enabled/disabled. default enabled
- command (admin only) to strip shard buffs from all players or a specific player
  - should create a system message for affected players
- command (admin only) to reset shards (despawn placed shards, shard items in inventories, shard items on ground, etc) and return them to the bosses that originally hold them.
  - should strip shard buffs from all players
  - should create system message for all players

## Config

Running the server with this mod installed will create a configuration file at `$(VRisingServerPath)/BepInEx/config/ShardPolice.cfg`.

```
## Settings file was created by plugin ShardPolice v1.0.0
## Plugin GUID: ShardPolice

[Shard Buffs]

## Whether or not to limit each player to only 1 shard buff at a time
# Setting type: Boolean
# Default value: true
LimitShardBuffsToOnlyOneAtATime = true

```
