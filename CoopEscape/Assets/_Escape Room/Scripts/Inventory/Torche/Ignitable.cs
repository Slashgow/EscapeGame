using System;
using PurrNet;
using UnityEngine;

public class Ignitable : AInteractable, IIgnitable
{
    [SerializeField]
    private GameObject flame;

    public bool IsIgnited { get; set; } = false;

    public event Action<Ignitable> OnStartIgnite = delegate { };

    [ObserversRpc]
    public void Ignite()
    {
        if(IsIgnited)
            return;

        IsIgnited = true;
        OnStartIgnite?.Invoke(this);
        flame.SetActive(true);
    }

    [ObserversRpc]
    public void Unignite()
    {
        if(!IsIgnited)
            return;

        IsIgnited = false;
        flame.SetActive(false);
    }
    public override void Interact()
    {

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
