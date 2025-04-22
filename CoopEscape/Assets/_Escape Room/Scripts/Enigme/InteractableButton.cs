using PurrNet;
using UnityEngine;
using UnityEngine.Events;

public enum ButtonInteraction
{
    Click,
    Hold
}

public class InteractableButton : AInteractable
{
    [SerializeField] private ButtonInteraction buttonInteraction;

    public UnityEvent OnHoldButton;
    public UnityEvent OnStopHoldButton;
    public UnityEvent OnClick;

    private bool isPushed;

    [ObserversRpc]
    public override void Interact()
    {
        if(buttonInteraction == ButtonInteraction.Click && !isPushed)
        {
            //isPushed = true;
            OnClick?.Invoke();
        }
    }

    public override void OnStopHover()
    {
        base.OnStopHover();

        if (buttonInteraction != ButtonInteraction.Hold)
            return;

        if (!isPushed )
            return;
        
        isPushed = false;
        StopHolding();
    }

    private void Update()
    {
        if(buttonInteraction != ButtonInteraction.Hold)
            return;

        if(IsHovering && isPushed && !SceneInputHandler.Instance.IsClickCurrentlyPressed)
        {
            isPushed = false;
            StopHolding();
        }

        else if(IsHovering && SceneInputHandler.Instance.IsClickCurrentlyPressed && !isPushed)
        {
            isPushed = true;
            StartHolding();
        }
    }

    [ObserversRpc]
    private void StartHolding()
    {
        isPushed = true;
        OnHoldButton?.Invoke();
        Debug.Log("button is pressed");
    }


    [ObserversRpc]
    private void StopHolding()
    {
        isPushed = false;
        OnStopHoldButton?.Invoke();
        Debug.Log("button is released");
    }
}
