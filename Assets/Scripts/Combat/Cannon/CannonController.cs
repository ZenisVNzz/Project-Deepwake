using EasyTextEffects.Editor.MyBoxCopy.Extensions;
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

    private Animator animator;

    private InputSystem_Actions inputActions;
    private Vector2 input;

    private CannonNavigation cannonNavigation;

    private void Awake()
    {
        Interactable = GetComponentInChildren<Interactable>();
        Interactable.Register(UseCannon);

        cannonNavigation = new CannonNavigation(RotateObj, NavigateGuideObj);
        animator = GetComponent<Animator>();

        inputActions = new InputSystem_Actions();
        inputActions.Cannon.Navigate.performed += OnMove;
        inputActions.Cannon.Navigate.canceled += OnMove;
        inputActions.Cannon.Shoot.performed += ctx => OnShoot();
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

        inputActions.Cannon.Enable();
    }

    private void ExitCannon()
    {

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void OnShoot()
    {
        animator.Play("Cannon_Shoot");
        cannonNavigation.ApplyRecoil();
    }
}
