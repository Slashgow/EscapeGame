using System;
using PurrNet;
using UnityEngine;

public class Item : AInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemPicture;
    [SerializeField] private Rigidbody rigidbody;

    protected bool isHeld;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        if (PlayerInventory.localInventory != null && PlayerInventory.localInventory.IsHoldingItem(this))
        {
            SetHoldStatus(true);
        }

        //Debug.Log($"{itemName} spawned");
    }

    protected override void OnOwnerChanged(PlayerID? oldOwner, PlayerID? newOwner, bool asServer)
    {
        base.OnOwnerChanged(oldOwner, newOwner, asServer);

        if (PlayerInventory.localInventory.IsHoldingItem(this))
        {
            rigidbody.isKinematic = true;
            Debug.Log("holding item on owner changed");
            SetHoldStatus(true);
            return;
        }
        rigidbody.isKinematic = false;
    }
    public string ItemName => itemName;
    public Sprite ItemPicture => itemPicture;

    public override void Interact()
    {
        Pickup();
    }

    [ObserversRpc]
    public void SetHoldStatus(bool p_isHeld)
    {
        Debug.Log($"set hold status {itemName} to {p_isHeld}");
        isHeld = p_isHeld;
    }
    public override bool CanInteract() => !isHeld;

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
        //Debug.Log("On Start Hover");
    }

    public override void OnStopHover()
    {
        base.OnStopHover();
        //Debug.Log("On stop Hover");
    }

    internal void SetKinematic(bool toggle)
    {
        rigidbody.isKinematic = toggle;
    }
}
