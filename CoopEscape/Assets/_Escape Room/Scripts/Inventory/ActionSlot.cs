using UnityEngine;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour
{
    [SerializeField] private Image slotImage;
    [SerializeField] private Sprite activeSprite;

    private Sprite originalSprite;
    private void Awake()
    {
        originalSprite = slotImage.sprite;
    }

    public void ToggleActive(bool toggle)
    {
        slotImage.sprite = toggle ? activeSprite : originalSprite;
    }
}
