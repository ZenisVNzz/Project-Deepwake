using System;
using UnityEngine;

public interface IAttackable
{
    void TakeDamage(float damage, Vector3 knockback, ICharacterRuntime characterRuntime);
}
