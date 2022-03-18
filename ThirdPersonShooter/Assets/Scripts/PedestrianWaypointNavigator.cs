using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianWaypointNavigator : MonoBehaviour
{
    private PedestrianController controller;
    public Waypoint currentWaypoint;

    void Awake()
    {
        controller = GetComponent<PedestrianController>(); 
    }

    void Start()
    {
        controller.SetDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        if (controller.reachedDestination)
        {
            currentWaypoint = currentWaypoint.nextWaypoint;
            controller.SetDestination(currentWaypoint.GetPosition());
        }
    }
}
