using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell", order = 1)]
public class Spell : ScriptableObject
{
    public string name = "New Spell";
    public GuesturePattern pattern;
    public UnityEvent onCast;
}
