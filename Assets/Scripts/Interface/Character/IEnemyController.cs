using UnityEngine;

public interface IEnemyController
{
    void Initialize
    (
      IAIMove movement,
      IState state,
      IDamageDealer attack,
      IAnimationHandler animation,
      IStateHandler stateHandler,
      ICharacterRuntime enemyRuntime
    );
}
