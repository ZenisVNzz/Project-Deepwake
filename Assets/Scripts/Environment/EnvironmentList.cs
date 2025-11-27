using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentList", menuName = "Data/EnvironmentList")]
public class EnvironmentList : ScriptableObject
{
    public List<GameObject> Prefabs = new List<GameObject>();
}
