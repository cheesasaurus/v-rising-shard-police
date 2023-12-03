using Bloodstone.API;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;

namespace ShardPolice.Utils;


public static class UserUtil {
    public static NativeArray<User> findAllUsers() {
        var entityManager = VWorld.Server.EntityManager;
        var userType = ComponentType.ReadOnly<User>();
        var query = entityManager.CreateEntityQuery(new ComponentType[]{userType});
        return query.ToComponentDataArray<User>(Allocator.Temp);
    }
}
