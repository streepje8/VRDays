using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New pattern", menuName = "Glove/GuesturePattern", order = 1)]
public class GuesturePattern : ScriptableObject
{
    public List<GuesturePose> poses = new List<GuesturePose>();
}
