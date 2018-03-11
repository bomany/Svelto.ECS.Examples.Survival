using System.Collections;
using System.Collections.Generic;
using Svelto.Tasks.Enumerators;
using System.IO;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Player.Pickup
{
    public class PickupSpawnerEngine : IEngine
    {
        public PickupSpawnerEngine(Factories.IGameObjectFactory gameobjectFactory, IEntityFactory entityFactory, IPhysics physics)
        {
            _gameobjectFactory = gameobjectFactory;
            _entityFactory = entityFactory;
            _physics = physics;
            _max_pickups = 10;

            IntervaledTick().Run();
        }

        IEnumerator IntervaledTick()
        {
            var pickuptoSpawn = ReadPickupSpawningDataServiceRequest();
            while (true)
            {
                yield return _waitForSecondsEnumerator;

                if (pickuptoSpawn != null)
                {
                    var index = Random.Range(0, pickuptoSpawn.Length - 1);
                    var pickupData = pickuptoSpawn[index];

                    var topSpawnZone = pickupData.spawnZone.topRight;
                    var bottomSpawnZone = pickupData.spawnZone.bottomLeft;
                    Vector3 spawnPoint = GetSpawnPoint(topSpawnZone, bottomSpawnZone, pickupData.sizeRadius);

                    var go = _gameobjectFactory.Build(pickupData.pickupPrefab);

                    List<IImplementor> implementors = new List<IImplementor>();
                    go.GetComponentsInChildren(implementors);

                    _entityFactory.BuildEntity<PickupEntityDescriptor>(
                                go.GetInstanceID(), implementors.ToArray());

                    go.transform.position = spawnPoint;

                }
            }
        }

        Vector3 GetSpawnPoint(Vector3 top, Vector3 bottom, float radius)
        {
            bool isBlocked = true;
            Vector3 spawnPoint = Vector3.zero;

            while (isBlocked)
            {
                spawnPoint = new Vector3(
                        Random.Range(top.x, bottom.x),
                        Random.Range(top.y, bottom.y),
                        Random.Range(top.z, bottom.z)
                    );
                isBlocked = _physics.CheckSphere(spawnPoint, radius, SHOOTABLE_MASK);
            }
            return spawnPoint;
        }

        static JsonPickupSpawnData[] ReadPickupSpawningDataServiceRequest()
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/PickupSpawningData.json");

            JsonPickupSpawnData[] pickuptoSpawn = JsonHelper.getJsonArray<JsonPickupSpawnData>(json);

            return pickuptoSpawn;
        }

        readonly WaitForSecondsEnumerator _waitForSecondsEnumerator = new WaitForSecondsEnumerator(10);

        static readonly int SHOOTABLE_MASK = LayerMask.GetMask("Shootable");

        readonly Factories.IGameObjectFactory _gameobjectFactory;
        readonly IEntityFactory _entityFactory;
        readonly IPhysics _physics;
        int _max_pickups;
    }
}
