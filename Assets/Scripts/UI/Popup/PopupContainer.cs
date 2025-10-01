using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupContainer", menuName = "PrefabContainer/PopupContainer")]
public class PopupContainer : ScriptableObject
{
    [SerializeField] private List<PopupEntry> _popups;
    public List<PopupEntry> Popups => _popups;
}

[Serializable]
public class PopupEntry
{
    public string ID;
    public GameObject Prefab;
}