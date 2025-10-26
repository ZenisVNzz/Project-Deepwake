using UnityEngine;

public class PlayerAnimationHandler
{
    private Animator animator;
    private PlayerState playerState;
    private PlayerDirectionHander directionHander;

    public PlayerAnimationHandler(Animator animator, PlayerState playerState, PlayerDirectionHander directionHander)
    {
        this.animator = animator;
        this.playerState = playerState;
        this.directionHander = directionHander;
    }

    public void UpdateAnimation()
    {
        CharacterStateType currentState = playerState.CurrentState;
        Direction currentDirection = directionHander.GetDirection();
        switch (currentState)
        {
            case CharacterStateType.Idle:
                IdleProcess();
                break;
            case CharacterStateType.Running:
                RunningProcess();
                break;
            default:
                IdleProcess();
                break;
        }
    }

    private void IdleProcess()
    {
        if (Direction.Left == directionHander.GetDirection())
        {
            animator.Play("Player_Left_Idle");
        }
        else if (Direction.Right == directionHander.GetDirection())
        {
            animator.Play("Player_Right_Idle");
        }
        else if (Direction.UpLeft == directionHander.GetDirection())
        {
            animator.Play("Player_UpLeft_Idle");
        }
        else if (Direction.UpRight == directionHander.GetDirection())
        {
            animator.Play("Player_UpRight_Idle");
        }
        else if (Direction.DownLeft == directionHander.GetDirection())
        {
            animator.Play("Player_DownLeft_Idle");
        }
        else if (Direction.DownRight == directionHander.GetDirection())
        {
            animator.Play("Player_DownRight_Idle");
        }
        else if (Direction.Up == directionHander.GetDirection())
        {
            animator.Play("Player_Up_Idle");
        }
        else if (Direction.Down == directionHander.GetDirection())
        {
            animator.Play("Player_Down_Idle");
        }
    }

    private void RunningProcess()
    {
        if (Direction.Left == directionHander.GetDirection())
        {
            animator.Play("Player_Left_Walk");
        }
        else if (Direction.Right == directionHander.GetDirection())
        {
            animator.Play("Player_Right_Walk");
        }
        else if (Direction.UpLeft == directionHander.GetDirection())
        {
            animator.Play("Player_UpLeft_Walk");
        }
        else if (Direction.UpRight == directionHander.GetDirection())
        {
            animator.Play("Player_UpRight_Walk");
        }
        else if (Direction.DownLeft == directionHander.GetDirection())
        {
            animator.Play("Player_DownLeft_Walk");
        }
        else if (Direction.DownRight == directionHander.GetDirection())
        {
            animator.Play("Player_DownRight_Walk");
        }
        else if (Direction.Up == directionHander.GetDirection())
        {
            animator.Play("Player_Up_Walk");
        }
        else if (Direction.Down == directionHander.GetDirection())
        {
            animator.Play("Player_Down_Walk");
        }
    }
}
