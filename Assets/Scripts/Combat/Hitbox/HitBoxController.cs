using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    private List<HitBoxHandler> _hitBoxHandlers = new List<HitBoxHandler>();
    private ICharacterRuntime _owner;

    public void Init(ICharacterRuntime owner)
    {
        _owner = owner;

        foreach (Transform child in transform)
        {
            HitBoxHandler handler = child.AddComponent<HitBoxHandler>();
            handler.SetData(gameObject.tag, _owner);
            _hitBoxHandlers.Add(handler);
        }
    }

    public void Init(ICharacterRuntime owner, float damage)
    {
        _owner = owner;

        foreach (Transform child in transform)
        {
            HitBoxHandler handler = child.AddComponent<HitBoxHandler>();
            handler.SetData(damage, gameObject.tag, _owner);
            _hitBoxHandlers.Add(handler);
        }
    }
}
