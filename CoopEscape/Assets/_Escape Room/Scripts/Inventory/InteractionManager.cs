using System;
using PurrNet;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private SceneInputHandler sceneInputHandler;

    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private float interactionDistance = 4f;

    private Camera cam;
    private AInteractable[] currentHoveredInteractables;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HandleHovers();

        if (!sceneInputHandler.ClickPressed)
            return;

        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit raycastHit, interactionDistance, interactableLayerMask))
            return;

        var interactables = raycastHit.collider.GetComponents<AInteractable>();
        foreach (var interactable in interactables)
        {
            if(interactable.CanInteract())
                interactable.Interact();
        }
    }

    private void HandleHovers()
    {
        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit raycastHit, interactionDistance, interactableLayerMask))
        {
            ClearHover();
            return;
        }

        var interactables = raycastHit.collider.GetComponents<AInteractable>();
        if(interactables == null || interactables.Length == 0)
        {
            ClearHover();
            return;
        }

        if(currentHoveredInteractables != null && currentHoveredInteractables.Length > 0)
        {
            if (!currentHoveredInteractables[0])
            {
                ClearHover();
                return ;
            }

            if(raycastHit.collider.gameObject == currentHoveredInteractables[0].gameObject)
                return;
        }
         

        currentHoveredInteractables = interactables;
        foreach (var interactable in interactables)
        {
            if (interactable.CanInteract())
                interactable.OnHover();

        }
    }

    private void ClearHover()
    {
        if (currentHoveredInteractables == null || currentHoveredInteractables.Length <= 0)
            return;

        foreach (var interactable in currentHoveredInteractables)
        {
            if(interactable)
                interactable.OnStopHover();
        }

        currentHoveredInteractables = null;
    }
}

public abstract class AInteractable : NetworkBehaviour
{
    public abstract void Interact();
    public event Action OnHoverStart = delegate { };
    public virtual void OnHover() { OnHoverStart?.Invoke(); }
    public event Action OnHoverStop = delegate { };
    public virtual void OnStopHover() { OnHoverStop?.Invoke(); }
    public virtual bool CanInteract()
    {
        return true;
    }

}
