using UnityEngine;

public class EnemyAnimationHandler : IAnimationHandler
{
    private Animator animator;
    private IState enemyState;
    private ICharacterDirectionHandler directionHandler;

    private string currentAnimName;

    public EnemyAnimationHandler(Animator animator, IState enemyState, ICharacterDirectionHandler directionHander)
    {
        this.animator = animator;
        this.enemyState = enemyState;
        this.directionHandler = directionHander;
    }

    public void UpdateAnimation()
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
            Direction.UpLeft => "Enemy_UpLeft_Walk",
            Direction.UpRight => "Enemy_UpRight_Walk",
            Direction.DownLeft => "Enemy_DownLeft_Walk",
            _ => "Enemy_DownRight_Walk"
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

    private void PlayAnimation(string animName)
    {
        if (currentAnimName == animName) return;
        animator.Play(animName);
        currentAnimName = animName;
    }
}
