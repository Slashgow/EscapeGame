using PurrNet;
using UnityEngine;

public class Fuse : Item, IDroppable
{
    [SerializeField, Range(0f, 10f)] private float interactionDistance = 4;
    [SerializeField] private LayerMask dropLayerMask;

    public InventoryItem inventoryItem;

    private Camera cam;

    public bool IsDropped { get; set; }

    private void Awake()
    {
        cam = Camera.main;
    }

    public override void Use()
    {
        base.Use();

        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit raycastHit, interactionDistance, dropLayerMask))
            return;

        var dropInteractors = raycastHit.collider.GetComponents<IDropInteractor>();
        foreach (var dropInteractor in dropInteractors)
        {
            if(!dropInteractor.IsEmpty)
                continue;

            if (!InstanceHandler.TryGetInstance(out InventoryManager inventoryManager))
            {
                Debug.LogError($"failed to get inventory manager to drop item");
                return;
            }
            inventoryItem = inventoryManager.GetInventoryItemByName(this.ItemName);
            Debug.Log(inventoryItem);

            inventoryManager.DropItemWithoutDestroy(inventoryItem);
            //Debug.Log($"drop item {item}");
            dropInteractor.Drop(this);
            SetDropStatus(true);


        }
    }

    [ObserversRpc]
    public void SetDropStatus(bool isDropped)
    {
        IsDropped = isDropped;
    }

    public override bool CanInteract()
    {
        return !isHeld && !IsDropped;
    }
}
