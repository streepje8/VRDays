using System;
using System.Collections;
using System.Collections.Generic;
using SG;
using UnityEngine;

public class SpellManager : Singleton<SpellManager>
{
    public List<Spell> spells = new List<Spell>();
    public GuesturePose spellStartPose;
    public float spellAccuracy = 0.7f;
    public Transform handL;
    public Transform handR;
    public SG_TrackedHand TrackedHandL;
    public SG_TrackedHand TrackedHandR;
    public float DEBUG = 0f;
    public bool spellScan = false;
    public bool spellActive = false;
    public Spell activeSpell = null;
    public int spellStep = 1;
    private float spellTime = 0f;
    private float[] lastFlexionsL = new float[5];
    private float[] lastFlexionsR = new float[5];
    
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
            float scanValue = spellStartPose.Validate(handL.position, handR.position, lastFlexionsL, lastFlexionsR,
                spellAccuracy);
            if (scanValue > spellStartPose.validationValue)
            {
                spellScan = true;
                spellTime = 5f;
                Debug.Log("Starting Spell!");
            }
        }

        if (spellScan)
        {
            foreach (Spell s in spells)
            {
                float spellValue = s.pattern.poses[0].Validate(handL.position, handR.position, lastFlexionsL,
                    lastFlexionsR, spellAccuracy);
                if (spellValue > s.pattern.poses[0].validationValue)
                {
                    spellScan = false;
                    activeSpell = s;
                    spellStep = 1;
                    spellTime = 5f;
                    spellActive = true;
                    Debug.Log("Spell Detected!");
                }
                else
                {
                    DEBUG = spellValue;
                }
            }
            spellTime -= Time.deltaTime;
            if (spellTime <= 0)
            {
                spellScan = false;
                activeSpell = null;
                Debug.Log("Spell Cancelled");
            }
        }

        if (spellActive)
        {
            if (activeSpell.pattern.poses.Count > spellStep)
            {
                float validationValue = activeSpell.pattern.poses[spellStep].Validate(handL.position, handR.position,
                    lastFlexionsL, lastFlexionsR, spellAccuracy);
                if (validationValue >
                    activeSpell.pattern.poses[spellStep].validationValue)
                {
                    Debug.Log("Spell step " + spellStep + "/" + activeSpell.pattern.poses.Count + " with value: " + validationValue);
                    spellStep++;
                    spellTime = 5f;
                }
            }
            else
            {
                Debug.Log("Cast: " + activeSpell.name);
                activeSpell.onCast.Invoke();
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
            }
        }
    }
}
