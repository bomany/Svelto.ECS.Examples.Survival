using System.Collections;
using System.Collections.Generic;
using Svelto.Tasks.Enumerators;
using System.IO;
using Svelto.Tasks;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyWaveSpawnerEngine : IEngine,
                                          IStep<DamageInfo>
    { 

        public EnemyWaveSpawnerEngine(Factories.IGameObjectFactory gameobjectFactory,
                                        IEntityFactory entityFactory,
                                        ISequencer waveSpawnerSequence)
        {
            _gameobjectFactory = gameobjectFactory;
            _entityFactory = entityFactory;
            _waveSpawnerSequence = waveSpawnerSequence;

            _numberOfEnemyAlive = 0;
            _waveCount = 0;
            _dificulty = 5f;

            _enemiestoSpawn = ReadEnemySpawningDataServiceRequest();

            WaitForWave().Run();
        }

        IEnumerator WaitForWave()
        {
            var waitTime = new WaitForSecondsEnumerator(5);
            yield return waitTime;
            SetupNewWave();
        }

        void SetupNewWave()
        {
            _waveCount++;
            _numberOfEnemyToSpawn = (int)(_waveCount * _dificulty);
            //_numberOfEnemyToSpawn = (int)(System.Math.Log(_waveCount) * _dificulty) + _dificulty;

            var waveInfo = new WaveInfo(_waveCount, _numberOfEnemyToSpawn);
            SpawnWave().Run();
            _waveSpawnerSequence.Next(this, ref waveInfo);
        }

        int DamageIncrease(int baseDamage)
        {
            return baseDamage + (int)(System.Math.Log(_waveCount)*5);
        }

        public void Step(ref DamageInfo token, int condition)
        {
            _numberOfEnemyAlive--;
            if (_numberOfEnemyAlive <= 0 && _numberOfEnemyToSpawn <= 0)
            {
                SetupNewWave();
            }
        }

        IEnumerator SpawnWave()
        {
            while (_numberOfEnemyToSpawn > 0)
            {
                yield return _waitForSecondsEnumerator;

                if (_enemiestoSpawn != null)
                {
                    for (int i = _enemiestoSpawn.Length - 1; i >= 0 && _numberOfEnemyToSpawn > 0; --i)
                    {
                        var spawnData = _enemiestoSpawn[i];

                        if (spawnData.timeLeft <= 0.0f)
                        {
                            int spawnPointIndex = Random.Range(0, spawnData.spawnPoints.Length);

                            var go = _gameobjectFactory.Build(spawnData.enemyPrefab);

                            var data = go.GetComponent<EnemyAttackDataHolder>();

                            List<IImplementor> implementors = new List<IImplementor>();
                            go.GetComponentsInChildren(implementors);
                            implementors.Add(new EnemyAttackImplementor(data.timeBetweenAttacks, DamageIncrease(data.attackDamage)));

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
        readonly ISequencer _waveSpawnerSequence;

        readonly WaitForSecondsEnumerator _waitForSecondsEnumerator = new WaitForSecondsEnumerator(1);

        int _numberOfEnemyAlive;
        int _waveCount;
        int _numberOfEnemyToSpawn;
        float _dificulty;
        JSonEnemySpawnData[] _enemiestoSpawn;

    }
}
