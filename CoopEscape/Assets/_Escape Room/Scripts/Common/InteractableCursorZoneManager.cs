using System.Collections;
using UnityEngine;

public class InteractableCursorZoneManager : MonoSingleton<InteractableCursorZoneManager>
{
    [SerializeField] private SceneInputHandler sceneInputHandler;
    public bool IsInInteractableCursorZone { get; private set; }
    private InteractableCursorZone currentInteractableCursorZone;

    public void EnterCursorZone(InteractableCursorZone interactableCursorZone)
    {
        IsInInteractableCursorZone = true;
        currentInteractableCursorZone = interactableCursorZone;
    }

    public void QuitCursorZone()
    {
        //IsInInteractableCursorZone = false;
        //currentInteractableCursorZone = null;
        StartCoroutine(QuitCursorZoneCoroutine());
    }

    private IEnumerator QuitCursorZoneCoroutine()
    {
        yield return new WaitForEndOfFrame();

        IsInInteractableCursorZone = false;
        currentInteractableCursorZone = null;
    }

    private void Update()
    {
        if (sceneInputHandler.IsToggleSettingsPressed && IsInInteractableCursorZone)
        {
            if (currentInteractableCursorZone == null)
                return;

            currentInteractableCursorZone.QuitMenu();
        }
    }
}
