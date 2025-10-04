using Mirror.BouncyCastle.Math.Field;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttack : IDamageDealer
{
    [Header("Player Attack Settings")]
    private float _cooldown = 2f;

    private IState _playerState;

    public PlayerAttack(IState playerState)
    {
        _playerState = playerState;
    }

    public void Attack()
    {
        CharacterStateType state = _playerState.GetCurrentState();
        if (state != CharacterStateType.Attacking)
        {
            _playerState.ChangeState(CharacterStateType.Attacking);
        }
    }
}
