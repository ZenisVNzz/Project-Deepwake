using DG.Tweening;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CannonController : NetworkBehaviour
{
    [SyncVar] private NetworkIdentity currentPlayer;
    public NetworkIdentity CurPlayer => currentPlayer;

    private IInteractable Interactable;
    [SerializeField] Transform playerLockPos;
    [SerializeField] Direction playerLockDir;
    [SerializeField] GameObject RotateObj;
    [SerializeField] GameObject NavigateGuideObj;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform recoilPivot;
    [SerializeField] GameObject cooldownSliderUI;
    [SerializeField] bool isFront;
    private Slider cooldownSlider;

    private Animator animator;
    private Vector2 input;

    private CannonNavigation cannonNavigation;
    private CannonShoot cannonShoot;

    private float cooldown = 1f;
    [SyncVar] private float timer;
    private Image fill;
    private Image bg;

    public List<ObjectFade> objectFade = new();

    private void Awake()
    {
        Interactable = GetComponentInChildren<Interactable>();
        Interactable.Register(UseCannon);

        cannonNavigation = new CannonNavigation(RotateObj, NavigateGuideObj, recoilPivot, isFront);
        cannonShoot = new CannonShoot(cannonNavigation, shootPos, this);
        animator = GetComponent<Animator>();

        if (cooldownSliderUI == null)
        {
            cooldownSliderUI = transform.Find("Canvas").transform.Find("CooldownUI").gameObject;
        }
        fill = cooldownSliderUI.transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();
        bg = cooldownSliderUI.transform.Find("Background").GetComponent<Image>();

        cooldownSlider = cooldownSliderUI.GetComponent<Slider>();
        cooldownSlider.maxValue = cooldown;

        timer = cooldown;     
    }

    private void Update()
    {
        if (!isServer) return;

        cannonNavigation.UpdateNavigation(input.x);
    }

    [Server]
    private void UseCannon(NetworkConnectionToClient player)
    {
        if (currentPlayer != null) return;
        currentPlayer = player.identity;

        netIdentity.AssignClientAuthority(player);

        GameObject playerObj = player.identity.gameObject;
        playerObj.transform.position = playerLockPos.position;

        GivePlayerCannonAccess(player);
    }

    [TargetRpc]
    private void GivePlayerCannonAccess(NetworkConnection target)
    {
        GameObject playerObj = NetworkClient.localPlayer.gameObject;
        PlayerController playerController = playerObj.GetComponent<PlayerController>();
        IInteractionHandler interactionHandler = playerObj.GetComponentInChildren<IInteractionHandler>();
        PlayerModifier playerModifier = playerController.playerModifier;
        
        playerModifier.MoveModifier(false);
        playerModifier.AttackModifier(false);
        playerModifier.DirectionModifier(true, playerLockDir);
        interactionHandler.SetInactive();
        NavigateGuideObj.SetActive(true);

        if (isFront)
        {
            CameraOffset.Instance.Move(-4f);
            CameraController.Instance.SetOrthographicSize(5.8f);
        }      
        else
        {
            CameraOffset.Instance.Move(4f);
            CameraController.Instance.SetOrthographicSize(5.8f);
        }

        PlayerRuntime playerRuntime = playerController.playerRuntime;
        playerRuntime.OnHit += ExitCannon;

        InputSystem_Actions playerInput = playerController.InputHandler;
        playerInput.Cannon.Enable();
        playerInput.Cannon.Navigate.performed += OnMove;
        playerInput.Cannon.Navigate.canceled += OnMove;
        playerInput.Cannon.Shoot.performed += OnShoot;
        playerInput.Cannon.Exit.performed += ctx => ExitCannon();

        if (objectFade.Count > 0)
        {
            foreach (var item in objectFade)
            {
                item.FadeObject();
            }
        }
    }

    private void ExitCannon()
    {   
        CameraOffset.Instance.Move(0f);
        CameraController.Instance.SetOrthographicSize(4.8f);

        RequestExitCannon();    
    }

    [Command]
    private void RequestExitCannon()
    {
        ProcessExitCannon();
    }

    [Server]
    private void ProcessExitCannon()
    {
        if (currentPlayer != null)
        {
            if (currentPlayer.connectionToClient != null)
            {
                NotifyExitCannon(currentPlayer.connectionToClient);
            }

            netIdentity.RemoveClientAuthority();
            currentPlayer = null;
        }
    }

    [TargetRpc]
    private void NotifyExitCannon(NetworkConnection target)
    {
        GameObject playerObj = NetworkClient.localPlayer.gameObject;
        PlayerController playerController = playerObj.GetComponent<PlayerController>();
        IInteractionHandler interactionHandler = playerObj.GetComponentInChildren<IInteractionHandler>();
        PlayerModifier playerModifier = playerController.playerModifier;

        playerModifier.MoveModifier(true);
        playerModifier.AttackModifier(true);
        playerModifier.DirectionModifier(false, playerLockDir);
        NavigateGuideObj.SetActive(false);

        PlayerRuntime playerRuntime = playerController.playerRuntime;
        playerRuntime.OnHit -= ExitCannon;

        InputSystem_Actions playerInput = playerController.InputHandler;
        playerInput.Cannon.Navigate.performed -= OnMove;
        playerInput.Cannon.Navigate.canceled -= OnMove;
        playerInput.Cannon.Shoot.performed -= OnShoot;
        playerInput.Cannon.Exit.performed += ctx => ExitCannon();
        playerInput.Cannon.Disable();
        StartCoroutine(UnlockInteract(interactionHandler));

        if (objectFade.Count > 0)
        {
            foreach (var item in objectFade)
            {
                item.UnfadeObject();
            }
        }
    }

    private IEnumerator UnlockInteract(IInteractionHandler interactionHandler)
    {
        yield return new WaitForEndOfFrame();
        interactionHandler.SetActive();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 clientInput = context.ReadValue<Vector2>();
        CmdMove(clientInput);
    }

    [Command]
    private void CmdMove(Vector2 input)
    {
        this.input = input;
    }

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (timer >= cooldown)
        {
            CameraShake.Instance.ShakeCamera();
        }
            
        CmdShoot();
    }

    [Command]
    private void CmdShoot()
    {
        ShootProcess();
    }

    [Server]
    private void ShootProcess()
    {
        if (timer >= cooldown)
        {
            RpcApplyRecoil();
            if (isFront)
            {
                animator.Play("Cannon_Shoot");
            }
            else
            {
                animator.Play("CannonBack_Shoot");
            }
            cannonShoot.Shoot(connectionToClient);
            StartCoroutine(timerUpdate());
        }   
    }

    [ClientRpc]
    private void RpcApplyRecoil()
    {
        cannonNavigation.ApplyRecoil();  
    }

    [Server]
    private IEnumerator timerUpdate()
    {
        timer = 0;
        RpcStartCooldownUI(cooldown); 

        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            RpcSetCooldownValue(timer);
            yield return null;
        }

        RpcEndCooldownUI();
    }

    [ClientRpc]
    private void RpcStartCooldownUI(float maxValue)
    {
        bg.DOKill();
        fill.DOKill();
        fill.color = Color.white;
        bg.color = Color.gray;

        cooldownSlider.value = 0f;
        cooldownSlider.maxValue = maxValue;
        cooldownSliderUI.SetActive(true);
    }

    [ClientRpc]
    private void RpcSetCooldownValue(float value)
    {
        cooldownSlider.value = value;
    }

    [ClientRpc]
    private void RpcEndCooldownUI()
    {
        bg.DOFade(0f, 0.4f);
        fill.DOFade(0f, 0.5f).OnComplete(() =>
        {
            cooldownSliderUI.SetActive(false);
        });
    }
}
