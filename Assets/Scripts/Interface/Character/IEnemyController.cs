using UnityEngine;

public interface IEnemyController
{
    public bool IsDead { get; }

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
