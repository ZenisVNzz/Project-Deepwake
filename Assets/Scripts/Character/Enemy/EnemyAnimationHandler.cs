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

    private readonly bool twoWayOnly; 

    public EnemyAnimationHandler(Animator animator, IState enemyState, ICharacterDirectionHandler directionHander, bool twoWayOnly = false)
    {
        this.animator = animator;
        this.enemyState = enemyState;
        this.directionHandler = directionHander;
        this.twoWayOnly = twoWayOnly;
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
        if (twoWayOnly)
        {
            Direction dir = directionHandler.GetDirection();
            bool faceRight = dir != Direction.Left;
            animator.transform.localScale = new Vector3(faceRight ?1f : -1f,1f,1f);
            PlayAnimation("Enemy_Idle");
            return;
        }

        Direction d = directionHandler.GetDirection();
        string anim = d switch
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
        if (twoWayOnly)
        {
            Direction dir = directionHandler.GetDirection();
            bool faceRight = dir != Direction.Left;
            animator.transform.localScale = new Vector3(faceRight ?1f : -1f,1f,1f);
            PlayAnimation("Enemy_Idle");
            return;
        }

        Direction d = directionHandler.GetDirection();
        string anim = d switch
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
        if (twoWayOnly)
        {
            Direction dir = directionHandler.GetDirection();
            bool faceRight = dir != Direction.Left;
            animator.transform.localScale = new Vector3(faceRight ?1f : -1f,1f,1f);
            PlayAnimation("Enemy_Attack1");
        }
        else
        {
            Direction d = directionHandler.GetDirection();
            string anim = d switch
            {
                Direction.UpLeft => "Enemy_UpLeft_Attack1",
                Direction.UpRight => "Enemy_UpRight_Attack1",
                Direction.DownLeft => "Enemy_DownLeft_Attack1",
                _ => "Enemy_DownRight_Attack1"
            };
            PlayAnimation(anim);
        }

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(currentAnimName) && stateInfo.normalizedTime >=1.0f)
        {
            enemyState.ChangeState(CharacterStateType.Idle);
        }
    }

    private void DeathProcess()
    {
        if (!isDeath)
        {
            if (twoWayOnly)
            {
                Direction dir = directionHandler.GetDirection();
                bool faceRight = dir != Direction.Left;
                animator.transform.localScale = new Vector3(faceRight ?1f : -1f,1f,1f);
                PlayAnimation("Enemy_Death");
                isDeath = true;
                return;
            }

            Direction dir2 = directionHandler.GetDirection();
            string anim = dir2 switch
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
