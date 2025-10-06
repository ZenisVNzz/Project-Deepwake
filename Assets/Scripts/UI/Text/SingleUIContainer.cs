using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleUIContainer", menuName = "PrefabContainer/SingleUIContainer")]
public class SingleUIContainer : ScriptableObject
{
    [SerializeField] private List<PopupEntry> _ui;
    public List<PopupEntry> UI => _ui;
}

[Serializable]
public class SingleUIEntry
{
    public string ID;
    public GameObject Prefab;
}
