using UnityEngine;

public class InteractableTooltip : MonoBehaviour
{
    [SerializeField] private AInteractable interactable;
    [SerializeField] private Tooltip tooltip;

    private void OnEnable()
    {
        interactable.OnHoverStart += Interactable_OnHoverStart;
        interactable.OnHoverStop += Interactable_OnHoverStop;
        interactable.OnInteract += Interactable_OnInteract;
    }
    private void OnDisable()
    {
        interactable.OnHoverStart -= Interactable_OnHoverStart;
        interactable.OnHoverStop -= Interactable_OnHoverStop;
        interactable.OnInteract -= Interactable_OnInteract;
    }
    private void Interactable_OnInteract()
    {
        tooltip.HideTooltip();
    }


    private void Interactable_OnHoverStop()
    {
        tooltip.HideTooltip();
    }

    private void Interactable_OnHoverStart()
    {
            tooltip.ShowTooltip();
    }
}
