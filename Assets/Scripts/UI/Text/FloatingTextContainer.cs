using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatingTextContainer", menuName = "PrefabContainer/FloatingTextContainer")]
public class FloatingTextContainer : ScriptableObject
{
    [SerializeField] private List<PopupEntry> _text;
    public List<PopupEntry> Text => _text;
}

[Serializable]
public class SingleUIEntry
{
    public string ID;
    public GameObject Prefab;
}
