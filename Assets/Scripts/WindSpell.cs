using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpell : MonoBehaviour
{
    public Transform origin;
    public float sphereRange = 0.1f;
    public LayerMask hittableLayers;
    public float strength = 100f;
    public ParticleSystem effect;
    public bool DEBUG = false;

    private void Update()
    {
        if(DEBUG) {Blow(); DEBUG = !DEBUG; }
    }

    public void Blow()
    {
        effect.Stop();
        effect.Play();
        foreach (RaycastHit hit in Physics.SphereCastAll(origin.position, sphereRange, origin.forward, Mathf.Infinity,
                     hittableLayers))
        {
            if (hit.collider.gameObject.GetComponent<Blowable>() != null)
            {
                hit.collider.gameObject.GetComponent<Blowable>().Blow(origin.forward, strength);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(origin.transform.position,sphereRange);
    }
}
