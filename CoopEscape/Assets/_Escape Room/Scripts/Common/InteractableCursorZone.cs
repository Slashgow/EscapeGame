using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InteractableCursorZone : AInteractable
{
    [SerializeField, Range(0f, 3f)] private float timeToMove = 1f;
    [SerializeField] private Ease easing;
    [SerializeField] private Transform fixTransform;
    [SerializeField] private Transform verticalRotationCamera;
    [SerializeField] private CursorUnlocker cursorUnlocker;
    [SerializeField] private Button quitFixButton;

    public override event Action OnInteract = delegate { };
    private void OnEnable() => quitFixButton.onClick.AddListener(QuitMenu);
    private void OnDisable() => quitFixButton.onClick.RemoveListener(QuitMenu);

    public void QuitMenu()
    {
        GameManager.Instance.IsInMenu = false;
        InteractableCursorZoneManager.Instance.QuitCursorZone();
        cursorUnlocker.HideCursor();
        //this.gameObject.SetActive(false);
        Debug.Log("quit menu");
    }

    public override void Interact()
    {
        OnInteract?.Invoke();
        GameManager.Instance.IsInMenu = true;
        InteractableCursorZoneManager.Instance.EnterCursorZone(this);
        cursorUnlocker.ShowCursor();
        FirstPersonController.localFirstPersonController.MovePlayerTo(fixTransform, timeToMove, easing);
        FirstPersonController.localFirstPersonController.MoveVerticalRotationTo(verticalRotationCamera.rotation, timeToMove, easing);
    }

    
}
