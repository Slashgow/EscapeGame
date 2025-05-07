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

    private void QuitMenu()
    {
        GameManager.Instance.IsInMenu = false;
        cursorUnlocker.HideCursor();
        //this.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        OnInteract?.Invoke();
        GameManager.Instance.IsInMenu = true;
        cursorUnlocker.ShowCursor();
        FirstPersonController.localFirstPersonController.MovePlayerTo(fixTransform, timeToMove, easing);
        FirstPersonController.localFirstPersonController.MoveVerticalRotationTo(verticalRotationCamera.rotation, timeToMove, easing);
    }

    
}
