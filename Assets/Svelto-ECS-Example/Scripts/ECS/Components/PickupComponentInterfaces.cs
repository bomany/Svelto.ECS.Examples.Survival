using UnityEngine;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Player.Pickup
{
    public interface IPickupAttributesComponent : IComponent
    {
        int amount { get; }
        bool empty { get; set; }
        PickupType pickupType { get; }
    }

    public interface IPickupTriggerComponent : IComponent
    {
        PickupCollisionData entityInRange { get; }
    }

    public struct PickupCollisionData
    {
        public int otherEntityID;
        public bool collides;

        public PickupCollisionData(int otherEntityID, bool collides)
        {
            this.otherEntityID = otherEntityID;
            this.collides = collides;
        }
    }
}
namespace Svelto.ECS.Example.Survive
{
    public enum PickupType
    {
        Ammo = 1,
        Health = 2
    }

    public struct PickupInfo
    {
        public int amount { get; private set; }
        public int triggerEntityID { get; private set; }
        public int entityID { get; private set; }

        public PickupInfo(int amount, int triggerEntityID, int entityID)
        {
            this.amount = amount;
            this.triggerEntityID = triggerEntityID;
            this.entityID = entityID;
        }
    }
}