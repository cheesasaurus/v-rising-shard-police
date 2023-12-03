using System.Collections.Generic;
using Bloodstone.API;
using Il2CppSystem;
using ProjectM;
using ProjectM.Gameplay;
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

    private static readonly Dictionary<PrefabGUID, PrefabGUID> ShardItemForBuilding = new Dictionary<PrefabGUID, PrefabGUID>() {
        { ShardPrefabs.TM_Relic_SoulShard_Behemoth, ShardPrefabs.Item_Building_Relic_Behemoth },
        { ShardPrefabs.TM_Relic_SoulShard_Manticore, ShardPrefabs.Item_Building_Relic_Manticore },
        { ShardPrefabs.TM_Relic_SoulShard_Monster, ShardPrefabs.Item_Building_Relic_Monster },
        { ShardPrefabs.TM_Relic_SoulShard_Paladin, ShardPrefabs.Item_Building_Relic_Paladin },
    };

    public static void PrepareShardItemsToDespawn() {
        var entityManager = VWorld.Server.EntityManager;
        var query = entityManager.CreateEntityQuery(new ComponentType[]{
            ComponentType.ReadOnly<Relic>(),
            ComponentType.ReadOnly<LifeTime>(),
        });
        foreach (var entity in query.ToEntityArray(Allocator.Temp)) {
            PrepareShardItemToDespawn(entity);
        }
    }

    public static void CheckItems() {
        var entityManager = VWorld.Server.EntityManager;

        var query = entityManager.CreateEntityQuery(new EntityQueryDesc() {
            All = new ComponentType[] {
                ComponentType.ReadOnly<Relic>(),
                ComponentType.ReadOnly<ItemData>(),
            },
            Options = EntityQueryOptions.IncludeDisabled
        });
        int count = 0;
        int sussyCount = 0;
        foreach (var entity in query.ToEntityArray(Allocator.Temp)) {
            if (!ShouldBeShardItem(entity)) {
                continue;
            }
            count++;
            if (!entityManager.HasComponent<Relic>(entity)) {
                sussyCount++;
            }
        }
        Plugin.Logger.LogMessage($"Found {count} items, {sussyCount} were sus");
    }

    public static bool ShouldBeShardItem(Entity entity) {
        var entityManager = VWorld.Server.EntityManager;
        if (!entityManager.TryGetComponentData<PrefabGUID>(entity, out var prefabGUID)) {
            return false;
        }
        foreach (var guid in ShardItems) {
            if (prefabGUID.Equals(guid)) {
                return true;
            }
        }
        return false;
    }

    public static void AppropriatePlacedShards(Entity character) {
        // todo

        var entityManager = VWorld.Server.EntityManager;
        var query = entityManager.CreateEntityQuery(new ComponentType[]{
            ComponentType.ReadOnly<Relic>(),
            ComponentType.ReadOnly<BlueprintData>(),
        });

        int count = 0;
        foreach (var shardBuildingEntity in query.ToEntityArray(Allocator.Temp)) {
            count++;
            entityManager.TryGetComponentData<PrefabGUID>(shardBuildingEntity, out var buildingPrefabGUID);
            Plugin.Logger.LogMessage(buildingPrefabGUID);

            if (ShardItemForBuilding.TryGetValue(buildingPrefabGUID, out var itemPrefabGUID)) {
                ItemUtil.TryDropSpawnedItem(character, itemPrefabGUID, 1);
                var result = ItemUtil.GiveItemToPlayer(character, itemPrefabGUID, 1);
                if (result.Success) {
                    var item = result.NewEntity;
                    entityManager.RemoveComponent<Disabled>(item);
                    // PrepareShardItemToDespawn(item);
                    // TODO: something isn't quite right with the item created this way.
                    // after its been dropped (naturally, or via code below) it can't be found by PrepareShardItemsToDespawn

                    /*
                    if (ItemUtil.TryDropItem(character, itemPrefabGUID, 1)) {
                        Plugin.Logger.LogMessage("dropped item");
                    }
                    DestroyUtility.Destroy(entityManager, shardBuildingEntity, DestroyDebugReason.Consume);
                    */
                }
            }
            
        }
        Plugin.Logger.LogMessage($"found {count} placed relics");
    }

    private static void PrepareShardItemToDespawn(Entity entity) {
        var entityManager = VWorld.Server.EntityManager;
        if (entityManager.TryGetComponentData<LifeTime>(entity, out var lifeTime)) {
            lifeTime.Duration = 0;
            entityManager.SetComponentData(entity, lifeTime);
        }
    }

}
