using Mirror;
using UnityEngine;

public class CharacterInstaller : NetworkBehaviour
{
    public CharacterData _characterData;

    protected PlayerController _characterController;
    protected IPlayerRuntime _characterRuntime;

    protected CharacterData CharacterDataClone;

    public void SetData(CharacterData data)
    {
        _characterData = data;
    }

    protected void Awake()
    {
    }

    public virtual void GetComponent()
    {
        _characterRuntime = GetComponent<PlayerRuntime>();
        _characterController = GetComponent<PlayerController>();
        _characterRuntime.Init();
        _characterController.Init();
    }

    public virtual void InitComponent()
    {
        CharacterDataClone = Instantiate(_characterData);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log($"[CharacterInstaller] OnStartClient for {gameObject.name}");

        InitCharacter();
    }


    public virtual void InitCharacter()
    {
        GetComponent();
        InitComponent();
        
        if (isLocalPlayer)
        {
            var uiManager = FindAnyObjectByType<CharacterUIManager>();

            if (uiManager != null)
            {
                uiManager.Init(_characterRuntime);
            }

            CameraController.Instance.SetTarget(this.transform);
        }

        ShipController.Instance.SetChild(this.transform, false);
    }
}
