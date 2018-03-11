using System.Collections;
using System.Collections.Generic;
using Svelto.Tasks.Enumerators;
using System.IO;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyWaveSpawnerEngine : IEngine, IStep<DamageInfo>
    {
        public EnemyWaveSpawnerEngine(Factories.IGameObjectFactory gameobjectFactory, IEntityFactory entityFactory)
        {
            _gameobjectFactory = gameobjectFactory;
            _entityFactory = entityFactory;

            _numberOfEnemyAlive = 0;
            _waveCount = 1;
            _dificulty = 5f;

            IncreaseDificulty();

            IntervaledTick().Run();
        }

        void IncreaseDificulty()
        {
            _numberOfEnemyToSpawn = (int)(_waveCount * _dificulty);
        }

        public void Step(ref DamageInfo token, int condition)
        {
            _numberOfEnemyAlive--;
            if (_numberOfEnemyAlive <= 0 && _numberOfEnemyToSpawn <= 0)
            {
                _waveCount++;
                IncreaseDificulty();
            }
        }

        IEnumerator IntervaledTick()
        {
            var enemiestoSpawn = ReadEnemySpawningDataServiceRequest();
            while (true)
            {
                yield return _waitForSecondsEnumerator;

                if (enemiestoSpawn != null)
                {
                    for (int i = enemiestoSpawn.Length - 1; i >= 0 && _numberOfEnemyToSpawn > 0; --i)
                    {
                        var spawnData = enemiestoSpawn[i];

                        if (spawnData.timeLeft <= 0.0f)
                        {
                            int spawnPointIndex = Random.Range(0, spawnData.spawnPoints.Length);

                            var go = _gameobjectFactory.Build(spawnData.enemyPrefab);

                            var data = go.GetComponent<EnemyAttackDataHolder>();

                            List<IImplementor> implementors = new List<IImplementor>();
                            go.GetComponentsInChildren(implementors);
                            implementors.Add(new EnemyAttackImplementor(data.timeBetweenAttacks, data.attackDamage));

                            _entityFactory.BuildEntity<EnemyEntityDescriptor>(
                                    go.GetInstanceID(), implementors.ToArray());

                            var transform = go.transform;
                            var spawnInfo = spawnData.spawnPoints[spawnPointIndex];

                            transform.position = spawnInfo.position;
                            transform.rotation = spawnInfo.rotation;

                            spawnData.timeLeft = spawnData.spawnTime;

                            _numberOfEnemyToSpawn--;
                            _numberOfEnemyAlive++;
                        }

                        spawnData.timeLeft -= 1.0f;
                    }
                }
            }
        }

        static JSonEnemySpawnData[] ReadEnemySpawningDataServiceRequest()
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/EnemySpawningData.json");

            JSonEnemySpawnData[] enemiestoSpawn = JsonHelper.getJsonArray<JSonEnemySpawnData>(json);

            return enemiestoSpawn;
        }

        readonly Factories.IGameObjectFactory _gameobjectFactory;
        readonly IEntityFactory _entityFactory;
        readonly WaitForSecondsEnumerator _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);

        int _numberOfEnemyAlive;
        int _waveCount;
        int _numberOfEnemyToSpawn;
        float _dificulty;

    }
}
