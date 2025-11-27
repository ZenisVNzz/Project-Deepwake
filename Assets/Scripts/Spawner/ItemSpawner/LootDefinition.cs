using System;
using UnityEngine;

[Serializable]
public class LootDefinition
{
    public ItemData item;

    [Range(0f, 1f)]
    public float rate = 1f;

    public GameObject pickupPrefab;
    public int quantity = 1;
}