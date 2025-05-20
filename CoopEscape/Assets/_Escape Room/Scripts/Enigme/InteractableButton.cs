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
    [SerializeField, Range(0f,2f)] private float cooldownButton = 0.3f;

    [SerializeField] private bool isDebugHold;

    public UnityEvent OnHoldButton;
    public UnityEvent OnStopHoldButton;
    public UnityEvent OnClick;
    public event Action<InteractableButton> OnClickInteractable = delegate { };

    public bool IsPushed => isPushed;
    private bool isPushed;
    private float timeElapsed = 0.0f;

    [ObserversRpc]
    public override void Interact()
    {
        if(buttonInteraction == ButtonInteraction.Click && !isPushed && timeElapsed >= cooldownButton)
        {
            if(triggerClickOnlyOnce)
                isPushed = true;

            timeElapsed = 0.0f;
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
                Debug.Log("start holding debug");
                StartHoldingLocal();
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
        if(timeElapsed < cooldownButton)
            timeElapsed += Time.deltaTime;

        if(buttonInteraction != ButtonInteraction.Hold || isDebugHold)
            return;

        else if(IsHovering && isPushed && !SceneInputHandler.Instance.IsClickCurrentlyPressed && timeElapsed>= cooldownButton)
        {
            timeElapsed = 0.0f;
            isPushed = false;
            StopHolding();
        }

        else if(IsHovering && SceneInputHandler.Instance.IsClickCurrentlyPressed && !isPushed && timeElapsed >= cooldownButton)
        {
            timeElapsed = 0.0f;
            isPushed = true;
            Debug.Log("start holding cd");
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

    private void StartHoldingLocal()
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
