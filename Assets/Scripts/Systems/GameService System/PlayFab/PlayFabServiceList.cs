using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayFabServiceList", menuName = "PlayFabService/PlayFabServiceList")]
public class PlayFabServiceList : ScriptableObject
{
    [SerializeField] private List<PlayFabService> _services = new List<PlayFabService>();
    public List<PlayFabService> Services => _services;
}
