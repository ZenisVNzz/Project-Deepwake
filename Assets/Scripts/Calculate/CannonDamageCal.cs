using UnityEngine;

public class CannonDamageCal
{
	public float Calculate(ICharacterRuntime characterRuntime)
	{
		return (8f + (characterRuntime.TotalAttack * 1.4f));
	}
}
