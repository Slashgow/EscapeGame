using System;
using PurrNet;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] TMP_Text amountText;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Canvas canvas;
    private Image itemImage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        itemImage = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        rectTransform.SetParent(rectTransform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
         rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter == null || !eventData.pointerEnter.TryGetComponent(out InventorySlot inventorySlot))
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            rectTransform.SetParent(originalParent);
            SetAvailable();
        }
      
    }

    internal void SetAvailable()
    {
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void Init(string itemName, Sprite itemPicture, int amount)
    {
        itemImage.sprite = itemPicture;
        amountText.text = amount.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (!InstanceHandler.TryGetInstance(out InventoryManager inventoryManager))
        {
            Debug.LogError($"failed to get inventory manager to drop item");
            return;
        }

        inventoryManager.DropItem(this);
    }
}
