using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LootChance
{
    public string itemName;
    public float weight;
};

public class LootTable : MonoBehaviour
{
    [SerializeField] public LootChance[] table;
}