using Bloodstone.API;
using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using ProjectM.Shared;
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
        if (NetworkedEntityUtil.TryFindEntity(data.Target, out var tileEntity)) {
            if (entityManager.HasComponent<Relic>(tileEntity)) {
                HandleShardWillBeDismantled(job, tileEntity);
            }
        }
    }

    private static void HandleTileWillBeMoved(Entity job) {
        var entityManager = VWorld.Server.EntityManager;
        var data = entityManager.GetComponentData<MoveTileModelEvent>(job);
        if (NetworkedEntityUtil.TryFindEntity(data.Target, out var tileEntity)) {
            if (entityManager.HasComponent<Relic>(tileEntity)) {
                HandleShardWillBeMoved(job, tileEntity);
            }
        }
    }

    private static void HandleShardWillBeDismantled(Entity job, Entity tileEntity) {
        Plugin.Logger.LogMessage("relic will be dismantled");
        SystemPatchUtil.CancelJob(job);
        SendMessageToActingUser(job, "Cannot dismantle placed shard during raid hours!");
        
    }

    private static void HandleShardWillBeMoved(Entity job, Entity tileEntity) {
        Plugin.Logger.LogMessage("relic will be moved");
        SystemPatchUtil.CancelJob(job);
        SendMessageToActingUser(job, "Cannot move placed shard during raid hours!");
    }

    private static void SendMessageToActingUser(Entity job, string message) {
        var entityManager = VWorld.Server.EntityManager;
        var fromCharacter = entityManager.GetComponentData<FromCharacter>(job);
        var user = entityManager.GetComponentData<User>(fromCharacter.User);
        ServerChatUtils.SendSystemMessageToClient(entityManager, user, message);
    }

}