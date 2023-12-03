using ShardPolice.Utils;
using VampireCommandFramework;

namespace ShardPolice.Commands;


public class ShardBuffsRemoveCommand {

    [Command("shard-buffs-remove", shortHand: "sbr", description: "remove shard buffs from self", adminOnly: true)]
    public void Execute(ChatCommandContext ctx) {
        ShardUtil.RemoveShardBuffsFromPlayer(ctx.User.LocalCharacter._Entity);
        ctx.Reply("Removed all shard buffs from self");
    }

}
