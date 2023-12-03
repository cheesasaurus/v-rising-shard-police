using Bloodstone.API;
using ProjectM;
using ShardPolice.Prefabs;
using Unity.Entities;

namespace ShardPolice.Utils;

public class ShardUtil {

    private static readonly PrefabGUID[] ShardBuffs = {
        ShardPrefabs.AB_Interact_UseRelic_Behemoth_Buff,
        ShardPrefabs.AB_Interact_UseRelic_Manticore_Buff,
        ShardPrefabs.AB_Interact_UseRelic_Monster_Buff,
        ShardPrefabs.AB_Interact_UseRelic_Paladin_Buff,
    };

    public static void GiveShardBuffsToPlayer(Entity character) {
        foreach (var shardBuff in ShardBuffs) {
            BuffUtil.GiveBuffToPlayer(character, shardBuff);
        }
    }

    public static void RemoveShardBuffsFromPlayer(Entity character) {
        foreach (var shardBuff in ShardBuffs) {
            BuffUtil.RemoveBuffFromPlayer(character, shardBuff);
        }
    }

    public static void RemoveShardBuffsFromPlayerExceptOne(Entity character, PrefabGUID keptBuff) {
        foreach (var shardBuff in ShardBuffs) {
            if (!shardBuff.Equals(keptBuff)) {
                BuffUtil.RemoveBuffFromPlayer(character, shardBuff);
            }
        }
    }

    public static bool IsShardBuffRelated(Entity entity) {
        var entityManager = VWorld.Server.EntityManager;
        if (!entityManager.HasComponent<PrefabGUID>(entity)) {
            return false;
        }
        var prefabGUID = entityManager.GetComponentData<PrefabGUID>(entity);
        foreach (var shardBuffGUID in ShardBuffs) {
            if (prefabGUID.Equals(shardBuffGUID)) {
                return true;
            }
        }
        return false;
    }
    
}
