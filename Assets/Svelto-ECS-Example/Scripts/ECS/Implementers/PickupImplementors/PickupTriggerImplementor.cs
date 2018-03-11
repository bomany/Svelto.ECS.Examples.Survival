using UnityEngine;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Player.Pickup
{
    public class PickupTriggerImplementor : MonoBehaviour, IPickupTriggerComponent, IImplementor
    {
        public PickupCollisionData entityInRange { get; private set; }

        void OnTriggerEnter(Collider other)
        {
            entityInRange = new PickupCollisionData(other.gameObject.GetInstanceID(), true);
        }

        void OnTriggerExit(Collider other)
        {
            entityInRange = new PickupCollisionData(other.gameObject.GetInstanceID(), false);
        }
    }
}