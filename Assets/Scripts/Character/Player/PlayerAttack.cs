using Mirror.BouncyCastle.Math.Field;
using System;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class PlayerAttack : IDamageDealer
{
    private IState _playerState;
    private HitBoxController _hitBoxController;

    public PlayerAttack(IState playerState, HitBoxController hitBoxController)
    {
        _playerState = playerState;
        _hitBoxController = hitBoxController;
    }

    public void Attack(float ATK)
    {
        _hitBoxController.SetStats(ATK, 10f);
        CharacterStateType state = _playerState.GetCurrentState();
        if (state != CharacterStateType.Attacking)
        {
            _playerState.ChangeState(CharacterStateType.Attacking);
        }
    }
}
