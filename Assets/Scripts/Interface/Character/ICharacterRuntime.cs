using System;
using UnityEngine;

public interface ICharacterRuntime : IAttackable
{
    float HP { get; }
    float TotalHealth { get; }
    float TotalAttack { get; }
    float TotalSpeed { get; }

    event Action<float> OnHPChanged;

    float BonusMaxHealth { get; }
    float BonusAttackPower { get; }
    float BonusDefense { get; }
    float BonusSpeed { get; }

    void Init();
    void ApplyBonusStat(BonusStat bonusStat, float amount);
}
