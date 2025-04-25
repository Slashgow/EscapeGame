using UnityEngine;
using UnityEngine.InputSystem;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private string startTooltip, endTooltip;
    [SerializeField] private InputActionReference inputActionReference;

    public void ShowTooltip()
    {
        UITooltipHolder.Instance.ShowTooltip($"{startTooltip} {inputActionReference.action.GetBindingDisplayString()} {endTooltip}");
    }

    public void HideTooltip()
    {
        UITooltipHolder.Instance.HideTooltip();
    }


}
