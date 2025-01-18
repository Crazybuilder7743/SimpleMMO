
using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] List<Vector3> enemySpawnPoints = new List<Vector3>();
    private List<int> usedSpawnPoints = new List<int>();
    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy),2,6);
    }

    public void ReactivateSpawnPoint(int id) 
    {
        usedSpawnPoints.Remove(id);
    }

    private void SpawnEnemy() 
    {
        if(usedSpawnPoints.Count >= enemySpawnPoints.Count) 
        {
            return; // no free spawnpoint
        }
        int spawnID = 0;
        do spawnID = Random.Range(0,enemySpawnPoints.Count); while (usedSpawnPoints.Contains(spawnID));
        Enemy instantiatedEnemy = Instantiate(enemy);
        instantiatedEnemy.SetOwnerAndSpawnID(this,spawnID);
        enemy.transform.position = this.transform.position + enemySpawnPoints[spawnID];
        instantiatedEnemy.GetComponent<NetworkObject>().Spawn();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach(var item in enemySpawnPoints) 
        {
            Gizmos.DrawSphere(this.transform.position + item,1);
        }
    }
}
