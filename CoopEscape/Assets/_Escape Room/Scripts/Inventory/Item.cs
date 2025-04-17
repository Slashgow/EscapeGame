using System;
using PurrNet;
using UnityEngine;

public class Item : AInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemPicture;
    [SerializeField] private Rigidbody rigidbody;

    protected override void OnOwnerChanged(PlayerID? oldOwner, PlayerID? newOwner, bool asServer)
    {
        base.OnOwnerChanged(oldOwner, newOwner, asServer);

        if (PlayerInventory.localInventory.IsHoldingItem(this))
        {
            rigidbody.isKinematic = true;
            return;
        }
        rigidbody.isKinematic = !isOwner;
    }
    public string ItemName => itemName;
    public Sprite ItemPicture => itemPicture;

    public override void Interact()
    {
        Pickup();
    }

    [ContextMenu("Test Pickup")]
    public void Pickup()
    {
        if (!InstanceHandler.TryGetInstance(out InventoryManager inventoryManager))
        {
            Debug.LogError($"Couldn't get inventory manager for item {itemName}", this);
            return;
        }

            
        inventoryManager.AddItem(this);
        Destroy(gameObject);
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

    internal void SetKinematic(bool toggle)
    {
        rigidbody.isKinematic = toggle;
    }
}
