using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pedestrianPrefab;
    [SerializeField] private int pedestriansToSpawn;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int pedestriansSpawned = 0;
        while (pedestriansSpawned < pedestriansToSpawn)
        {
            GameObject pedestrian = Instantiate(pedestrianPrefab);
            Transform spawnWaypoint = transform.GetChild(Random.Range(0, transform.childCount));
            pedestrian.GetComponent<PedestrianWaypointNavigator>().currentWaypoint = spawnWaypoint.GetComponent<Waypoint>();
            pedestrian.transform.position = spawnWaypoint.position;

            yield return new WaitForEndOfFrame();

            pedestriansSpawned++;
        }
    }
}
