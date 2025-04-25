using System;
using PurrNet;
using UnityEngine;

public class Ignitable : AInteractable, IIgnitable
{
    [SerializeField] private GameObject flame, flameLight;
    public GameObject Flame => flame;

    [SerializeField] private AudioSource audioSource;
    public AudioSource AudioSource => audioSource;

    public bool IsIgnited { get; set; } = true;

    public event Action<Ignitable> OnStartIgnite = delegate { };
    public event Action OnUnignite = delegate { };

    protected override void OnSpawned()
    {
        base.OnSpawned();

        IsIgnited = false;
        flame.SetActive(false);
        flameLight.SetActive(false);
    }

    [ObserversRpc]
    public void Ignite()
    {
        if(IsIgnited)
            return;

        Debug.Log("ignite");
        IsIgnited = true;
        OnStartIgnite?.Invoke(this);
        flame.SetActive(true);
        flameLight.SetActive(true);
        audioSource.Play();
    }

    [ObserversRpc]
    public void Unignite()
    {
        if(!IsIgnited)
            return;

        Debug.Log("unignite");
        IsIgnited = false;
        flame.SetActive(false);
        flameLight.SetActive(false);
        OnUnignite?.Invoke();
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
