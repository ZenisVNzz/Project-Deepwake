using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCleaner : MonoBehaviour
{
    public static ObjectCleaner Instance;
    public List<GameObject> ObjectToClean = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void Clean()
    {
        foreach (var item in ObjectToClean)
        {
            NetworkServer.Destroy(item);
        }
        ObjectToClean.Clear();
    }
}
