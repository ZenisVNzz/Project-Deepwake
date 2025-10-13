using UnityEngine;

public class PlayerModifier
{
    private bool canMove = true;
    public bool CanMove => canMove;

    private bool canDash = true;
    public bool CanDash => canDash;

    public void MoveModifier(bool canMove) { this.canMove = canMove; }
    public void DashModifier(bool canDash) { this.canDash = canDash; }
}
