using BepInEx.Configuration;

namespace ShardPolice;

public static class ShardPoliceConfig {
    public static ConfigEntry<bool> LimitShardBuffsToOnlyOneAtATime { get; private set; }

    public static void Init(ConfigFile config) {

        LimitShardBuffsToOnlyOneAtATime = config.Bind<bool>(
            section: "Shard Buffs",
            key: "LimitShardBuffsToOnlyOneAtATime",
            defaultValue: true,
            description: "Whether or not to limit each player to only 1 shard buff at a time"
        );
        
    }
    
}
