using UnityEngine;

public class TorcheTooltip : MonoBehaviour
{
    [SerializeField] private AInteractable staticTorche;
    [SerializeField] private Tooltip tooltip;

    private void Start()
    {
        staticTorche.OnHoverStart += StaticTorche_OnHoverStart;
        staticTorche.OnHoverStop += StaticTorche_OnHoverStop;
    }

    private void StaticTorche_OnHoverStop()
    {
        tooltip.HideTooltip();
    }

    private void StaticTorche_OnHoverStart()
    {
        if (PlayerInventory.localInventory.ItemInHand == null)
            return;

        if (PlayerInventory.localInventory.ItemInHand is Torche)
            tooltip.ShowTooltip();
    }
}
