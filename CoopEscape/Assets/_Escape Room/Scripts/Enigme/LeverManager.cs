using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LeverManager : MonoBehaviour
{
    [SerializeField] private List<Lever> levers;

    private bool isUnlocked;

    public UnityEvent OnStartUnlockUnityEvent, OnStopUnlockUnityEvent;

    public event Action OnStartUnlock;
    public event Action OnStopUnlock;

    private void Awake()
    {
        isUnlocked = false;
        levers.ForEach(lever => lever.OnToggle.AddListener(UpdateUnlock));

    }

    private void UpdateUnlock()
    {
        if (!isUnlocked && IsAllLeverEnabled())
        {
            Debug.Log("Start Unlock lever");
            OnStartUnlock?.Invoke();
            OnStartUnlockUnityEvent?.Invoke();
            isUnlocked = true;
        }
        else if(isUnlocked && !IsAllLeverEnabled())
        {
            Debug.Log("Stop Unlock lever");
            isUnlocked = false;
            OnStopUnlock?.Invoke();
            OnStopUnlockUnityEvent?.Invoke();
        }
    }

    private bool IsAllLeverEnabled() => levers.All(x => x.IsEnabled);
}
