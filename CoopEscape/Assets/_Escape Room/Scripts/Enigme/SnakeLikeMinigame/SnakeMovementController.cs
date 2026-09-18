using UnityEditor.Rendering;
using UnityEngine;

public class SnakeMovementController : MonoBehaviour
{
    [SerializeField] private NetworkPlayerInputHandler networkPlayerInputHandler;
    [SerializeField] private Vector2 boundsMin = new Vector2(-5f, -5f); // Minimum boundary
    [SerializeField] private Vector2 boundsMax = new Vector2(5f, 5f); // Maximum boundary

    private float spriteWidth;
    private float spriteHeight;
    private Vector3 newPosition;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteWidth = spriteRenderer.bounds.size.x / this.transform.localScale.x;
            spriteHeight = spriteRenderer.bounds.size.y;

            Debug.Log($"sprite width : {spriteWidth} || sprite height : {spriteHeight}");
        }
    }

    private void OnEnable()
    {
        networkPlayerInputHandler.OnMoveToDown += NetworkPlayerInputHandler_OnMoveToDown;
        networkPlayerInputHandler.OnMoveToTop += NetworkPlayerInputHandler_OnMoveToTop;
        networkPlayerInputHandler.OnMoveToLeft += NetworkPlayerInputHandler_OnMoveToLeft;
        networkPlayerInputHandler.OnMoveToRight += NetworkPlayerInputHandler_OnMoveToRight;
    }

    private void OnDisable()
    {
        networkPlayerInputHandler.OnMoveToDown -= NetworkPlayerInputHandler_OnMoveToDown;
        networkPlayerInputHandler.OnMoveToTop -= NetworkPlayerInputHandler_OnMoveToTop;
        networkPlayerInputHandler.OnMoveToLeft -= NetworkPlayerInputHandler_OnMoveToLeft;
        networkPlayerInputHandler.OnMoveToRight -= NetworkPlayerInputHandler_OnMoveToRight;
    }
    private void NetworkPlayerInputHandler_OnMoveToRight()
    {
        newPosition = transform.localPosition;
        if (newPosition.z - spriteWidth >= boundsMin.x)
        {
            newPosition.z -= spriteWidth;
        }
        transform.localPosition = newPosition;
    }

    private void NetworkPlayerInputHandler_OnMoveToLeft()
    {
        newPosition = transform.localPosition;
        if (newPosition.z + spriteWidth <= boundsMax.x)
        {
            newPosition.z += spriteWidth;
        }
        transform.localPosition = newPosition;
    }

    private void NetworkPlayerInputHandler_OnMoveToTop()
    {
        newPosition = transform.localPosition;
        if (newPosition.y + spriteHeight <= boundsMax.y)
        {
            newPosition.y += spriteHeight;
        }
        transform.localPosition = newPosition;
    }

    private void NetworkPlayerInputHandler_OnMoveToDown()
    {
        newPosition = transform.localPosition;
        if (newPosition.y - spriteHeight >= boundsMin.y)
        {
            newPosition.y -= spriteHeight;
        }
        transform.localPosition = newPosition;
    }
}
