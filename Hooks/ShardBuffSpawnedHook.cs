using Bloodstone.API;
using ShardPolice.Utils;
using HarmonyLib;
using ProjectM;
using Unity.Collections;
using Unity.Entities;

namespace ShardPolice.Hooks;


[HarmonyPatch(typeof(BuffSystem_Spawn_Server), nameof(BuffSystem_Spawn_Server.OnUpdate))]
public static class ShardBuffSpawnedHook
{
    public static void Prefix(BuffSystem_Spawn_Server __instance)
    {
        if (!ShardPoliceConfig.LimitShardBuffsToOnlyOneAtATime.Value) {
            return;
        }
        var events = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
        foreach (var entity in events)
        {
            if (ShardUtil.IsShardBuffRelated(entity)) {
                HandleShardBuffSpawned(entity);
            }
        }
    }

    private static void HandleShardBuffSpawned(Entity entity) {
        var entityManager = VWorld.Game.EntityManager;
        var buffPrefabGuid = entityManager.GetComponentData<PrefabGUID>(entity);
        var entityOwner = entityManager.GetComponentData<EntityOwner>(entity);

        if (entityManager.HasComponent<PlayerCharacter>(entityOwner.Owner)) {
            var playerCharacter = entityManager.GetComponentData<PlayerCharacter>(entityOwner.Owner);
            ShardUtil.RemoveShardBuffsFromPlayerExceptOne(entityOwner.Owner, buffPrefabGuid);
            Plugin.Logger.LogInfo($"Limited shard buffs for player {playerCharacter.Name}");
        }
    }

}