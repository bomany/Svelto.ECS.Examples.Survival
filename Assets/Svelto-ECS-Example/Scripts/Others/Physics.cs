using UnityEngine;
namespace Svelto.ECS.Example.Survive
{
    public interface IPhysics
    {
        bool CheckSphere(Vector3 position, float radius, int layerMask = UnityEngine.Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
    }
    public class Physics : IPhysics
    {
        public bool CheckSphere(Vector3 position, float radius, int layerMask = UnityEngine.Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return UnityEngine.Physics.CheckSphere(position, radius, layerMask, queryTriggerInteraction);
        }
    }
}