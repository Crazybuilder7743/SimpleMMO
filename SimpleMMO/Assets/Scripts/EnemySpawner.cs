
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] List<Vector3> enemySpawnPoints = new List<Vector3>();
    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy),2,6);
    }

    private void SpawnEnemy() 
    {
        int spawnID = Random.Range(0,enemySpawnPoints.Count);
        Enemy instantiatedEnemy = Instantiate(enemy);
        enemies.Add(instantiatedEnemy);
        enemy.transform.position = enemySpawnPoints[spawnID];
        instantiatedEnemy.GetComponent<NetworkObject>().Spawn();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach(var item in enemySpawnPoints) 
        {
            Gizmos.DrawSphere(item,1);
        }
    }
}
