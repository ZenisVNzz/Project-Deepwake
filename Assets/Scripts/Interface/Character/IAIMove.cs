using System;
using UnityEngine;

public interface IAIMove : IMovable
{
	public bool CanMove { get; set; }
    bool HaveReachedTarget();
}
