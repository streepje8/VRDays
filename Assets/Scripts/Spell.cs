using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell", order = 1)]
public class Spell : ScriptableObject
{
#pragma warning disable CS0108, CS0114
    public string name = "New Spell";
#pragma warning restore CS0108, CS0114
    public GuesturePattern pattern;
    public string responseID;
}
