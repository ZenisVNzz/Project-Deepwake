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
        if (isDead) return;
        isDead = true;

        RpcDeathEffect();
        StartCoroutine(ServerDeathProcess());
    }

    [ClientRpc]
    public override void RpcDeathEffect()
    {
        StartCoroutine(ClientDeathCoroutine());
    }

    public override IEnumerator ClientDeathCoroutine()
    {
        cd2D.enabled = false;
        hurtBox.enabled = false;
        yield return null;
    }

    public override IEnumerator ServerDeathProcess()
    {
        yield return new WaitForSeconds(8f);
        isDead = false;
    }
}
