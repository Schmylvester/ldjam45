using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    [System.Serializable]
    public struct Spawn
    {
        public GameObject prefab;
        public float weight;
    }

    public Spawn[] spawnList;
}
