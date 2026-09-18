using UnityEngine;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private SceneInputHandler sceneInputHandler;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        ToggleMenuSettings(false);
    }
    private void Update()
    {
        if (sceneInputHandler.IsToggleSettingsPressed && !InteractableCursorZoneManager.Instance.IsInInteractableCursorZone)
        {
            Debug.Log("open settings menu");
            bool isOpen = canvasGroup.alpha > 0f;
            ToggleMenuSettings(!isOpen);
        }
    }
    public void ToggleMenuSettings(bool toggle)
    {
        canvasGroup.alpha = toggle ? 1f : 0f;
        canvasGroup.blocksRaycasts = toggle;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = toggle;
    }
}
