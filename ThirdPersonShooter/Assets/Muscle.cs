using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muscle : MonoBehaviour
{
    public BodyPart bodyPart = BodyPart.None;
    public bool critical = false;

    private RagdollSystem ragdoll;

    void Start()
    {
        ragdoll = GetComponentInParent<RagdollSystem>();
    }

    void Update()
    {

    }

    public void GetDamage(Vector3 force)
    {
        ragdoll.Damage(bodyPart, force);
    }
}