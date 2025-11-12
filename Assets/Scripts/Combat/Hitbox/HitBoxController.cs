using Mirror;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBoxController : NetworkBehaviour
{
    private List<HitBoxHandler> _hitBoxHandlers = new List<HitBoxHandler>();
    private CharacterRuntime _owner;

    private void Awake()
    {
        CharacterRuntime owner = GetComponentInParent<CharacterRuntime>();
        Init(owner);
    }

    public void Init(CharacterRuntime owner)
    {
        _owner = owner;

        foreach (Transform child in transform)
        {
            HitBoxHandler handler = child.GetComponent<HitBoxHandler>();
            handler.SetData(gameObject.tag, _owner);
            _hitBoxHandlers.Add(handler);
        }
    }

    public void Init(CharacterRuntime owner, float damage)
    {
        _owner = owner;

        foreach (Transform child in transform)
        {
            HitBoxHandler handler = child.GetComponent<HitBoxHandler>();
            handler.SetData(damage, gameObject.tag, _owner);
            _hitBoxHandlers.Add(handler);
        }
    }
}
