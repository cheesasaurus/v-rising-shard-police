using Bloodstone.API;
using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using ShardPolice.Utils;
using Unity.Collections;
using Unity.Entities;

namespace ShardPolice.Hooks;

[HarmonyPatch(typeof(PlaceTileModelSystem), nameof(PlaceTileModelSystem.OnUpdate))]
public static class PlacedShardWillBeMovedOrDismantledHook {
    public static void Prefix(PlaceTileModelSystem __instance) {
        var dismantleJobs = __instance._DismantleTileQuery.ToEntityArray(Allocator.Temp);
        foreach (var job in dismantleJobs) {
            HandleTileWillBeDismantled(job);
        }

        var moveJobs = __instance._MoveTileQuery.ToEntityArray(Allocator.Temp);
        foreach (var job in moveJobs) {
            HandleTileWillBeMoved(job);
        }        
    }

    private static void HandleTileWillBeDismantled(Entity job) {
        var entityManager = VWorld.Server.EntityManager;
        var data = entityManager.GetComponentData<DismantleTileModelEvent>(job);
        var networkId = data.Target;
        // todo: how to efficiently get target entity?

        // FromCharacter
        // DismantleTileModelEvent
        // NetworkEventType
        // ReceiveNetworkEventTag
    }

    private static void HandleTileWillBeMoved(Entity job) {
        var entityManager = VWorld.Server.EntityManager;
        var data = entityManager.GetComponentData<MoveTileModelEvent>(job);
        var networkId = data.Target;
        // todo: how to efficiently get target entity?

        // FromCharacter
        // MoveTileModelEvent
        // NetworkEventType
        // ReceiveNetworkEventTag
    }

}