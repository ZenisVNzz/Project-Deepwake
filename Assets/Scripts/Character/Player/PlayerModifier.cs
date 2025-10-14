using UnityEngine;

public class PlayerModifier
{
    private bool _canMove = true;
    public bool CanMove => _canMove;

    private bool _canDash = true;
    public bool CanDash => _canDash;

    private bool _canAttack = true;
    public bool CanAttack => _canAttack;

    private ICharacterDirectionHandler _characterDirectionHandler;

    public PlayerModifier(ICharacterDirectionHandler characterDirectionHandler)
    {
        _characterDirectionHandler = characterDirectionHandler;
    }

    public void MoveModifier(bool canMove) { this._canMove = canMove; }
    public void DashModifier(bool canDash) { this._canDash = canDash; }
    public void AttackModifier(bool canAttack) { this._canAttack = canAttack; }
    public void DirectionModifier(bool enable, Direction dir)
    {
        if (enable)
        {
            _characterDirectionHandler.EnableForceDir(dir);
        }
        else
        {
            _characterDirectionHandler.DisableForceDir();
        }
    }
}
