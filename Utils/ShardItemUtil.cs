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

    public static void PrepareShardItemsToDespawn() {
        var entityManager = VWorld.Server.EntityManager;
        var query = entityManager.CreateEntityQuery(new ComponentType[]{
            ComponentType.ReadOnly<Relic>(),
            ComponentType.ReadOnly<LifeTime>(),
        });
        foreach (var entity in query.ToEntityArray(Allocator.Temp)) {
            entityManager.TryGetComponentData<LifeTime>(entity, out var lifeTime);
            lifeTime.Duration = 0;
            entityManager.SetComponentData(entity, lifeTime);
        }
    }

}
