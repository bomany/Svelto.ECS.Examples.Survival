using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerEntityView : EntityView
    {
        public IPlayerInputComponent inputComponent;
        
        public ISpeedComponent         speedComponent;
        public IRigidBodyComponent     rigidBodyComponent;
        public IPositionComponent      positionComponent;
        public IAnimationComponent     animationComponent;
        public ITransformComponent     transformComponent;
    }

    public interface IPlayerInputComponent
    {
        Vector3 input { get; set; }
        Ray camRay { get; set; }
        bool fire { get; set; }
        bool special { get; set; }
    }

    public class PlayerTargetEntityView : EntityView
    {
        public IPlayerTargetComponent     playerTargetComponent;
        public IPositionComponent         positionComponent;
        public IRigidBodyComponent        rigidBodyComponent;
    }
}

namespace Svelto.ECS.Example.Survive.Player.Gun
{
    public class GunEntityView : EntityView
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
    }
}

namespace Svelto.ECS.Example.Survive.Player.Special
{
    public class SpecialEntityView : EntityView
    {
        public IPlayerSpecialAtributesComponent     specialComponent;
        public IPlayerSpecialTriggerComponent       triggerComponent;
    }
}



