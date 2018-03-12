using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    [Serializable]
    public class JsonPickupSpawnData
    {
        public GameObject pickupPrefab;
        public SpawningZoneStruct spawnZone;
        public float chanceToSpawn;

        public JsonPickupSpawnData(PickupSpawnData spawnData)
        {
            pickupPrefab = spawnData.pickupPrefab;
            chanceToSpawn = spawnData.chanceToSpawn;
            spawnZone = new SpawningZoneStruct(spawnData.topRight.position, spawnData.bottomLeft.position);
        }
    }

    [Serializable]
    public class PickupSpawnData
    {
        public GameObject pickupPrefab;
        public Transform topRight;
        public Transform bottomLeft;
        [Range(0f, 1f)]
        public float chanceToSpawn;
    }

    [Serializable]
    public struct SpawningZoneStruct
    {
        public Vector3 topRight;
        public Vector3 bottomLeft;

        public SpawningZoneStruct(Vector3 topRight, Vector3 bottomLeft)
        {
            this.topRight = topRight;
            this.bottomLeft = bottomLeft;
        }
    }
}