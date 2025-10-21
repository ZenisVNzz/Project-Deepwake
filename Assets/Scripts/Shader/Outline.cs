using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private List<GameObject> outline;

    private void Awake()
    {
        foreach (var obj in outline)
        {
            obj.SetActive(false);
        }
    }

    public void ActiveOutline()
    {
        foreach (var obj in outline)
        {
            obj.SetActive(true);
        }
    }

    public void DeactiveOutline()
    {
        foreach (var obj in outline)
        {
            obj.SetActive(false);
        }   
    }
}
