using UnityEngine;
using UnityEngine.UI;

public class UIPushToTalk : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(ChangeHoldToTalk);
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(ChangeHoldToTalk);
    }

    private void ChangeHoldToTalk(bool toggle)
    {
        BindingManager.Instance.SetHoldToTalk(toggle);
    }
}
