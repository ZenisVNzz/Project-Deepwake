using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    private GameObject currentPlayer;

    private IInteractable Interactable;
    [SerializeField] Transform playerLockPos;
    [SerializeField] Direction playerLockDir;
    [SerializeField] GameObject RotateObj;
    [SerializeField] GameObject NavigateGuideObj;
    [SerializeField] Transform shootPos;

    private Animator animator;

    private InputSystem_Actions inputActions;
    private Vector2 input;

    private CannonNavigation cannonNavigation;
    private CannonShoot cannonShoot;

    private float cooldown = 2f;
    private float timer;

    private void Awake()
    {
        Interactable = GetComponentInChildren<Interactable>();
        Interactable.Register(UseCannon);

        cannonNavigation = new CannonNavigation(RotateObj, NavigateGuideObj);
        cannonShoot = new CannonShoot(cannonNavigation, shootPos);
        animator = GetComponent<Animator>();

        inputActions = new InputSystem_Actions();
        inputActions.Cannon.Navigate.performed += OnMove;
        inputActions.Cannon.Navigate.canceled += OnMove;
        inputActions.Cannon.Shoot.performed += ctx => OnShoot();
        inputActions.Cannon.Exit.performed += ctx => ExitCannon();

        timer = cooldown;     
    }

    private void Update()
    {
        cannonNavigation.UpdateNavigation(input.x);
    }

    private void UseCannon(GameObject player)
    {
        currentPlayer = player;

        IPlayerController playerController = player.GetComponent<IPlayerController>();
        PlayerModifier playerModifier = playerController.PlayerModifier;

        player.transform.position = playerLockPos.transform.position;
        playerModifier.MoveModifier(false);
        playerModifier.AttackModifier(false);
        playerModifier.DirectionModifier(true, playerLockDir);
        NavigateGuideObj.SetActive(true);
        SetupCam(-3.5f);

        inputActions.Cannon.Enable();
    }

    private void ExitCannon()
    {
        IPlayerController playerController = currentPlayer.GetComponent<IPlayerController>();
        PlayerModifier playerModifier = playerController.PlayerModifier;

        playerModifier.MoveModifier(true);
        playerModifier.AttackModifier(true);
        playerModifier.DirectionModifier(false, playerLockDir);
        NavigateGuideObj.SetActive(false);
        SetupCam(0f);

        inputActions.Cannon.Disable();
    }

    private void SetupCam(float offset)
    {
        var cinemachine = Camera.main?.GetComponent<CinemachineBrain>();
        if (cinemachine != null)
        {
            var acticeCam = cinemachine.ActiveVirtualCamera as CinemachineCamera;
            if (acticeCam != null)
            {
                var component = acticeCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
                if (component is CinemachinePositionComposer composer)
                {
                    composer.TargetOffset = new Vector3(0f, offset, 0f);
                }
            }
        }
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
            animator.Play("Cannon_Shoot");
            cannonShoot.Shoot();           
            StartCoroutine(timerUpdate());
        }    
    }

    private IEnumerator timerUpdate()
    {
        timer = 0;

        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
