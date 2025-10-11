using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHandler : IStateHandler
{
    private IState enemyState;
    private Rigidbody2D rb;

    private Dictionary<string, Action> eventListeners = new Dictionary<string, Action>();

    private bool IsWaitForKnockBack = false;

    public EnemyStateHandler(IState state, Rigidbody2D rigidbody2D)
    {
        enemyState = state;
        this.rb = rigidbody2D;
    }

    public void UpdateState()
    {
        if (enemyState.GetCurrentState() == CharacterStateType.Attacking)
        {
            return;
        }
        else if (enemyState.GetCurrentState() == CharacterStateType.Death)
        {
            Trigger("OnDeath");
            return;
        }
        else if (enemyState.GetCurrentState() == CharacterStateType.Knockback)
        {
            if (!IsWaitForKnockBack)
            {
                CoroutineRunner.Instance.RunCoroutine(WaitForKnockBack());
            }
            return;
        }
        else
        {
            if (CheckIfMoving())
                enemyState.ChangeState(CharacterStateType.Running);
            else
                enemyState.ChangeState(CharacterStateType.Idle);
        }
    }

    private bool CheckIfMoving()
    {
        return rb.linearVelocity.sqrMagnitude > 0.05f;
    }

    private IEnumerator WaitForKnockBack()
    {
        IsWaitForKnockBack = true;
        yield return new WaitForSeconds(0.7f);
        if (enemyState.GetCurrentState() == CharacterStateType.Knockback)
        {
            enemyState.ChangeState(CharacterStateType.Idle);
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
