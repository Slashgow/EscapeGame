using UnityEngine;

public class SnakeMovementController : MonoBehaviour
{
    [SerializeField] private NetworkPlayerInputHandler networkPlayerInputHandler;


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
        throw new System.NotImplementedException();
    }

    private void NetworkPlayerInputHandler_OnMoveToLeft()
    {
        throw new System.NotImplementedException();
    }

    private void NetworkPlayerInputHandler_OnMoveToTop()
    {
        throw new System.NotImplementedException();
    }

    private void NetworkPlayerInputHandler_OnMoveToDown()
    {
        throw new System.NotImplementedException();
    }
}
