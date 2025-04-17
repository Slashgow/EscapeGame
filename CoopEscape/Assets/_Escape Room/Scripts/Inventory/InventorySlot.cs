using PurrNet;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    private InventoryItem item;
    public bool IsEmpty => item == null;
    public InventoryItem Item => item;

    public void SetItem(InventoryItem p_item) => item = p_item;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag == null)
            return;

        eventData.pointerDrag.transform.SetParent(transform);
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        inventoryItem.SetAvailable();

        if(!InstanceHandler.TryGetInstance(out InventoryManager inventoryManager))
        {
            Debug.LogError("Failed to get inventory manager", this);
            return;
        }

        inventoryManager.ItemMoved(inventoryItem, this);
    }

}
