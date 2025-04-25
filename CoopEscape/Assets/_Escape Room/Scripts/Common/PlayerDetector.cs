using PurrNet;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    public UnityEvent<Transform> OnPlayerEnter;
    public UnityEvent<Transform> OnPlayerExit;

    public UnityEvent OnAnyPlayerEnter;
    public UnityEvent OnAnyPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            AnyPlayerEnter();

            if(player != Player.localPlayerInstance)
                return;

            Debug.Log($"on player enter {this.name} || {player.localPlayer}");
            OnPlayerEnter?.Invoke(player.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            AnyPlayerExit();

            if (player != Player.localPlayerInstance)
                return;

            Debug.Log($"on player exit {this.name} || {player.localPlayer}");
            OnPlayerExit?.Invoke(player.transform);
        }
    }

   
    public void AnyPlayerEnter()
    {
        OnAnyPlayerEnter?.Invoke();
    }

   
    public void AnyPlayerExit()
    {
        OnAnyPlayerExit?.Invoke();
    }
}
