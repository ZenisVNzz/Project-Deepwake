using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour, IAnimationHandler
{
    private Animator animator;
    private IState enemyState;
    private ICharacterDirectionHandler directionHandler;

    private string currentAnimName;
    private bool isDeath = false;

    [SerializeField]
    public bool twoWayOnly;

    public enum AnimDirectionMode { TwoWay, FourWayDiagonal, EightWay }

    [SerializeField]
    private AnimDirectionMode directionMode = AnimDirectionMode.FourWayDiagonal;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyState = GetComponent<EnemyController>().enemyState;
        directionHandler = GetComponent<ICharacterDirectionHandler>();

        if (twoWayOnly)
            directionMode = AnimDirectionMode.TwoWay;
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

        switch (directionMode)
        {
            case AnimDirectionMode.TwoWay:
                {
                    bool faceRight = dir != Direction.Left;
                    animator.transform.localScale = new Vector3(faceRight ? 1f : -1f, 1f, 1f);
                    PlayAnimation("Enemy_Idle");
                    break;
                }
            case AnimDirectionMode.EightWay:
                {
                    string anim = dir switch
                    {
                        Direction.Up => "Enemy_Up_Idle",
                        Direction.UpLeft => "Enemy_UpLeft_Idle",
                        Direction.UpRight => "Enemy_UpRight_Idle",
                        Direction.DownLeft => "Enemy_DownLeft_Idle",
                        Direction.DownRight => "Enemy_DownRight_Idle",
                        Direction.Down => "Enemy_Down_Idle",
                        Direction.Left => "Enemy_Left_Idle",
                        Direction.Right => "Enemy_Right_Idle",
                        _ => "Enemy_Down_Idle"
                    };
                    PlayAnimation(anim);
                    break;
                }
            default: 
                {
                    string anim = dir switch
                    {
                        Direction.UpLeft => "Enemy_UpLeft_Idle",
                        Direction.UpRight => "Enemy_UpRight_Idle",
                        Direction.DownLeft => "Enemy_DownLeft_Idle",
                        _ => "Enemy_DownRight_Idle"
                    };
                    PlayAnimation(anim);
                    break;
                }
        }
    }

    private void RunningProcess()
    {
        Direction dir = directionHandler.GetDirection();

        switch (directionMode)
        {
            case AnimDirectionMode.TwoWay:
                {
                    bool faceRight = dir != Direction.Left;
                    animator.transform.localScale = new Vector3(faceRight ? 1f : -1f, 1f, 1f);
                    PlayAnimation("Enemy_Run");
                    break;
                }
            case AnimDirectionMode.EightWay:
                {
                    string anim = dir switch
                    {
                        Direction.Up => "Enemy_Up_Run",
                        Direction.UpLeft => "Enemy_UpLeft_Run",
                        Direction.UpRight => "Enemy_UpRight_Run",
                        Direction.DownLeft => "Enemy_DownLeft_Run",
                        Direction.DownRight => "Enemy_DownRight_Run",
                        Direction.Down => "Enemy_Down_Run",
                        Direction.Left => "Enemy_Left_Run",
                        Direction.Right => "Enemy_Right_Run",
                        _ => "Enemy_Down_Run"
                    };
                    PlayAnimation(anim);
                    break;
                }
            default:
                {
                    string anim = dir switch
                    {
                        Direction.UpLeft => "Enemy_UpLeft_Run",
                        Direction.UpRight => "Enemy_UpRight_Run",
                        Direction.DownLeft => "Enemy_DownLeft_Run",
                        _ => "Enemy_DownRight_Run"
                    };
                    PlayAnimation(anim);
                    break;
                }
        }
    }

    private void AttackProcess()
    {
        Direction dir = directionHandler.GetDirection();

        switch (directionMode)
        {
            case AnimDirectionMode.TwoWay:
                {
                    bool faceRight = dir != Direction.Left;
                    animator.transform.localScale = new Vector3(faceRight ? 1f : -1f, 1f, 1f);
                    PlayAnimation("Enemy_Attack1");
                    break;
                }
            case AnimDirectionMode.EightWay:
                {
                    string anim = dir switch
                    {
                        Direction.Up => "Enemy_Up_Attack1",
                        Direction.UpLeft => "Enemy_UpLeft_Attack1",
                        Direction.UpRight => "Enemy_UpRight_Attack1",
                        Direction.DownLeft => "Enemy_DownLeft_Attack1",
                        Direction.DownRight => "Enemy_DownRight_Attack1",
                        Direction.Down => "Enemy_Down_Attack1",
                        Direction.Left => "Enemy_Left_Attack1",
                        Direction.Right => "Enemy_Right_Attack1",
                        _ => "Enemy_Down_Attack1"
                    };
                    PlayAnimation(anim);
                    break;
                }
            default:
                {
                    bool faceRight = false;
                    string anim = dir switch
                    {
                        Direction.UpLeft => "Enemy_UpLeft_Attack1",
                        Direction.UpRight => "Enemy_UpRight_Attack1",
                        Direction.DownLeft => "Enemy_DownLeft_Attack1",
                        _ => "Enemy_DownRight_Attack1"
                    };
                    PlayAnimation(anim);
                    break;
                }
        }

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

            switch (directionMode)
            {
                case AnimDirectionMode.TwoWay:
                    {
                        bool faceRight = dir != Direction.Left;
                        animator.transform.localScale = new Vector3(faceRight ? 1f : -1f, 1f, 1f);
                        PlayAnimation("Enemy_Death");
                        isDeath = true;
                        break;
                    }
                case AnimDirectionMode.EightWay:
                    {
                        string anim = dir switch
                        {
                            Direction.Up => "Enemy_Up_Death",
                            Direction.UpLeft => "Enemy_UpLeft_Death",
                            Direction.UpRight => "Enemy_UpRight_Death",
                            Direction.DownLeft => "Enemy_DownLeft_Death",
                            Direction.DownRight => "Enemy_DownRight_Death",
                            Direction.Down => "Enemy_Down_Death",
                            Direction.Left => "Enemy_Left_Death",
                            Direction.Right => "Enemy_Right_Death",
                            _ => "Enemy_Down_Death"
                        };
                        PlayAnimation(anim);
                        isDeath = true;
                        break;
                    }
                default:
                    {
                        string anim = dir switch
                        {
                            Direction.UpLeft => "Enemy_UpLeft_Death",
                            Direction.UpRight => "Enemy_UpRight_Death",
                            Direction.DownLeft => "Enemy_DownLeft_Death",
                            _ => "Enemy_DownRight_Death"
                        };
                        PlayAnimation(anim);
                        isDeath = true;
                        break;
                    }
            }
        }
    }

    private void PlayAnimation(string animName)
    {
        if (currentAnimName == animName) return;

        animator.Play(animName);
        currentAnimName = animName;
    }
}
