using System;
using PurrNet;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour
{
    public static PlayerInventory localInventory;

    [SerializeField] private Transform itemAttachPoint;

    private Item itemInHand;
    protected override void OnSpawned()
    {
        base.OnSpawned();

        if(!isOwner)
            return;

        localInventory = this;
    }

    protected override void OnDespawned()
    {
        base.OnDespawned();

        if(!isOwner )
            return;

        localInventory = null;
    }

    public void EquipItem(Item item)
    {
        if (!item)
            return;

        itemInHand = Instantiate(item, itemAttachPoint.position, Quaternion.identity, itemAttachPoint);
        itemInHand.SetKinematic(true);
        Debug.Log($"equip item {item.ItemName} ");
    }

    public void UnequipedItem(Item item)
    {
        if (!item)
            return;

        if (!itemInHand)
            return;

        if (itemInHand.ItemName != item.ItemName)
            return;

        Destroy(itemInHand.gameObject);
        itemInHand = null;
        Debug.Log($"unequip item {item.ItemName} ");
    }

    public bool IsHoldingItem(Item item)
    {
        if(!itemInHand) 
            return false;

        return item = itemInHand;
    }
}
