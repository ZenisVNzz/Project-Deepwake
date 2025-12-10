using DG.Tweening;
using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D.IK;

public class GhostPirateController : EnemyController
{
    [Server]
    public override void OnDead()
    {
        RpcDeathEffect();
        StartCoroutine(ServerDeathProcess());
    }

    [Server]
    public override void RpcDeathEffect()
    {
        cd2D.enabled = false;
        hurtBox.enabled = false;
    }

    public override IEnumerator ClientDeathCoroutine()
    {    
        yield return null;
    }

    public override IEnumerator ServerDeathProcess()
    {
        yield return new WaitForSeconds(8f);
        isDead = false;
        enemyState.ChangeState(CharacterStateType.Awake);
        enemyRuntime.Revive();
        cd2D.enabled = true;
        hurtBox.enabled = true;
    }

    public void PlaySoulAnimation()
    {
        enemyState.ChangeState(CharacterStateType.Revive);
    }
}
