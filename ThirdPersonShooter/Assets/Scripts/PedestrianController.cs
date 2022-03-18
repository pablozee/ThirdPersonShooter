using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianController : MonoBehaviour
{
    public bool reachedDestination;

    [SerializeField] private float walkPointRange;
    [SerializeField] public Vector3 walkPoint;
    private NavMeshAgent agent;
    private Vector3 destination;
    private Animator animator;
    private bool walkPointSet;


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
        if (walkPointSet) CheckPathComplete();    
        MovementAnimations();
        Roam();
    }

    void MovementAnimations()
    {
        animator.SetFloat("MoveMagnitude", agent.velocity.magnitude);
    }

    void Roam()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        int sidewalkArea = 3;
        int grassArea = 5;
        int crossingArea = 6;
        int sidewalkAreaMask = 1 << sidewalkArea;
        int grassAreaMask = 1 << grassArea;
        int crossingAreaMask = 1 << crossingArea;
        int finalMask = sidewalkAreaMask | grassAreaMask | crossingAreaMask;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(walkPoint, out hit, 5.0f, finalMask))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }

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
                    walkPointSet = false;
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
