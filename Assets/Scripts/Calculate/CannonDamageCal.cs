using UnityEngine;

public class CannonDamageCal
{
	public float Calculate(ICharacterRuntime characterRuntime)
	{
		return (18f + (characterRuntime.TotalAttack * 0.3f));
	}
}
