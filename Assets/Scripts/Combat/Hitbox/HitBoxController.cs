using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    private List<HitBoxHandler> _hitBoxHandlers = new List<HitBoxHandler>();
    private float _damage = 10f;
    private float _knockbackForce = 10f;

    public float Damage => _damage;
    public float KnockbackForce => _knockbackForce;

    public void SetStats(float damage, float knockbackForce)
    {
        _damage = damage;
        _knockbackForce = knockbackForce;
    }

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            HitBoxHandler handler = child.AddComponent<HitBoxHandler>();
            handler.Init(this);
            _hitBoxHandlers.Add(handler);
        }
    }
}
