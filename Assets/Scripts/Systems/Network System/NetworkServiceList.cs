using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkServiceList", menuName = "NetworkSystem/NetworkServiceList")]
public class NetworkServiceList : ScriptableObject
{
    public List<NetworkService> NetworkServices = new();
}
