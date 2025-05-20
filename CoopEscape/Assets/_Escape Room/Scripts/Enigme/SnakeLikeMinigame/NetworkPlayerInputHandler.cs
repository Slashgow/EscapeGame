using System;
using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerInputHandler : NetworkBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "MovementMiniGame";

    private InputAction movementAction;
    public Vector2 MovementInput { get; private set; }

    public event Action OnMoveToRight = delegate { };
    public event Action OnMoveToLeft = delegate { };
    public event Action OnMoveToTop = delegate { };
    public event Action OnMoveToDown = delegate { };

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();

        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);
        movementAction = mapReference.FindAction(movement);

        movementAction.performed += MovementAction_Performed;
        movementAction.canceled += MovementAction_canceled;
    }

    private void MovementAction_Performed(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        MovementAction_PerformedObserver(MovementInput);
    }

    [ObserversRpc]
    private void MovementAction_PerformedObserver(Vector2 movementInput, RPCInfo rpcInfo = default)
    {
        if(rpcInfo.sender == networkManager.players[0])
        {
            if(movementInput.x >= 1)
            {
                OnMoveToRight?.Invoke();
            }
            else if(movementInput.y >= 1)
            {
                OnMoveToTop?.Invoke();
            }
        }
        else if(networkManager.playerCount > 1 && rpcInfo.sender == networkManager.players[1])
        {
            if (movementInput.x <= -1)
            {
                OnMoveToLeft?.Invoke();
            }
            else if (movementInput.y <= -1)
            {
                OnMoveToDown?.Invoke();
            }
        }
    }

    private void MovementAction_canceled(InputAction.CallbackContext context)
    {
        MovementInput = Vector2.zero;
    }

    private void OnDisable()
    {
        movementAction.performed -= MovementAction_Performed;
        movementAction.canceled -= MovementAction_canceled;

        playerControls.FindActionMap(actionMapName).Disable();
    }
}
