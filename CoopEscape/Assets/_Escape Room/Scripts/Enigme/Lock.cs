using System;
using System.Collections.Generic;
using System.Linq;
using PurrNet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Lock : NetworkBehaviour
{
    [SerializeField] private Button validationButton;
    [SerializeField] private List<LockNumber> lockNumbers;

    public UnityEvent OnUnlockUnityEvent; 
    public bool IsUnlocked { get; private set; }
    public event Action OnUnlock;

    protected override void OnSpawned()
    {
        base.OnSpawned();
        validationButton.onClick.AddListener(TryUnlock);
    }

    private void OnDisable()
    {
        validationButton.onClick.RemoveListener(TryUnlock);
    }

    [ObserversRpc]
    private void TryUnlock()
    {
        Debug.Log("Try Unlock");
        if (IsAllNumbersCorrect())
        {
            IsUnlocked = true;
            OnUnlockUnityEvent?.Invoke();
            OnUnlock?.Invoke();
            Debug.Log("Is Unlocked");
        }
    }

    private bool IsAllNumbersCorrect()
    {
        if (lockNumbers.All(x => x.IsUnlocked))
            return true;

        return false;
    }
}
