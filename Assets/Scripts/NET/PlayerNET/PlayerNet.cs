using Mirror;
using UnityEngine;

public class PlayerNet : NetworkBehaviour
{
    private IState playerState;

    [SyncVar]
    private string playerName = "Zenis";

    [SyncVar(hook = nameof(OnCharacterStateChanged))]
    public CharacterStateType characterStateType = CharacterStateType.Idle;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        playerState = GetComponent<PlayerController>().playerState;
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
