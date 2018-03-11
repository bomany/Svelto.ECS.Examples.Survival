﻿using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    [Serializable]
    public class JsonPickupSpawnData
    {
        public GameObject pickupPrefab;
        public SpawningZoneStruct spawnZone;
        //Radius of the object plus extra padding.
        public float sizeRadius;

        public JsonPickupSpawnData(PickupSpawnData spawnData)
        {
            pickupPrefab = spawnData.pickupPrefab;
            spawnZone = new SpawningZoneStruct(spawnData.topRight.position, spawnData.bottomLeft.position);
        }
    }

    [Serializable]
    public class PickupSpawnData
    {
        public GameObject pickupPrefab;
        public Transform topRight;
        public Transform bottomLeft;
        public float sizeRadius;
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