using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Important")] public Transform handRPos;

    [Header("Objects")]
    public SerializableDictionaryBase<string, GameObject>
        objects = new SerializableDictionaryBase<string, GameObject>();

    public void SpawnObject(string ObjectID)
    {
        GameObject spawned = Instantiate(objects[ObjectID], handRPos.position + handRPos.forward * 0.4f + handRPos.up * 0.2f, Quaternion.identity);
        Debug.Log("Spawned: " + spawned.name);
    }
    
}
