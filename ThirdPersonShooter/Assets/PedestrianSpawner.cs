using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pedestrianPrefab;
    [SerializeField] private int pedestriansToSpawn;
    [SerializeField] private GameObject topRight;
    [SerializeField] private GameObject topLeft;
    [SerializeField] private GameObject bottomRight;
    [SerializeField] private GameObject bottomLeft;

    void Start()
    {
        Random.InitState((int)Time.realtimeSinceStartup);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int pedestriansSpawned = 0;
        while (pedestriansSpawned < pedestriansToSpawn)
        {
            float x = Random.Range(topLeft.transform.position.x, topRight.transform.position.x);
            float z = Random.Range(topLeft.transform.position.z, bottomLeft.transform.position.z);

            Vector3 spawnPosition = new Vector3(x, 0.0f, z);

            int sidewalkArea = 3;
            int grassArea = 5;
            int crossingArea = 6;
            int sidewalkAreaMask = 1 << sidewalkArea;
            int grassAreaMask = 1 << grassArea;
            int crossingAreaMask = 1 << crossingArea;
            int finalMask = sidewalkAreaMask | grassAreaMask | crossingAreaMask;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, 5.0f, finalMask))
            {
                GameObject pedestrian = Instantiate(pedestrianPrefab);
                //       Transform spawnWaypoint = transform.GetChild(Random.Range(0, transform.childCount));
                //       pedestrian.GetComponent<PedestrianWaypointNavigator>().currentWaypoint = spawnWaypoint.GetComponent<Waypoint>();
                NavMeshAgent pedestriantAgent = pedestrian.GetComponent<NavMeshAgent>();
                pedestriantAgent.Warp(spawnPosition);
                pedestrian.transform.position = spawnPosition;
                pedestrian.GetComponent<PedestrianController>().walkPoint = spawnPosition;
                pedestriantAgent.speed = Random.Range(1f, 3f);


                pedestriansSpawned++;
            }
        }
        yield return new WaitForEndOfFrame();
    }
}
