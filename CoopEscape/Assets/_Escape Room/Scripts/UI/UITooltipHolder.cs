using TMPro;
using UnityEngine;

public class UITooltipHolder : MonoSingleton<UITooltipHolder>
{
    [SerializeField] private TextMeshProUGUI tooltipText;

    private void Start()
    {
        HideTooltip();
    }

    public void ShowTooltip(string tooltip)
    {
        tooltipText.text = tooltip;
        this.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        this.gameObject.SetActive(false);
    }

}
