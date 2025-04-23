using System;
using UnityEngine;
using UnityEngine.Events;

public class CoupledButton : MonoBehaviour
{
    [SerializeField] private InteractableButton button1, button2;

    public UnityEvent OnStartBothButtonsPushed;
    public UnityEvent OnStopBothButtonsPushed;

    private bool isBothButtonsPushed;

    private void Start()
    {
        button1.OnHoldButton.AddListener(OnHoldButton);
        button2.OnHoldButton.AddListener(OnHoldButton);

        button1.OnStopHoldButton.AddListener(OnStopHoldButton);
        button2.OnStopHoldButton.AddListener(OnStopHoldButton);
    }

    private void OnDisable()
    {
        button1.OnHoldButton.RemoveListener(OnHoldButton);
        button2.OnHoldButton.RemoveListener(OnHoldButton);

        button1.OnStopHoldButton.RemoveListener(OnStopHoldButton);
        button2.OnStopHoldButton.RemoveListener(OnStopHoldButton);
    }

    private void OnStopHoldButton()
    {
        if (isBothButtonsPushed)
        {
            isBothButtonsPushed = false;
            OnStopBothButtonsPushed?.Invoke();
            Debug.Log("On stop Hold both buttons");
        }
    }

    private void OnHoldButton()
    {
        if(!isBothButtonsPushed && button1.IsPushed && button2.IsPushed)
        {
            isBothButtonsPushed = true;
            OnStartBothButtonsPushed.Invoke();
            Debug.Log("On start Hold both buttons");
        }
    }
}
