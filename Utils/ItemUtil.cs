using Bloodstone.API;
using ProjectM;
using ProjectM.Shared;
using ShardPolice.Prefabs;
using Unity.Collections;
using Unity.Entities;

namespace ShardPolice.Utils;


public static class ItemUtil {

    public static AddItemResponse GiveItemToPlayer(Entity character, PrefabGUID prefabGUID, int amount) {
        var gameDataSystem = VWorld.Server.GetExistingSystem<GameDataSystem>();
		var addItemSettings = AddItemSettings.Create(VWorld.Server.EntityManager, gameDataSystem.ItemHashLookupMap);
		return InventoryUtilitiesServer.TryAddItem(addItemSettings, character, prefabGUID, amount);
    }

    public static bool TryDropItemFromInventory(Entity character, PrefabGUID prefabGUID, int amount) {
        var entityManager = VWorld.Server.EntityManager;
        var gameDataSystem = VWorld.Server.GetExistingSystem<GameDataSystem>();
        var commandBuffer = VWorld.Server.GetExistingSystem<EntityCommandBufferSystem>().CreateCommandBuffer();

        InventoryUtilities.TryGetMainInventoryEntity(entityManager, character, out var mainInventoryEntity);
        return InventoryUtilitiesServer.TryDropItem(entityManager, commandBuffer, gameDataSystem.ItemHashLookupMap, mainInventoryEntity, prefabGUID, amount);
    }

    public static bool TryDropSpawnedItem(Entity character, PrefabGUID prefabGUID, int amount) {
        var entityManager = VWorld.Server.EntityManager;
        var gameDataSystem = VWorld.Server.GetExistingSystem<GameDataSystem>();
        var commandBuffer = VWorld.Server.GetExistingSystem<EntityCommandBufferSystem>().CreateCommandBuffer();

        InventoryUtilitiesServer.CreateDroppedItemEntity(entityManager, commandBuffer, gameDataSystem.ItemHashLookupMap, character, prefabGUID, amount);

        return true;
    }

}