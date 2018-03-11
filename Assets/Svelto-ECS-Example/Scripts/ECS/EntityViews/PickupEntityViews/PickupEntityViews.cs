namespace Svelto.ECS.Example.Survive.Player.Pickup
{
    public class PickupEntityView : EntityView
    {
        public IPickupAttributesComponent pickupComponent;
        public IPickupTriggerComponent targetTriggerComponent;
        public IDestroyComponent destroyComponent;
    }
}