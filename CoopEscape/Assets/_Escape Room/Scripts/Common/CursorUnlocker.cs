using UnityEngine;

public class CursorUnlocker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            Debug.Log("enter cursor zone");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.TryGetComponent(out Player player))
        {
            Debug.Log("exit cursor zone");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
