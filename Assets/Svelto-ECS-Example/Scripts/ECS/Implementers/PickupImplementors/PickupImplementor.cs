using UnityEngine;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Player.Pickup
{
    public class PickupImplementor : MonoBehaviour, IPickupAttributesComponent, IDestroyComponent, IImplementor
    {
        public int Amount;
        public int amount { get { return Amount; } }

        bool _empty;
        public bool empty { get { return _empty; } set { _empty = value; } }

        public PickupType PickupType;
        public PickupType pickupType { get { return PickupType; } }

        void Awake()
        {
            destroyed = new DispatchOnChange<bool>(GetInstanceID());
            destroyed.NotifyOnValueSet(OnDestroyed);
        }

        void OnDestroyed(int sender, bool isDestroyed)
        {
            Destroy(gameObject);
        }

        public DispatchOnChange<bool> destroyed { get; private set; }
    }
}