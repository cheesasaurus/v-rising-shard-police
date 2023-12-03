using Bloodstone.API;
using ProjectM;
using ProjectM.Shared;
using ShardPolice.Prefabs;
using Unity.Collections;
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
        NativeList<Entity> inventoryEntities = new NativeList<Entity>(Allocator.Temp);
        InventoryUtilities.TryGetInventoryEntities(entityManager, character, ref inventoryEntities);
        foreach (var shardItemPrefab in ShardItems) {
            var count = InventoryUtilities.GetItemAmount(entityManager, inventoryEntities, shardItemPrefab);
            // var found = InventoryUtilities.HasItemInInventory(entityManager, mainInventoryEntity, shardItemPrefab, 1);
            if (count > 0) {
                Plugin.Logger.LogMessage("found a shard in an inventory");
                var slotIndex = InventoryUtilities.GetItemSlot(entityManager, character, shardItemPrefab);
                Plugin.Logger.LogMessage($"slot#{slotIndex}");
                InventoryUtilities.TryGetItemAtSlot(entityManager, character, slotIndex, out var item);

                var entity = item.ItemEntity._Entity;

                entityManager.TryGetComponentData<LifeTime>(entity, out var lifeTime);
                Plugin.Logger.LogMessage($"duration {lifeTime.Duration}");
                lifeTime.Duration = 0;
                entityManager.SetComponentData<LifeTime>(entity, lifeTime);

                
                
                // todo: something
            }
        }
    }
}
