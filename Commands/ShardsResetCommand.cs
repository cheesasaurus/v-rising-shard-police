using Bloodstone.API;
using ProjectM;
using ShardPolice.Utils;
using VampireCommandFramework;

namespace ShardPolice.Commands;


public class ShardsResetCommand {

    [Command("shards-reset", description: "reset all shards", adminOnly: true)]
    public void Execute(ChatCommandContext ctx) {
        foreach (var user in UserUtil.FindAllUsers()) {
            ShardBuffUtil.TryRemoveShardBuffsFromPlayer(user.LocalCharacter._Entity);
        }
        ShardItemUtil.PrepareShardItemsToDespawn();
        // TODO: despawn placed shards
        
        ServerChatUtils.SendSystemMessageToAllClients(VWorld.Server.EntityManager, "Shards have been reset!");
    }

}
