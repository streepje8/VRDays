using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SG;
using UnityEngine;

public class GuestureGrader : MonoBehaviour
{
    public GuesturePose guesture;
    public Transform handL;
    public Transform handR;
    public SG_TrackedHand TrackedHandL;
    public SG_TrackedHand TrackedHandR;

    [Range(0, 1000)] public float acc = 0f;
    
    private float[] lastFlexionsL = new float[5];
    private float[] lastFlexionsR = new float[5];
    void Update()
    {
        if(TrackedHandL.IsConnected()) TrackedHandL.GetNormalizedFlexion(out lastFlexionsL);
        if(TrackedHandR.IsConnected()) TrackedHandR.GetNormalizedFlexion(out lastFlexionsR);
        Debug.Log(guesture.Validate(handL.position,handR.position,lastFlexionsL,lastFlexionsR,acc));
    }
}
