using PurrNet;
using UnityEngine;
using UnityEngine.Events;

public class Lever : AInteractable
{
    public UnityEvent OnToggle;
    public bool IsEnabled {  get; private set; }

    [ObserversRpc]
    public override void Interact()
    {
        IsEnabled = !IsEnabled;
        OnToggle?.Invoke();
    }

    public override void OnHover()
    {
        base.OnHover();
        Debug.Log("On Start Hover");
    }

    public override void OnStopHover()
    {
        base.OnStopHover();
        Debug.Log("On stop Hover");
    }
}
