using System.Collections.Generic;
using UnityEngine;

public class GhostPirateMovement : MonoBehaviour, IAIMove
{
    public List<CannonController> Cannon;

    private bool haveReachedTarget = false;
    public bool CanMove = true;

    public void Move(float moveSpeed)
    {
    }

    public void CmdMove(Vector2 input, float moveSpeed, bool isMoveOnSlope) => Move(moveSpeed);

    public Vector2 GetDir()
    {
        return Vector2.zero; //Test
    }

    public bool HaveReachedTarget() => haveReachedTarget;
}
