using UnityEngine;
using Svelto.Tasks;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Player.Pickup
{
    public class PickupDestroyEngine : IQueryingEntityViewEngine, IStep<PickupInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        { }

        public PickupDestroyEngine(IEntityFunctions entityFunctions)
        {
            _entityFunctions = entityFunctions;
        }

        public void Step(ref PickupInfo pickup, int type)
        {
            var entity = entityViewsDB.QueryEntityView<PickupEntityView>(pickup.entityPickupID);
            entity.destroyComponent.destroyed.value = true;
            _entityFunctions.RemoveEntity(entity.ID);
        }

        readonly IEntityFunctions _entityFunctions;
    }
}
