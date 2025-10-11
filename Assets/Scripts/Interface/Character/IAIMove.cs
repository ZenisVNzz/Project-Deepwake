using System;
using UnityEngine;

public interface IAIMove : IMovable
{
	bool HaveReachedTarget();
}
