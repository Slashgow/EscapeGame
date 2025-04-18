using System;
using System.Collections.Generic;
using PurrNet;
using PurrNet.Utils;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<Item> allItems = new List<Item>();
    [SerializeField] private SceneInputHandler inventoryInputHandler;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();
    [SerializeField] private List<ActionSlot> actionSlots = new List<ActionSlot>();

    [PurrReadOnly, SerializeField] private InventoryItemData[] inventoryData;
    private ActionSlot activeActionSlot;

    private void Awake()
    {
        InstanceHandler.RegisterInstance(this);
        inventoryData = new InventoryItemData[slots.Count];
        ToggleInventory(false);
        //SetActionSlotActive(actionSlots[0]);
    }
    private void OnDestroy()
    {
        InstanceHandler.UnregisterInstance<InventoryManager>();
    }

    private void Update()
    {
        if (inventoryInputHandler.InventoryTriggered)
        {
            Debug.Log("inventory triggered");
            bool isOpen = canvasGroup.alpha > 0f;
            ToggleInventory(!isOpen);
        }

        if (inventoryInputHandler.ScrollValue.y >= 1f)
            ActiveNextActionSlot();
        else if (inventoryInputHandler.ScrollValue.y <= -1f)
            ActivePreviousActionSlot();
       
    }

    private void ActivePreviousActionSlot()
    {
        SetActionSlotActive(actionSlots[MathsUtility.Modulo(actionSlots.IndexOf(activeActionSlot) - 1, actionSlots.Count)]);
    }

    private void ActiveNextActionSlot()
    {
        SetActionSlotActive(actionSlots[MathsUtility.Modulo(actionSlots.IndexOf(activeActionSlot) + 1, actionSlots.Count)]);
    }
    public void SetActionSlotActive(ActionSlot actionSlot)
    {
        if (activeActionSlot == actionSlot)
            return;

        if (activeActionSlot != null)
        {
            activeActionSlot.ToggleActive(false);
            PlayerInventory.localInventory.UnequipedItem(GetItemByActionSlot(activeActionSlot));
        }
       

        actionSlot.ToggleActive(true);
        activeActionSlot = actionSlot;

        PlayerInventory.localInventory.EquipItem(GetItemByActionSlot(actionSlot));
    }
    private void ToggleInventory(bool toggle)
    {
        canvasGroup.alpha = toggle ? 1f : 0f;
        canvasGroup.blocksRaycasts = toggle;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = toggle;
    }

    public void ItemMoved(InventoryItem inventoryItem, InventorySlot newSlot)
    {
        int newSlotIndex = slots.IndexOf(newSlot);
        int oldSlotIndex = Array.FindIndex(inventoryData, x => x.inventoryItem == inventoryItem);
        if(oldSlotIndex == -1)
        {
            Debug.LogError($"Couldn't find item {inventoryItem.name} in inventory data!", this);
        }
        InventoryItemData oldData = inventoryData[oldSlotIndex];
        inventoryData[oldSlotIndex] = default;
        inventoryData[newSlotIndex] = oldData;

        if (!newSlot.TryGetComponent(out ActionSlot actionSlot))
        {
            Item item = GetItemByName(oldData.itemName);
            if (PlayerInventory.localInventory.IsHoldingItem(item))
            {
                PlayerInventory.localInventory.UnequipedItem(item);
            }
        }
    }

    public void AddItem(Item item)
    {
        if (!TryStackItem(item))
        {
            AddNewItem(item);
        }
    }

    private bool TryStackItem(Item item)
    {
        for (int i = 0; i < inventoryData.Length; i++)
        {
            InventoryItemData data = inventoryData[i];
            if(string.IsNullOrEmpty(data.itemName))
                continue;

            if (data.itemName != item.ItemName)
                continue;

            data.amount++;
            data.inventoryItem.Init(item.ItemName, item.ItemPicture, data.amount);
            inventoryData[i] = data;
            return true;
        }
        return false;
    }

    private void AddNewItem(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlot slot = slots[i];
            if (!slot.IsEmpty)
                continue;

            InventoryItem inventoryItem = Instantiate(itemPrefab, slot.transform);
            inventoryItem.Init(item.ItemName, item.ItemPicture, 1);

            var itemData = new InventoryItemData()
            {
                itemName = item.ItemName,
                itemPicture = item.ItemPicture,
                inventoryItem = inventoryItem,
                amount = 1
            };
            inventoryData[i] = itemData;
            slot.SetItem(inventoryItem);
            return;
        }
    }

    internal void DropItem(InventoryItem inventoryItem)
    {
        for (int i = 0; i < inventoryData.Length; i++)
        {
            var data = inventoryData[i];

            if(data.inventoryItem != inventoryItem)
                continue;

            var itemToSpawn = GetItemByName(data.itemName);
            if (itemToSpawn == null)
            {
                Debug.LogError($"item to spawn with name {data.itemName}", this);
                return;
            }

            PlayerInventory.localInventory.UnequipedItem(itemToSpawn);

            Vector3 spawnPosition = Player.localPlayerInstance.transform.position + Player.localPlayerInstance.transform.forward + Vector3.up;
            Item item = Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);


            if(DeductItem(inventoryItem) <=0)
                PlayerInventory.localInventory.UnequipedItem(itemToSpawn);
            break;
        }
    }

    private Item GetItemByName(string itemName) => allItems.Find(x => x.ItemName == itemName);
    private Item GetItemByActionSlot(ActionSlot actionSlot)
    {
        InventorySlot inventorySlot = actionSlot.GetComponent<InventorySlot>();
        for(int i = slots.Count - 1; i >= 0 ; i--)
        {
            if(slots[i] == inventorySlot)
            {
                return GetItemByName(inventoryData[i].itemName);
            }
        }
        return null;
    }
    private int DeductItem(InventoryItem inventoryItem)
    {
        for (int i = 0; i < inventoryData.Length; i++)
        {
            var data = inventoryData[i];

            if (data.inventoryItem != inventoryItem)
                continue;

            data.amount--;
            if(data.amount <= 0)
            {
                inventoryData[i] = default;
                slots[i].SetItem(null);
                Destroy(inventoryItem.gameObject);
                return 0;
            }
            else
            {
                data.inventoryItem.Init(data.itemName, data.itemPicture, data.amount);
                inventoryData[i] = data;
                return data.amount;
            }
        }

        return 0;
    }

    [Serializable]
    public struct InventoryItemData
    {
        public string itemName;
        public Sprite itemPicture;
        public InventoryItem inventoryItem;
        public int amount;
    }
}
