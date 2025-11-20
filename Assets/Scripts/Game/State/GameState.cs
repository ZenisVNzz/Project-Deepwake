using System;
using UnityEngine;

[Serializable]
public class GameState
{
    protected GameStateMachine Machine;

    public virtual void Enter(GameStateMachine machine) => Machine = machine;
    public virtual void Update() { }
    public virtual void Exit() { }
}
