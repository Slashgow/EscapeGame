using System;
using PurrNet;
using UnityEngine;

public class Ignitable : AInteractable, IIgnitable
{
    [SerializeField] private GameObject flame;
    public GameObject Flame => flame;

    [SerializeField] private AudioSource audioSource;
    public AudioSource AudioSource => audioSource;

    public bool IsIgnited { get; set; } = false;

    public event Action<Ignitable> OnStartIgnite = delegate { };
    public event Action OnUnignite = delegate { };

    [ObserversRpc]
    public void Ignite()
    {
        if(IsIgnited)
            return;

        IsIgnited = true;
        OnStartIgnite?.Invoke(this);
        flame.SetActive(true);
        audioSource.Play();
    }

    [ObserversRpc]
    public void Unignite()
    {
        if(!IsIgnited)
            return;

        OnUnignite?.Invoke();
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
