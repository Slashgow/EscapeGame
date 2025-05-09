using Unity.VisualScripting;
using UnityEngine;

public class CursorUnlocker : MonoBehaviour
{
    [SerializeField] private bool showCursorOnStart;
    private void Awake()
    {
        if (showCursorOnStart)
            ShowCursor();
    }
    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
