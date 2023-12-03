using Bloodstone.API;
using ProjectM;
using ShardPolice.Prefabs;
using Unity.Entities;

namespace ShardPolice.Utils;

public static class ShardItemUtil {

    private static readonly PrefabGUID[] ShardItems = {
        ShardPrefabs.Item_Building_Relic_Behemoth,
        ShardPrefabs.Item_Building_Relic_Manticore,
        ShardPrefabs.Item_Building_Relic_Monster,
        ShardPrefabs.Item_Building_Relic_Paladin,
    };

    public static void FindShardItemsFromPlayer(Entity character) {
        var entityManager = VWorld.Server.EntityManager;
        InventoryUtilities.TryGetMainInventoryEntity(entityManager, character, out var mainInventoryEntity);
        foreach (var shardItem in ShardItems) {
            var found = InventoryUtilities.HasItemInInventory(entityManager, mainInventoryEntity, shardItem, 1);
            if (found) {
                Plugin.Logger.LogMessage("found a shard in an inventory");
                // todo: something
            }
        }
    }
}
