using UnityEngine;

public interface IPlayerController
{
    public PlayerModifier PlayerModifier { get; }

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
      CharacterData characterData,
      GameObject charMenuUI,
      GameObject gameMenuUI
    );
}
