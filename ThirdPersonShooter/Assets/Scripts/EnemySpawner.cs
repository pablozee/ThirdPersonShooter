using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int numberOfEnemiesInLevel = 1000;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private int numberOfZombieMeshes;
    [SerializeField] private GameObject topRight;
    [SerializeField] private GameObject topLeft;
    [SerializeField] private GameObject bottomRight;
    [SerializeField] private GameObject bottomLeft;
    [SerializeField] private LayerMask groundLayer;

    private NavMeshTriangulation triangulation;
    private int enemyCount;


    // Start is called before the first frame update
    void Start()
    {
        //    StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemyCount < numberOfEnemiesInLevel)
        {
            OnSpawnEnemy(Random.Range(0, numberOfZombieMeshes + 1));
            yield return new WaitForSeconds(1f);
        }
    }

    void OnSpawnEnemy(int spawnIndex)
    {
        float x = Random.Range(topLeft.transform.position.x, topRight.transform.position.x);
        float z = Random.Range(topLeft.transform.position.z, bottomLeft.transform.position.z);

        //  float y = -2.384186f
        float y = 0f;

        Vector3 spawnPosition = new Vector3(x, y, z);
        Debug.Log("Spawn position: " + spawnPosition);
        if (Physics.Raycast(new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z), -transform.up, 2f, groundLayer))
        {
            Debug.Log("Spawning enemy");
            GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity, transform);
            zombie.transform.GetChild(spawnIndex + 1).gameObject.SetActive(true);
            Enemy enemy = zombie.GetComponent<Enemy>();
            enemy.SetEnemySpawner(this);
            enemyCount++;
        }
    }

    public void DecreaseEnemyCount()
    {
        enemyCount--;
    }
}
