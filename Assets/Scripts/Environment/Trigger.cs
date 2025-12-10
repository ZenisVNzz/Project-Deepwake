using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private Action action;

    public void RegisterAction(Action action)
    {
        this.action = action;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ship"))
        {
            action?.Invoke();
            Destroy(this.gameObject);
        }     
    }
}
