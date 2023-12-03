using Bloodstone.API;
using ShardPolice.Utils;
using HarmonyLib;
using ProjectM;
using Unity.Collections;
using Unity.Entities;
using System.Collections.Generic;

namespace ShardPolice.Hooks;


[HarmonyPatch(typeof(BuffSystem_Spawn_Server), nameof(BuffSystem_Spawn_Server.OnUpdate))]
public static class ShardBuffSpawnedHook
{
    public static void Prefix(BuffSystem_Spawn_Server __instance)
    {
        if (!ShardPoliceConfig.LimitShardBuffsToOnlyOneAtATime.Value) {
            return;
        }
        var buffTracker = new BuffTracker();
        var events = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
        foreach (var entity in events) {
            buffTracker.BuffWasSpawned(entity);
        }
        foreach (var psb in buffTracker.PlayerShardBuffs()) {
            ShardUtil.RemoveShardBuffsFromPlayerExceptOne(psb.Character, psb.LatestShardBuffGuid);
            Plugin.Logger.LogInfo($"Limited shard buffs for player {psb.CharacterName}");
        }
    }

    /**
     * Used to keep track of players getting shard buffs during a game tick
     */
    private class BuffTracker {
        public class PlayerShardBuff {
            public Entity Character;
            public FixedString64 CharacterName;
            public PrefabGUID LatestShardBuffGuid;
        }

        private EntityManager EntityManager = VWorld.Game.EntityManager;
        private Dictionary<Entity, PlayerShardBuff> _PlayerShardBuffs = new Dictionary<Entity, PlayerShardBuff>();

        public void BuffWasSpawned(Entity entity) {
            if (!ShardUtil.IsShardBuffRelated(entity)) {
                return;
            }
            var entityOwner = EntityManager.GetComponentData<EntityOwner>(entity);
            var buffedCharacter = entityOwner.Owner;
            if (!EntityManager.HasComponent<PlayerCharacter>(buffedCharacter)) {
                return;
            }
            var playerCharacter = EntityManager.GetComponentData<PlayerCharacter>(buffedCharacter);

            PlayerShardBuff playerBuff;
            if (!_PlayerShardBuffs.TryGetValue(buffedCharacter, out playerBuff)) {
                playerBuff = new PlayerShardBuff() {
                    Character = buffedCharacter,
                    CharacterName = playerCharacter.Name
                };
                _PlayerShardBuffs.Add(buffedCharacter, playerBuff);
            }
            playerBuff.LatestShardBuffGuid = EntityManager.GetComponentData<PrefabGUID>(entity);
        }

        public IEnumerable<PlayerShardBuff> PlayerShardBuffs() {
            return _PlayerShardBuffs.Values;
        }
    }

}