using Mirror;
using Mirror.BouncyCastle.Math.Field;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour, IDamageDealer
{
    private IState _playerState;
    private HitBoxController _hitBoxController;

    private PlayerNet PlayerNet;

    public void Awake()
    {
        _playerState = GetComponent<PlayerController>().playerState;
        _hitBoxController = GetComponent<HitBoxController>();
        PlayerNet = GetComponent<PlayerNet>();
    }

    [Command]
    public void CmdAttack(float ATK)
    {
        if (!GetComponent<PlayerController>().playerModifier.CanAttack) return;

        CharacterStateType state = _playerState.GetCurrentState();
        if (state != CharacterStateType.Attacking)
        {
            PlayerNet.ChangeState(CharacterStateType.Attacking);
        }
    }
}
