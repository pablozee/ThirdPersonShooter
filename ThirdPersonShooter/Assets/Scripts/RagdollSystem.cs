using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public enum AnimationState
{
    Animated,
    RagdollMode,
    WaitForStable,
    RagdollToAnim
}

[System.Serializable]
public class MuscleComponent
{
    public Transform transform;
    public Rigidbody rigidbody;
    public Collider collider;
    
    public MuscleComponent(Transform t)
    {
        transform = t;
        rigidbody = t.GetComponent<Rigidbody>();
        collider = t.GetComponent<Collider>();
    }
}
public class RagdollSystem : MonoBehaviour
{
    [SerializeField] private AnimationState animState = AnimationState.Animated;
    [SerializeField] private float minHitSpeed = 15f;
    [SerializeField] private List<MuscleComponent> muscleComponents = new List<MuscleComponent>();
    [SerializeField] private bool isRagdoll;

    private Animator anim;
    private Vector3 hitVelocity;
    private PedestrianController controller;
    private NavMeshAgent agent;

    void Start()
    {
        controller = GetComponent<PedestrianController>();
        agent = GetComponent<NavMeshAgent>();
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            muscleComponents.Add(new MuscleComponent(rb.transform));
        SetRagdollPart(true, true);
    }

    void Update()
    {
        switch (animState)
        {
            case AnimationState.Animated:
                controller.agentActive = true;
                controller.enabled = true;
                agent.enabled = true;
                if (hitVelocity.magnitude > minHitSpeed)
                {
                    animState = AnimationState.RagdollMode;
                    controller.agentActive = false;
                    controller.enabled = false;
                    agent.enabled = false;
                }
                break;
            case AnimationState.RagdollMode:
                controller.agentActive = false;
                controller.enabled = false;
                agent.enabled = false;
                SetRagdollPart(false, true);
                foreach (MuscleComponent comp in muscleComponents)
                    comp.rigidbody.AddForce(hitVelocity * 0.1f, ForceMode.Impulse);
                if (isRagdoll) animState = AnimationState.WaitForStable;
                break;
            case AnimationState.WaitForStable:
                break;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].otherCollider.transform != transform.parent)
        {
            if (collision.collider.tag == "Car")
                hitVelocity = collision.relativeVelocity;
        }
    }

    void SetRagdollPart(bool isActive, bool gravity)
    {
        if (anim) anim.enabled = isActive;
        foreach (MuscleComponent comp in muscleComponents)
        {
            comp.rigidbody.useGravity = gravity;

            if (comp.transform == transform)
            {
                comp.collider.isTrigger = !isActive;
                comp.rigidbody.isKinematic = isActive;
                continue;
            }

            comp.collider.isTrigger = isActive;
            comp.rigidbody.isKinematic = isActive;
        }
    }

    public void AssignAnimator()
    {
        anim = GetComponentInChildren<Animator>();
    }
}
