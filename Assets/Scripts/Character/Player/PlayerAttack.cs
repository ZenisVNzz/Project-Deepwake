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

    private void Awake()
    {
        _playerState = GetComponent<PlayerController>().playerState;
        _hitBoxController = GetComponent<HitBoxController>();
        PlayerNet = GetComponent<PlayerNet>();
    }

    public void Attack(float ATK)
    {
        CharacterStateType state = _playerState.GetCurrentState();
        if (state != CharacterStateType.Attacking)
        {
            PlayerNet.CmdChangeState(CharacterStateType.Attacking);
        }
    }
}
