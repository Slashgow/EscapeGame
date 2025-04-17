using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneInputHandler : MonoSingleton<SceneInputHandler>
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [SerializeField] private string inventory = "ToggleInventory";
    [SerializeField] private string click = "Click";
    [SerializeField] private string scroll = "ScrollItem";

    private InputAction inventoryAction;
    private InputAction clickAction;
    private InputAction scrollAction;
    public bool InventoryTriggered => inventoryAction?.WasPressedThisFrame() ?? false;
    public bool ClickPressed => clickAction?.WasPressedThisFrame() ?? false;
    public Vector2 ScrollValue { get; private set; }

    protected override void Awake()
    {
        base.Awake();   
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        inventoryAction = mapReference.FindAction(inventory);
        clickAction = mapReference.FindAction(click);
        scrollAction = mapReference.FindAction(scroll);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        scrollAction.started += scrollPerformed;
        scrollAction.canceled += inputInfo => ScrollValue = Vector2.zero;
    }

    private void scrollPerformed(InputAction.CallbackContext context)
    {
        ScrollValue = context.ReadValue<Vector2>();
        Debug.Log(ScrollValue);
    }
}
