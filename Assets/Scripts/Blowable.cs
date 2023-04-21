using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Blowable : MonoBehaviour
{
    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Blow(Vector3 originForward, float strength)
    {
        rb.isKinematic = false;
        rb.AddForce(originForward.normalized * strength,ForceMode.Impulse);
    }
}
