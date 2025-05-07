using System;
using UnityEngine;

public interface IDropInteractor 
{
    public bool IsEmpty { get; set; }
    public void Drop(AInteractable interactable);
    public event Action OnInteractableEnter;
}
