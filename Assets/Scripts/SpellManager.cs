using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using SG;
using UnityEngine;
using UnityEngine.Events;

public class SpellManager : Singleton<SpellManager>
{
    [Header("Main Settings")]
    public List<Spell> spells = new List<Spell>();
    public GuesturePose spellStartPose;
    public float spellAccuracy = 0.7f;

    [Header("Spell Responses")]
    public SerializableDictionaryBase<string, UnityEvent> responses =
        new SerializableDictionaryBase<string, UnityEvent>();

    [Header("Assign these pls")]
    public Transform ctrL;
    public Transform ctrR;
    public SG_TrackedHand TrackedHandL;
    public SG_TrackedHand TrackedHandR;
    
    [HideInInspector]public bool spellScan = false;
    [HideInInspector]public bool spellActive = false;
    [HideInInspector]public Spell activeSpell = null;
    [HideInInspector]public int spellStep = 1;
    private float spellTime = 0f;
    private float[] lastFlexionsL = new float[5];
    private float[] lastFlexionsR = new float[5];

    public int historyCount = 1;
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
    
    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(TrackedHandL.IsConnected()) TrackedHandL.GetNormalizedFlexion(out lastFlexionsL);
        if(TrackedHandR.IsConnected()) TrackedHandR.GetNormalizedFlexion(out lastFlexionsR);
        if (!spellActive && !spellScan)
        {
            float scanValue = spellStartPose.Validate(ctrL.position - oldest.handL,ctrR.position- oldest.handR, lastFlexionsL, lastFlexionsR,
                spellAccuracy,GuesturePose.HandToCheck.Right);
            if (scanValue > spellStartPose.validationValue)
            {
                spellScan = true;
                spellTime = 5f;
                GameController.Instance.handEffects.FlashSpellEffect();
                Debug.Log("Starting Spell!");
            }
        }

        if (spellScan)
        {
            foreach (Spell s in spells)
            {
                float spellValue = s.pattern.poses[0].Validate(ctrL.position - oldest.handL,ctrR.position- oldest.handR, lastFlexionsL,
                    lastFlexionsR, spellAccuracy,GuesturePose.HandToCheck.Right);
                if (spellValue > s.pattern.poses[0].validationValue)
                {
                    spellScan = false;
                    activeSpell = s;
                    spellStep = 1;
                    spellTime = 5f;
                    spellActive = true;
                    GameController.Instance.handEffects.FlashSpellEffect();
                    Debug.Log("Spell Detected!");
                }
            }
            spellTime -= Time.deltaTime;
            if (spellTime <= 0)
            {
                spellScan = false;
                activeSpell = null;
                Debug.Log("Spell Cancelled");
                GameController.Instance.handEffects.FlashSpellEffect(true);
            }
        }

        if (spellActive)
        {
            if (activeSpell.pattern.poses.Count > spellStep)
            {
                float validationValue = activeSpell.pattern.poses[spellStep].Validate(ctrL.position - oldest.handL,ctrR.position - oldest.handR,
                    lastFlexionsL, lastFlexionsR, spellAccuracy,GuesturePose.HandToCheck.Right);
                if (validationValue >
                    activeSpell.pattern.poses[spellStep].validationValue)
                {
                    Debug.Log("Spell step " + (spellStep + 1) + "/" + activeSpell.pattern.poses.Count + " with value: " + validationValue);
                    spellStep++;
                    spellTime = 5f;
                }
            }
            else
            {
                GameController.Instance.handEffects.FlashSpellEffect();
                Debug.Log("Cast: " + activeSpell.name);
                responses[activeSpell.responseID].Invoke();
                activeSpell = null;
                spellActive = false;
                //Spell finished!
            }
            spellTime -= Time.deltaTime;
            if (spellTime <= 0)
            {
                spellActive = false;
                activeSpell = null;
                Debug.Log("Spell Cancelled");
                GameController.Instance.handEffects.FlashSpellEffect(true);
            }
        }
        counter += Time.deltaTime;
        if (counter > pollingRate)
        {
            q.Enqueue(new PositionPair(ctrL.position,ctrR.position));
            if (q.Count > historyCount / pollingRate)
            {
                oldest = q.Dequeue();
            }
            counter = 0;
        }
    }
}
