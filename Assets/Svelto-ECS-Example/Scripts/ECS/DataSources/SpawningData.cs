using System.IO;
using Svelto.ECS.Example.Survive;
using UnityEngine;

[ExecuteInEditMode]
public class SpawningData : MonoBehaviour
{
    static private bool serializedOnce;

    void Awake()
    {
        if (serializedOnce == false)
        {
            SerializeData();
        }
    }
    public void SerializeData()
    {
        serializedOnce = true;
        var enemyData = GetComponents<EnemySpawnDataSource>();
        if (enemyData.Length > 0)
        {
            JSonEnemySpawnData[] spawningdata = new JSonEnemySpawnData[enemyData.Length];

            for (int i = 0; i < enemyData.Length; i++)
                spawningdata[i] = new JSonEnemySpawnData(enemyData[i].spawnData);

            var json = JsonHelper.arrayToJson(spawningdata);

            Utility.Console.Log(json);

            File.WriteAllText(Application.persistentDataPath + "/EnemySpawningData.json", json);
        }

        var pickupData = GetComponents<PickupSpawnDataSource>();
        if (pickupData.Length > 0)
        {
            JsonPickupSpawnData[] spawningdata = new JsonPickupSpawnData[pickupData.Length];

            for (int i = 0; i < pickupData.Length; i++)
                spawningdata[i] = new JsonPickupSpawnData(pickupData[i].spawnData);

            var json = JsonHelper.arrayToJson(spawningdata);

            Utility.Console.Log(json);

            File.WriteAllText(Application.persistentDataPath + "/PickupSpawningData.json", json);

        }
    }
}