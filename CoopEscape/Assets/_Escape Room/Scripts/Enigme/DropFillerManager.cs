using System.Collections.Generic;
using PurrNet;
using UnityEngine;
using UnityEngine.Events;

public class DropFillerManager : MonoBehaviour
{
    [SerializeField] private List<DropInteractor> dropInteractors = new List<DropInteractor>();

    public UnityEvent OnUnlock;
    private int dropInteractorFilledCount = 0;
    private void OnEnable()
    {
        dropInteractors.ForEach(dropInteractor => dropInteractor.OnInteractableEnter += DropInteractor_OnInteractableEnter);
    }

    private void OnDisable()
    {
        dropInteractors.ForEach(dropInteractor => dropInteractor.OnInteractableEnter -= DropInteractor_OnInteractableEnter);
    }

    
    private void DropInteractor_OnInteractableEnter()
    {
        dropInteractorFilledCount++;
        Debug.Log("On try Unlock");

        if (dropInteractorFilledCount >= dropInteractors.Count)
        {
            Debug.Log("On Unlock");
            OnUnlock?.Invoke();
        }
    }
}
