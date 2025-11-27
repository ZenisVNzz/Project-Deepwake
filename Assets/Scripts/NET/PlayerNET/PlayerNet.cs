using Mirror;
using TMPro;
using UnityEngine;

public class PlayerNet : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdatePlayerName))]
    public string playerName = "Zenis";
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI player2NameText;
    public TextMeshProUGUI player3NameText;

    private IState playerState;

    [SyncVar(hook = nameof(OnCharacterStateChanged))]
    public CharacterStateType characterStateType = CharacterStateType.Idle;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (playerNameText == null)
        {
            playerNameText = GetComponentInChildren<TextMeshProUGUI>(true);
        }

        if (playerNameText != null)
        {
            playerNameText.text = playerName;
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        playerState = GetComponent<PlayerController>().playerState;
    }

    private void UpdatePlayerName(string oldname, string newname)
    {
        if (playerNameText == null)
        {
            playerNameText = GetComponentInChildren<TextMeshProUGUI>(true);
        }

        if (playerNameText != null)
        {
            playerNameText.text = newname;
        }
    }

    public void SetActivePlayerNameUI(bool active)
    {
        if (playerNameText != null)
        {
            playerNameText.gameObject.SetActive(active);
        }
    }

    [Server]
    public void ChangeState(CharacterStateType newState)
    {
        characterStateType = newState;
    }

    private void OnCharacterStateChanged(CharacterStateType oldState, CharacterStateType newState)
    {
        if (playerState == null)
        {
            playerState = GetComponent<PlayerController>().playerState;
        }

        playerState.ChangeState(newState);
    }

    [Server]
    public void ChangeGameMapRequest(NodeTypes MapType)
    {
        Debug.Log($"[Server] Changing game map to {MapType}");

        ScriptableObject mapNodeData = ScriptableObject.CreateInstance(nameof(NodeType));
        if (mapNodeData is NodeType nodeType)
        {
            nodeType.NodeTypes = MapType;
        }

        MapNodeData mapData = new MapNodeData
        {
            nodeType = mapNodeData as NodeType
        };

        RpcChangeGameMap(mapData);
    }

    [ClientRpc]
    private void RpcChangeGameMap(MapNodeData mapData)
    {
        mapData.OnSelect();
    }
}
