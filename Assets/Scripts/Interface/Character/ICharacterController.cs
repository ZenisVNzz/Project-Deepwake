using UnityEngine;

public interface ICharacterController
{
    void Initialize
    (
      IMovable movement,
      IState state,
      IDamageDealer attack,
      IAnimationHandler animation,
      IStateHandler stateHandler
    );
}
