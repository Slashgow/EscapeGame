using System;
using PurrNet;
using QuickOutline;
using UnityEngine;

public class DropInteractor : AInteractable, IDropInteractor
{
    [SerializeField] private Transform dropPoint;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material previewMaterial;

    public event Action OnInteractableEnter = delegate { };

    private Item currentPreviewItem;

    public bool IsEmpty { get; set; } = true;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        meshRenderer.sharedMaterial = previewMaterial;
    }

    [ObserversRpc]
    public void Drop(AInteractable interactable)
    {
        Debug.Log($"receive drop item : {interactable}");
        interactable.transform.SetParent(dropPoint);
        interactable.transform.localPosition = Vector3.zero;
        interactable.transform.localRotation = Quaternion.identity;
        interactable.transform.localScale = Vector3.one;
        meshRenderer.enabled = false;
        DisableOutline(interactable);
        OnInteractableEnter?.Invoke();
        IsEmpty = false;
    }

    //[ObserversRpc]
    public void DisableOutline(AInteractable interactable)
    {
        interactable.GetComponent<OutlineHoverInteractable>().enabled = false;
        interactable.GetComponent<Outline>().enabled = false;
    }
    public override void Interact()
    {
        
    }

    public override void OnHover()
    {
        base.OnHover();
        
        if (!IsEmpty)
            return;

        Debug.Log($"on hover {this.name}");
        if(PlayerInventory.localInventory.ItemInHand != null)
        {
            meshRenderer.enabled = true;

            if (currentPreviewItem != null && currentPreviewItem == PlayerInventory.localInventory.ItemInHand)
                return;

          
            currentPreviewItem = PlayerInventory.localInventory.ItemInHand;
            meshFilter.mesh = currentPreviewItem.GetComponent<MeshFilter>().sharedMesh;
        }
    }

    public override void OnStopHover()
    {
        base.OnStopHover();

        meshRenderer.enabled = false;
    }
}
