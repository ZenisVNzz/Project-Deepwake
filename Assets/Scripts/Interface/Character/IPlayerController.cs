using UnityEngine;

public interface IPlayerController
{
    public InputSystem_Actions InputHandler { get; }
    public PlayerModifier PlayerModifier { get; }
    public bool IsDead { get; }

    public void MoveOnSlope(bool moveOnSlope);

    void Initialize
    (
      IMovable movement,
      IDashable dash,
      IState state,
      ICharacterDirectionHandler characterDirectionHandler,
      IDamageDealer attack,
      IAnimationHandler animation,
      IStateHandler stateHandler,
      InputSystem_Actions input,
      IPlayerRuntime playerRuntime
    );
}
