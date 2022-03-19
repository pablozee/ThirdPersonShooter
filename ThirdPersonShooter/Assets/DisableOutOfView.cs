using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DisableOutOfView : MonoBehaviour
{

    private GameObject player;
    private Camera cam;
    private NavMeshAgent agent;
    private PedestrianController controller;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerArmature");
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<PedestrianController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 0 && viewportPosition.x < 1)
        {
            if (viewportPosition.y > 0 && viewportPosition.y < 1)
            {
                if (viewportPosition.z > 0)
                {
                    agent.enabled = true;
                    controller.agentActive = true;
                }
            }
        }
        else
        {
            agent.enabled = false;
            controller.agentActive = false;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 30)
        {
            agent.enabled = true;
            controller.agentActive = true;
        }
    }
}
