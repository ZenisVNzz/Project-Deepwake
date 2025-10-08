using UnityEngine;

public class EnemyInstaller : CharacterInstaller
{
    private ICharacterRuntime _enemyRuntime;

    public override void InitComponent()
    {
        _enemyRuntime = gameObject.AddComponent<EnemyRuntime>();
    }

    public override void InitCharacter()
    {
        GetComponent();
        InitComponent();
        _enemyRuntime.Init(_playerData, _rigidbody2D);
    }
}
