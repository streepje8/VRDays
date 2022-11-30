using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEffects : MonoBehaviour
{
    public Renderer handL;
    public Renderer handR;
    [ColorUsage(false, true)] public Color defaultEmission;
    [ColorUsage(false, true)] public Color pointMovementEmission;
    [ColorUsage(false, true)] public Color spellCompleteEmission;
    [ColorUsage(false, true)] public Color spellFailEmission;
    public float changeSpeed = 10f;
    public bool isWalking = false;
    private Material handLMat;
    private Material handRMat;
    
    private static int EmissionColor;
    private void Start()
    {
        EmissionColor = Shader.PropertyToID("_EmissionColor");
        handLMat = new Material(handL.material);
        handRMat = new Material(handR.material);
        handL.sharedMaterial = handLMat;
        handR.sharedMaterial = handRMat;
        handL.material.EnableKeyword("_EMISSION");
        handR.material.EnableKeyword("_EMISSION");
    }

    public void FlashSpellEffect(bool isFail = false)
    {
        handR.material.SetColor(EmissionColor,isFail ? spellFailEmission : spellCompleteEmission);
    }

    private void Update()
    {
        handR.material.SetColor(EmissionColor,Color.Lerp(handR.material.GetColor(EmissionColor),defaultEmission,changeSpeed * Time.deltaTime));
        handL.material.SetColor(EmissionColor,Color.Lerp(handL.material.GetColor(EmissionColor),isWalking ? pointMovementEmission : defaultEmission,changeSpeed * Time.deltaTime));
    }
}
