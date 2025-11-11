using Mirror;
using UnityEngine;

public class CharacterInstaller : NetworkBehaviour
{
    public CharacterData _characterData;
    public MultiplayerStatusUI multiplayerStatusUI;

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
            PlayerRuntime local = _characterRuntime as PlayerRuntime;

            var uiManager = GetComponent<CharacterUIManager>();

            if (uiManager != null)
            {
                uiManager.Init(_characterRuntime);
            }

            multiplayerStatusUI.BindLocalPlayer(local);

            PlayerNetManager playerNetManager = PlayerNetManager.Instance;
            playerNetManager.localCharacterRuntime = local;
            CmdRegisterCharacterRuntime();

            multiplayerStatusUI.AutoBindData();

            CameraController.Instance.SetTarget(this.transform);        
        }

        ShipController.Instance.SetChild(this.transform, false);
    }

    [Command]
    private void CmdRegisterCharacterRuntime()
    {
        PlayerNetManager.Instance.RegisterCharacterRuntime(_characterRuntime as PlayerRuntime);
    }
}
