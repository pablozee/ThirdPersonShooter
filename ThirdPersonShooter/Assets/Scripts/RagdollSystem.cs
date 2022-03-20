using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public enum AnimationState
{
    Animated,
    HitReaction,
    DamageRecover,
    RagdollMode,
    WaitForStable,
    RagdollToAnim
}

[System.Serializable]
public enum BodyPart
{
    Spine, 
    Chest, 
    LeftHip,
    LeftKnee, 
    RightHip, 
    RightKnee, 
    Head, 
    LeftArm, 
    LeftElbow, 
    RightArm, 
    RightElbow, 
    None
}

[System.Serializable]
public class MuscleComponent
{
    public BodyPart bodyPart = BodyPart.None;
    public Transform transform;
    public Quaternion storedRotation;
    public Vector3 storedPosition;
    public Rigidbody rigidbody;
    public Collider collider;
    
    public MuscleComponent(Transform t)
    {
        transform = t;
        rigidbody = t.GetComponent<Rigidbody>();
        collider = t.GetComponent<Collider>();
        Muscle muscle;
        t.TryGetComponent<Muscle>(out muscle);
        if (muscle)
            bodyPart = muscle.bodyPart;

    }
}
public class RagdollSystem : MonoBehaviour
{
    [SerializeField] private AnimationState animState = AnimationState.Animated;
    [SerializeField] private float minHitSpeed = 15f;
    [SerializeField] private float blendAmount = 1f;
    [SerializeField] private float getUpDelay = 2f;
    [SerializeField] private List<MuscleComponent> muscleComponents = new List<MuscleComponent>();
    [SerializeField] private bool isRagdoll, isDamaged;
    [SerializeField] private string getUpFromFrontAnim;
    [SerializeField] private string getUpFromBackAnim;

    private Animator anim;
    private Vector3 hitVelocity;
    private Vector3 hitForce;
    private BodyPart hitPart;
    private PedestrianController controller;
    private NavMeshAgent agent;
    private Transform hips;
    private Transform hipsParent;
    private Rigidbody hipsRB;
    private bool resetNavMeshPosition;
    private float timer = 2f;
    private float blendValue;

    void Start()
    {
        controller = GetComponent<PedestrianController>();
        agent = GetComponent<NavMeshAgent>();
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            muscleComponents.Add(new MuscleComponent(rb.transform));
        ToggleAnimationState(true, true);
    }

    void Update()
    {
        switch (animState)
        {
            case AnimationState.Animated:
                controller.agentActive = true;
                controller.enabled = true;
                agent.enabled = true;
                
                if (resetNavMeshPosition)
                {
                    agent.Warp(transform.position);
                    resetNavMeshPosition = false;
                    agent.speed = Random.Range(1.0f, 3.0f);
                }

                if (hitVelocity.magnitude > minHitSpeed)
                {
                    animState = AnimationState.RagdollMode;
                    controller.agentActive = false;
                    controller.enabled = false;
                    agent.enabled = false;
                    agent.speed = 0;
                    timer = getUpDelay;
                    isRagdoll = true;
                }
                break;
            case AnimationState.HitReaction:
                ToggleAnimationState(false, false);
                Debug.Log("In hit reaction state");
                foreach (MuscleComponent comp in muscleComponents)
                {
                    if (comp.bodyPart == hitPart)
                    {
                        Debug.Log("Applying force to muscle component");
                        comp.rigidbody.AddForce(hitForce, ForceMode.VelocityChange);
                    }
                }
                animState = AnimationState.DamageRecover;
                break;
            case AnimationState.DamageRecover:
                animState = AnimationState.RagdollMode;
                break;
            case AnimationState.RagdollMode:
                controller.agentActive = false;
                controller.enabled = false;
                agent.enabled = false;
                ToggleAnimationState(false, true);
                if (isRagdoll)
                {
                    foreach (MuscleComponent comp in muscleComponents)
                    {
                        comp.rigidbody.AddForce(hitVelocity * 0.005f, ForceMode.Impulse);
                        comp.rigidbody.AddForce(comp.transform.up * 3f, ForceMode.Impulse);
                    }
                    hitVelocity = Vector3.zero;
                    hips.parent = null;
                    transform.position = hips.position;
                }
                if (hipsRB.velocity.magnitude < 5f)
                    timer -= Time.deltaTime;  
                if (isRagdoll || isDamaged)
                    if (timer <= 0.0f) 
                        animState = AnimationState.WaitForStable;
                break;
            case AnimationState.WaitForStable:
                blendValue = blendAmount;
                hips.parent = hipsParent;
                animState = AnimationState.RagdollToAnim;
                if (isRagdoll)
                    GetUp();
                foreach (MuscleComponent component in muscleComponents)
                {
                    component.storedPosition = component.transform.localPosition;
                    component.storedRotation = component.transform.localRotation;
                }
                ToggleAnimationState(true, true);    
                break;
            case AnimationState.RagdollToAnim:
                break;

        }
    }

    private void LateUpdate()
    {
        if(animState == AnimationState.RagdollToAnim)
        {
            blendValue -= Time.deltaTime;
            foreach (MuscleComponent component in muscleComponents)
            {
                component.transform.localPosition = Vector3.Slerp(component.transform.localPosition, component.storedPosition, blendAmount);
                component.transform.localRotation = Quaternion.Slerp(component.transform.localRotation, component.storedRotation, blendAmount);
            }

            if (blendValue <= 0.0f)
            {
                animState = AnimationState.Animated;
                resetNavMeshPosition = true;
                isRagdoll = false;
                isDamaged = false;
                hitForce = Vector3.zero;
                hitPart = BodyPart.None;
            }
        }
    }

    public void Damage(BodyPart hitBodyPart, Vector3 force)
    {
        hitPart = hitBodyPart;
        hitForce = force;
        isDamaged = true;
        animState = AnimationState.HitReaction;
        Debug.Log(hitPart);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].otherCollider.transform != transform.parent)
        {
            if (collision.collider.tag == "Car")
                hitVelocity = collision.relativeVelocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != transform.parent)
        {
            if (other.tag == "Car")
                hitVelocity = other.attachedRigidbody.velocity;
        }
    }

    void ToggleAnimationState(bool isActive, bool gravity)
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
        hips = anim.GetBoneTransform(HumanBodyBones.Hips);
        hipsParent = hips.parent;
        hipsRB = hips.GetComponent<Rigidbody>();
    }

    void GetUp()
    {
        transform.rotation = Quaternion.FromToRotation(transform.forward, RagdollDirection()) * transform.rotation;
        hips.rotation = Quaternion.FromToRotation(RagdollDirection(), transform.forward) * hips.rotation;
        string animationToPlay = CheckIfLieOnBack() ? getUpFromBackAnim : getUpFromFrontAnim;
        anim.Play(animationToPlay, 0, 0);
    }

    Vector3 RagdollDirection()
    {
        Vector3 ragdollDirection = hips.position - anim.GetBoneTransform(HumanBodyBones.Head).position;
        ragdollDirection.y = 0;
        if (CheckIfLieOnBack())
        {
            return ragdollDirection.normalized;
        }
        else
        {
            return -ragdollDirection.normalized;
        }

    }

    bool CheckIfLieOnBack()
    {
        Vector3 leftLeg = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position;
        Vector3 rightLeg = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg).position;
        leftLeg -= hips.position;
        rightLeg -= hips.position;
        leftLeg.y = 0f;
        rightLeg.y = 0f;
        Quaternion rotation = Quaternion.FromToRotation(leftLeg, Vector3.right);
        Vector3 t = rotation * rightLeg;
        return t.z < 0f;
    }
}
