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
            
            ShardItemUtil.FindShardItemsFromPlayer(user.LocalCharacter._Entity);
        }
        // WIP

        // TODO: despawn shards in inventories
        // TODO: despawn placed shards
        // TODO: despawn shards on ground
        ServerChatUtils.SendSystemMessageToAllClients(VWorld.Server.EntityManager, "Shards have been reset!");
    }

}
