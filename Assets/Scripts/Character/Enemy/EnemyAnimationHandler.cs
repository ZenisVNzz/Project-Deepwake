using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour, IAnimationHandler
{
    private Animator animator;
    private IState enemyState;
    private ICharacterDirectionHandler directionHandler;

    private string currentAnimName;
    EnemyController enemyController;
    private bool isDeath => GetComponent<EnemyController>().IsDead;

    [SerializeField]
    public bool twoWayOnly;

    public enum AnimDirectionMode { TwoWay, FourWayDiagonal, EightWay }

    [SerializeField]
    private AnimDirectionMode directionMode = AnimDirectionMode.FourWayDiagonal;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        enemyState = enemyController.enemyState;
        directionHandler = GetComponent<ICharacterDirectionHandler>();

        if (twoWayOnly)
            directionMode = AnimDirectionMode.TwoWay;
    }

    public void UpdateAnimation()
    {
        CharacterStateType currentState = enemyState.GetCurrentState();

        switch (currentState)
        {
            case CharacterStateType.Awake:
                AwakeProcess();
                break;
            case CharacterStateType.Idle:
                IdleProcess();
                break;
            case CharacterStateType.Running:
                RunningProcess();
                break;
            case CharacterStateType.Attacking:
                AttackProcess();
                break;
            case CharacterStateType.Revive:
                ReviveProcess();
                break;
            case CharacterStateType.Death:
                DeathProcess();
                break;
            default:
                IdleProcess();
                break;
        }
    }

    private void AwakeProcess()
    {
        Direction dir = directionHandler.GetDirection();
        switch (directionMode)
        {
            case AnimDirectionMode.TwoWay:
                {
                    bool faceRight = dir != Direction.Left;
                    animator.transform.localScale = new Vector3(faceRight ? 1f : -1f, 1f, 1f);
                    PlayAnimation("Enemy_Awake");
                    Debug.Log("Playing Enemy_Awake animation");
                    break;
                }
            case AnimDirectionMode.EightWay:
                {
                    string anim = dir switch
                    {
                        Direction.Up => "Enemy_Up_Awake",
                        Direction.UpLeft => "Enemy_UpLeft_Awake",
                        Direction.UpRight => "Enemy_UpRight_Awake",
                        Direction.DownLeft => "Enemy_DownLeft_Awake",
                        Direction.DownRight => "Enemy_DownRight_Awake",
                        Direction.Down => "Enemy_Down_Awake",
                        Direction.Left => "Enemy_Left_Awake",
                        Direction.Right => "Enemy_Right_Awake",
                        _ => "Enemy_Down_Awake"
                    };
                    PlayAnimation(anim);
                    break;
                }
            default:
                {
                    string anim = dir switch
                    {
                        Direction.UpLeft => "Enemy_UpLeft_Awake",
                        Direction.UpRight => "Enemy_UpRight_Awake",
                        Direction.DownLeft => "Enemy_DownLeft_Awake",
                        _ => "Enemy_DownRight_Awake"
                    };
                    PlayAnimation(anim);
                    break;
                }
        }

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(currentAnimName) && stateInfo.normalizedTime >= 1.0f)
        {
            Invoke(nameof(ChangeToIdleState), 0.3f);
        }
    }

    private void ChangeToIdleState()
    {
        enemyState.ChangeState(CharacterStateType.Idle);
        IAIMove aIMove = GetComponent<IAIMove>();
        aIMove.CanMove = true;
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
                        enemyController.SetStatus(true);
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
                        enemyController.SetStatus(true);
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
                        enemyController.SetStatus(true);
                        break;
                    }
            }
        }
    }

    private void ReviveProcess()
    {
        Debug.Log("ReviveProcess called");

        Direction dir = directionHandler.GetDirection();
        switch (directionMode)
        {
            case AnimDirectionMode.TwoWay:
                {
                    bool faceRight = dir != Direction.Left;
                    animator.transform.localScale = new Vector3(faceRight ? 1f : -1f, 1f, 1f);
                    PlayAnimation("Enemy_Revive");
                    break;
                }
            case AnimDirectionMode.EightWay:
                {
                    string anim = dir switch
                    {
                        Direction.Up => "Enemy_Up_Revive",
                        Direction.UpLeft => "Enemy_UpLeft_Revive",
                        Direction.UpRight => "Enemy_UpRight_Revive",
                        Direction.DownLeft => "Enemy_DownLeft_Revive",
                        Direction.DownRight => "Enemy_DownRight_Revive",
                        Direction.Down => "Enemy_Down_Revive",
                        Direction.Left => "Enemy_Left_Revive",
                        Direction.Right => "Enemy_Right_Revive",
                        _ => "Enemy_Down_Revive"
                    };
                    PlayAnimation(anim);
                    break;
                }
            default:
                {
                    string anim = dir switch
                    {
                        Direction.UpLeft => "Enemy_UpLeft_Revive",
                        Direction.UpRight => "Enemy_UpRight_Revive",
                        Direction.DownLeft => "Enemy_DownLeft_Revive",
                        _ => "Enemy_DownRight_Revive"
                    };
                    PlayAnimation(anim);
                    break;
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
