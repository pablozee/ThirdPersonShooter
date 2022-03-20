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
    private Collider col;
    private GameObject childCol;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerArmature");
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<PedestrianController>();
        col = GetComponent<Collider>();
        childCol = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewportPosition = cam.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > -1 && viewportPosition.x < 1)
        {
            if (viewportPosition.y > -1 && viewportPosition.y < 1)
            {
                if (viewportPosition.z > 0)
                {
                    agent.enabled = true;
                    controller.agentActive = true;
                    controller.enabled = true;
               //     col.enabled = true;
                 //   childCol.SetActive(true);
                }
            }
        }
        else
        {
            agent.enabled = false;
            controller.agentActive = false;
            controller.enabled = false;
       //     col.enabled = false;
       //     childCol.SetActive(false);

        }

        if (Vector3.Distance(transform.position, player.transform.position) < 30)
        {
            agent.enabled = true;
            controller.agentActive = true;
            controller.enabled = true;
         //   col.enabled = true;
           // childCol.SetActive(true);
        }
    }
}
