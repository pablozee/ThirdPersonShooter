using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianController : MonoBehaviour
{
    public bool reachedDestination;
    
    private NavMeshAgent agent;
    private Vector3 destination;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!reachedDestination) CheckPathComplete();    
        MovementAnimations();
    }

    void MovementAnimations()
    {
        animator.SetFloat("MoveMagnitude", agent.velocity.magnitude);
    }

    void CheckPathComplete()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    reachedDestination = true;
                }
            }
        }
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
        this.destination = destination;
        reachedDestination = false;
    }
}
