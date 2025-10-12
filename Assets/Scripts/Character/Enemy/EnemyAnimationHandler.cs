using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimationHandler : IAnimationHandler
{
    private Animator animator;
    private IState enemyState;
    private ICharacterDirectionHandler directionHandler;

    private string currentAnimName;
    private bool isDeath = false;

    public EnemyAnimationHandler(Animator animator, IState enemyState, ICharacterDirectionHandler directionHander)
    {
        this.animator = animator;
        this.enemyState = enemyState;
        this.directionHandler = directionHander;
    }

    public void UpdateAnimation()
    {
        if (!isDeath)
        {
            CharacterStateType currentState = enemyState.GetCurrentState();

            switch (currentState)
            {
                case CharacterStateType.Idle:
                    IdleProcess();
                    break;
                case CharacterStateType.Running:
                    RunningProcess();
                    break;
                case CharacterStateType.Attacking:
                    AttackProcess();
                    break;
                case CharacterStateType.Death:
                    DeathProcess();
                    break;
                default:
                    IdleProcess();
                    break;
            }
        }   
    }

    private void IdleProcess()
    {
        Direction dir = directionHandler.GetDirection();
        string anim = dir switch
        {
            Direction.UpLeft => "Enemy_UpLeft_Idle",
            Direction.UpRight => "Enemy_UpRight_Idle",
            Direction.DownLeft => "Enemy_DownLeft_Idle",
            _ => "Enemy_DownRight_Idle"
        };
        PlayAnimation(anim);
    }

    private void RunningProcess()
    {
        Direction dir = directionHandler.GetDirection();
        string anim = dir switch
        {
            Direction.UpLeft => "Enemy_UpLeft_Run",
            Direction.UpRight => "Enemy_UpRight_Run",
            Direction.DownLeft => "Enemy_DownLeft_Run",
            _ => "Enemy_DownRight_Run"
        };
        PlayAnimation(anim);
    }

    private void AttackProcess()
    {
        Direction dir = directionHandler.GetDirection();
        string anim = dir switch
        {
            Direction.UpLeft => "Enemy_UpLeft_Attack1",
            Direction.UpRight => "Enemy_UpRight_Attack1",
            Direction.DownLeft => "Enemy_DownLeft_Attack1",
            _ => "Enemy_DownRight_Attack1"
        };
        PlayAnimation(anim);

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(currentAnimName) && stateInfo.normalizedTime >= 1.0f)
        {
            enemyState.ChangeState(CharacterStateType.Idle);
        }
    }

    private void DeathProcess()
    {
        if (!isDeath)
        {
            Direction dir = directionHandler.GetDirection();
            string anim = dir switch
            {
                Direction.UpLeft => "Enemy_UpLeft_Death",
                Direction.UpRight => "Enemy_UpRight_Death",
                Direction.DownLeft => "Enemy_DownLeft_Death",
                _ => "Enemy_DownRight_Death"
            };
            PlayAnimation(anim);
            isDeath = true;
        }   
    }

    private void PlayAnimation(string animName)
    {
        if (currentAnimName == animName) return;
        animator.Play(animName);
        currentAnimName = animName;
    }
}
