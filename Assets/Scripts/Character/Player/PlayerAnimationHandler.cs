using System;
using UnityEngine;

public class PlayerAnimationHandler : IAnimationHandler
{
    private readonly Animator animator;
    private readonly IState playerState;
    private readonly ICharacterDirectionHandler directionHandler;

    private string currentAnimName;
    private bool isDeath = false;

    public PlayerAnimationHandler(Animator animator, IState playerState, ICharacterDirectionHandler directionHandler)
    {
        this.animator = animator;
        this.playerState = playerState;
        this.directionHandler = directionHandler;
    }

    public void UpdateAnimation()
    {
        CharacterStateType currentState = playerState.GetCurrentState();

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
            default:
                IdleProcess();
                break;
        }
    }

    private void IdleProcess()
    {
        Direction dir = directionHandler.GetDirection();
        string anim = dir switch
        {
            Direction.Left => "Player_Left_Idle",
            Direction.Right => "Player_Right_Idle",
            Direction.UpLeft => "Player_UpLeft_Idle",
            Direction.UpRight => "Player_UpRight_Idle",
            Direction.DownLeft => "Player_DownLeft_Idle",
            Direction.DownRight => "Player_DownRight_Idle",
            Direction.Up => "Player_Up_Idle",
            Direction.Down => "Player_Down_Idle",
            _ => "Player_Down_Idle"
        };
        PlayAnimation(anim);
    }

    private void RunningProcess()
    {
        Direction dir = directionHandler.GetDirection();
        string anim = dir switch
        {
            Direction.Left => "Player_Left_Walk",
            Direction.Right => "Player_Right_Walk",
            Direction.UpLeft => "Player_UpLeft_Walk",
            Direction.UpRight => "Player_UpRight_Walk",
            Direction.DownLeft => "Player_DownLeft_Walk",
            Direction.DownRight => "Player_DownRight_Walk",
            Direction.Up => "Player_Up_Walk",
            Direction.Down => "Player_Down_Walk",
            _ => "Player_Down_Walk"
        };
        PlayAnimation(anim);
    }

    private void AttackProcess()
    {
        Direction dir = directionHandler.GetDirection();
        string anim = dir switch
        {
            Direction.Left => "Player_Left_Attack1",
            Direction.Right => "Player_Right_Attack1",
            Direction.UpLeft => "Player_UpLeft_Attack1",
            Direction.UpRight => "Player_UpRight_Attack1",
            Direction.DownLeft => "Player_DownLeft_Attack1",
            Direction.DownRight => "Player_DownRight_Attack1",
            Direction.Up => "Player_Up_Attack1",
            Direction.Down => "Player_Down_Attack1",
            _ => "Player_Down_Attack1"
        };
        PlayAnimation(anim);

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(currentAnimName) && stateInfo.normalizedTime >= 1.0f)
        {
            playerState.ChangeState(CharacterStateType.Idle);
        }
    }

    private void PlayAnimation(string animName)
    {
        if (currentAnimName == animName) return;
        animator.Play(animName);
        currentAnimName = animName;
    }
}
