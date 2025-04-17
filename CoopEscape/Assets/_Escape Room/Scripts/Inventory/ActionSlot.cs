using UnityEngine;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour
{
    [SerializeField] private Image slotImage;
    [SerializeField] private Color activeColor;

    private Color originalColor;
    private void Awake()
    {
        originalColor = slotImage.color;
    }

    public void ToggleActive(bool toggle)
    {
        slotImage.color = toggle ? activeColor : originalColor;
    }
}
