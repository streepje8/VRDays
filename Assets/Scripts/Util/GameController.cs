using System;
using Openverse.Input;
using SG;
#if UNITY_EDITOR
using System.Runtime;
using UnityEditor;
#endif
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [Header("Actual Gamecontroller stuff")]
    public float deviceCheckTime = 0.5f;
    public float accuracy = 0.8f;
    public HandEffects handEffects;

    [Header("For recoding poses")]
    public Transform handL;
    public Transform handR;
    public SG_TrackedHand TrackedHandL;
    public SG_TrackedHand TrackedHandR;
    public Transform player;
    public GuesturePose pose = null;

    private float[] lastFlexionsL = new float[5];
    private float[] lastFlexionsR = new float[5];
    private void Awake()
    {
        Instance = this;
    }

    private float timer = 0f;

    private void Update()
    {
        if(TrackedHandL.IsConnected()) TrackedHandL.GetNormalizedFlexion(out lastFlexionsL);
        if(TrackedHandR.IsConnected()) TrackedHandR.GetNormalizedFlexion(out lastFlexionsR);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pose = ScriptableObject.CreateInstance<GuesturePose>();
            pose.Initialize(handL.position,handR.position,lastFlexionsL,lastFlexionsR);
            #if UNITY_EDITOR
            AssetDatabase.CreateAsset(pose, "Assets/NewPose.asset");
            AssetDatabase.Refresh();
            #endif
        }
        //Debug.Log(pose?.Validate(handL.position,handR.position,lastFlexionsL,lastFlexionsR,0.9f));
    }

    private void FixedUpdate()
    {
        if (timer <= 0f)
        {
            OpenverseInput.UpdateDevices();
            timer = deviceCheckTime;
        }
        timer -= Time.fixedDeltaTime;
    }
}
