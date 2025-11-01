using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStateHandler : IStateHandler
{
    private IState playerState;
    private Rigidbody2D rb;
    private InputSystem_Actions inputHandler;

    private Dictionary<string, Action> eventListeners = new Dictionary<string, Action>();

    private bool IsWaitForKnockBack = false;

    public PlayerStateHandler(IState state, Rigidbody2D rigidbody2D, InputSystem_Actions inputHandler)
    {
        playerState = state;
        rb = rigidbody2D;
        this.inputHandler = inputHandler;
    }

    public void UpdateState()
    {
        if (playerState.GetCurrentState() == CharacterStateType.Attacking || playerState.GetCurrentState() == CharacterStateType.Death)
        {
            inputHandler.Player.Move.Disable();
            return;
        }
        else if (playerState.GetCurrentState() == CharacterStateType.Knockback)
        {
            if (!IsWaitForKnockBack)
            {
                CoroutineRunner.Instance.RunCoroutine(WaitForKnockBack());
            }
            return;
        }
        else
        {
            inputHandler.Player.Move.Enable();
            if (CheckIfMoving())
                playerState.ChangeState(CharacterStateType.Running);
            else
                playerState.ChangeState(CharacterStateType.Idle);
        }   
    }

    private bool CheckIfMoving()
    {
        return rb.linearVelocity.sqrMagnitude > 0.05f;
    }

    private IEnumerator WaitForKnockBack()
    {
        IsWaitForKnockBack = true;
        inputHandler.Player.Disable();
        yield return new WaitForSeconds(0.3f);
        inputHandler.Player.Enable();
        yield return new WaitForSeconds(0.4f);
        if (playerState.GetCurrentState() == CharacterStateType.Knockback)
        {
            playerState.ChangeState(CharacterStateType.Idle);
            IsWaitForKnockBack = false;
        }
    }

    public void Register(string eventName, Action listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = null;
        }
        eventListeners[eventName] += listener;
    }

    private void Trigger(string eventName)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName]?.Invoke();
        }
    }
}
