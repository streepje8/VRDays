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

    public int historyCount = 1;

    [Range(0, 10)] public float acc = 0f;
    
    private float[] lastFlexionsL = new float[5];
    private float[] lastFlexionsR = new float[5];

    public GuesturePose.HandToCheck htc;
    
    struct PositionPair
    {
        public Vector3 handL;
        public Vector3 handR;
        public PositionPair(Vector3 handL, Vector3 handR)
        {
            this.handL = handL;
            this.handR = handR;
        }
    }
    private Queue<PositionPair> q = new Queue<PositionPair>();
    private float counter = 0f;
    private float pollingRate = 0.1f;
    private PositionPair oldest;
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > pollingRate)
        {
            q.Enqueue(new PositionPair(handL.position,handR.position));
            if (q.Count > historyCount / pollingRate)
            {
                oldest = q.Dequeue();
            }
        }
        if(TrackedHandL.IsConnected()) TrackedHandL.GetNormalizedFlexion(out lastFlexionsL);
        if(TrackedHandR.IsConnected()) TrackedHandR.GetNormalizedFlexion(out lastFlexionsR);
        Debug.Log(guesture.Validate(handL.position - oldest.handL,handR.position- oldest.handR,lastFlexionsL,lastFlexionsR,acc,htc));
    }
}
