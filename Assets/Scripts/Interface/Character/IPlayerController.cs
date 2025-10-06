using UnityEngine;

public interface IPlayerController
{
    void Initialize
    (
      IMovable movement,
      IDashable dash,
      IState state,
      IDamageDealer attack,
      IAnimationHandler animation,
      IStateHandler stateHandler,
      InputSystem_Actions input
    );
}
