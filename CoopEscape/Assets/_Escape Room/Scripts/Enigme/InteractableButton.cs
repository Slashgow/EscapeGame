using System;
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
    [SerializeField] private bool triggerClickOnlyOnce;

    [SerializeField] private bool isDebugHold;

    public UnityEvent OnHoldButton;
    public UnityEvent OnStopHoldButton;
    public UnityEvent OnClick;
    public event Action<InteractableButton> OnClickInteractable = delegate { };

    public bool IsPushed => isPushed;
    private bool isPushed;

    [ObserversRpc]
    public override void Interact()
    {
        if(buttonInteraction == ButtonInteraction.Click && !isPushed)
        {
            if(triggerClickOnlyOnce)
                isPushed = true;

            OnClick?.Invoke();
            OnClickInteractable?.Invoke(this);
        }

        if (isDebugHold)
        {
            if (isPushed)
            {
                isPushed = false;
                StopHolding();
            }
            else 
            {
                isPushed = true;
                StartHolding();
            }
        }
    }

    public override void OnStopHover()
    {
        base.OnStopHover();

        if (buttonInteraction != ButtonInteraction.Hold || isDebugHold)
            return;

        if (!isPushed )
            return;
        
        isPushed = false;
        StopHolding();
    }

    private void Update()
    {
        if(buttonInteraction != ButtonInteraction.Hold || isDebugHold)
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
