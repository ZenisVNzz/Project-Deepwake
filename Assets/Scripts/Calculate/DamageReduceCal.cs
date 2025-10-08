using UnityEngine;

public class DamageReduceCal
{
    public float Calculate(float rawDamage, float rawDefense)
    {
        float finalDamage = rawDamage * (100f / (100f + rawDefense));
        return finalDamage;
    }
}
