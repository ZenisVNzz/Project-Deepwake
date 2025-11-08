using DG.Tweening;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using Mirror;
using System.Collections;
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
    private float timer;
    private Image fill;
    private Image bg;

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
        cannonNavigation.UpdateNavigation(input.x);
    }

    [Server]
    private void UseCannon(NetworkConnectionToClient player)
    {
        if (currentPlayer != null) return;
        currentPlayer = player.identity;

        if (!isOwned)
            netIdentity.AssignClientAuthority(player);

        GivePlayerCannonAccess(player);
    }

    [TargetRpc]
    private void GivePlayerCannonAccess(NetworkConnection target)
    {
        GameObject playerObj = NetworkClient.localPlayer.gameObject;
        IPlayerController playerController = playerObj.GetComponent<IPlayerController>();
        IInteractionHandler interactionHandler = playerObj.GetComponentInChildren<IInteractionHandler>();
        PlayerModifier playerModifier = playerController.PlayerModifier;

        playerObj.transform.position = playerLockPos.position;
        playerModifier.MoveModifier(false);
        playerModifier.AttackModifier(false);
        playerModifier.DirectionModifier(true, playerLockDir);
        interactionHandler.SetInactive();
        NavigateGuideObj.SetActive(true);

        if (isFront)
            CameraOffset.Instance.Move(-3.5f);
        else
            CameraOffset.Instance.Move(3.5f);

        InputSystem_Actions playerInput = playerController.InputHandler;
        playerInput.Cannon.Enable();
        playerInput.Cannon.Navigate.performed += OnMove;
        playerInput.Cannon.Navigate.canceled += OnMove;
        playerInput.Cannon.Shoot.performed += ctx => OnShoot();
        playerInput.Cannon.Exit.performed += ctx => ExitCannon();
    }

    private void ExitCannon()
    {   
        CameraOffset.Instance.Move(0f);

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
        NotifyExitCannon(currentPlayer.connectionToClient);
        netIdentity.RemoveClientAuthority();
        currentPlayer = null;
    }

    [TargetRpc]
    private void NotifyExitCannon(NetworkConnection target)
    {
        GameObject playerObj = NetworkClient.localPlayer.gameObject;
        IPlayerController playerController = playerObj.GetComponent<IPlayerController>();
        IInteractionHandler interactionHandler = playerObj.GetComponentInChildren<IInteractionHandler>();
        PlayerModifier playerModifier = playerController.PlayerModifier;

        playerModifier.MoveModifier(true);
        playerModifier.AttackModifier(true);
        playerModifier.DirectionModifier(false, playerLockDir);
        NavigateGuideObj.SetActive(false);

        InputSystem_Actions playerInput = playerController.InputHandler;
        playerInput.Cannon.Navigate.performed -= OnMove;
        playerInput.Cannon.Navigate.canceled -= OnMove;
        playerInput.Cannon.Shoot.performed -= ctx => OnShoot();
        playerInput.Cannon.Exit.performed -= ctx => ExitCannon();
        playerInput.Cannon.Disable();
        StartCoroutine(UnlockInteract(interactionHandler));
    }

    private IEnumerator UnlockInteract(IInteractionHandler interactionHandler)
    {
        yield return new WaitForEndOfFrame();
        interactionHandler.SetActive();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void OnShoot()
    {
        if (timer >= cooldown)
        {         
            cannonNavigation.ApplyRecoil();
            if (isFront)
            {
                animator.Play("Cannon_Shoot");
            }
            else
            {
                animator.Play("CannonBack_Shoot");
            }
            
            cannonShoot.Shoot();           
            StartCoroutine(timerUpdate());
        }    
    }

    private IEnumerator timerUpdate()
    {
        bg.DOKill();
        fill.DOKill();
        fill.color = Color.white;
        bg.color = Color.gray;

        timer = 0;
        cooldownSlider.value = 0f;
        cooldownSliderUI.SetActive(true);      

        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            cooldownSlider.value = timer;
            yield return null;
        }    

        if (timer >= cooldown && cooldownSlider.value >= cooldown)
        {
            bg.DOFade(0f, 0.4f);
            fill.DOFade(0f, 0.5f).OnComplete(() =>
            {
                cooldownSliderUI.SetActive(false);              
            });
        }     
    }
}
