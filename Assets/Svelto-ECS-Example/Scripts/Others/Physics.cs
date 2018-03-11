using UnityEngine;
using System.Collections.Generic;
namespace Svelto.ECS.Example.Survive
{
    public interface IPhysics
    {
        bool CheckSphere(Vector3 position, float radius, int layerMask);
        int[] OverlapSphere(Vector3 position, float radius, int layerMask);
    }
    public class Physics : IPhysics
    {
        public bool CheckSphere(Vector3 position, float radius, int layerMask)
        {
            return UnityEngine.Physics.CheckSphere(position, radius, layerMask);
        }

        public int[] OverlapSphere(Vector3 position, float radius, int layerMask)
        {
            UnityEngine.Collider[] colliders = UnityEngine.Physics.OverlapSphere(position, radius, layerMask);
            List<int> ids = new List<int>();

            for (var i = 0; i <= colliders.Length-1; i++)
                ids.Add(colliders[i].gameObject.GetInstanceID());
     
            return ids.ToArray();
        }
    }
}