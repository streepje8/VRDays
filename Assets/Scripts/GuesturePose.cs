using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class GuesturePose : ScriptableObject
{
    [Header("Settings")]
    public float validationValue = 0f;

    [Header("Thumb")]
    [Range(0, 1)] public float thumbFlexGoalL;
    [Range(0, 1)] public float thumbFlexGoalR;

    [Header("Index Finger")]
    [Range(0, 1)] public float indexFlexGoalL;
    [Range(0, 1)] public float indexFlexGoalR;

    [Header("Middle Finger")]
    [Range(0, 1)] public float middleFlexGoalL;
    [Range(0, 1)] public float middleFlexGoalR;
    
    [Header("Ring Finger")]
    [Range(0, 1)] public float ringFlexGoalL;
    [Range(0, 1)] public float ringFlexGoalR;

    [Header("Pinky")]
    [Range(0, 1)] public float pinkyFlexGoalL;
    [Range(0, 1)] public float pinkyFlexGoalR;

    [Header("Vectors")] 
    public Vector3 handLVector;
    public Vector3 handRVector;

    public enum HandToCheck
    {
        Left,
        Right,
        Both
    }

    public float Validate(Vector3 handLVector, Vector3 handRVector,float[] flexionsL, float[] flexionsR, float accuracy, HandToCheck htc)
    {
        float result = 0f;
        if (htc == HandToCheck.Left || htc == HandToCheck.Both)
        {
            result += Math.Abs(thumbFlexGoalL - flexionsL[0]);
            result += Math.Abs(indexFlexGoalL - flexionsL[1]);
            result += Math.Abs(middleFlexGoalL - flexionsL[2]);
            result += Math.Abs(ringFlexGoalL - flexionsL[3]);
            result += Math.Abs(pinkyFlexGoalL - flexionsL[4]);
            Vector3 diff = this.handLVector - handLVector;
            result += (diff.x + diff.y + diff.x) / 3f;
        }

        if (htc == HandToCheck.Right || htc == HandToCheck.Both)
        {
            result += Math.Abs(thumbFlexGoalR - flexionsR[0]);
            result += Math.Abs(indexFlexGoalR - flexionsR[1]);
            result += Math.Abs(middleFlexGoalR - flexionsR[2]);
            result += Math.Abs(ringFlexGoalR - flexionsR[3]);
            result += Math.Abs(pinkyFlexGoalR - flexionsR[4]);
            Vector3 diff = this.handRVector - handRVector;
            result += (diff.x + diff.y + diff.x) / 3f;
        }
        result *= accuracy;
        result = Mathf.Clamp01(1 - result);
        return result;
    }
    
    public void Initialize(Vector3 handLVector, Vector3 handRVector,float[] flexionsL, float[] flexionsR)
    {
        thumbFlexGoalL = flexionsL[0];
        indexFlexGoalL = flexionsL[1];
        middleFlexGoalL = flexionsL[2];
        ringFlexGoalL = flexionsL[3];
        pinkyFlexGoalL = flexionsL[4];
        thumbFlexGoalR = flexionsR[0];
        indexFlexGoalR = flexionsR[1];
        middleFlexGoalR = flexionsR[2];
        ringFlexGoalR = flexionsR[3];
        pinkyFlexGoalR = flexionsR[4];
        this.handLVector = handLVector;
        this.handRVector = handRVector;
    }
}
