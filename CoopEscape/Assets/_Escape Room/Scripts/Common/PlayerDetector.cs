using PurrNet;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    public int PlayerCount {  get; private set; }

    public UnityEvent<Transform> OnPlayerEnter;
    public UnityEvent<Transform> OnPlayerExit;

    public UnityEvent OnAnyPlayerEnter;
    public UnityEvent OnAnyPlayerExit;

    public UnityEvent OnLastPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            PlayerCount++;
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
            PlayerCount--;

            if (PlayerCount <= 0)
                OnLastPlayerExit?.Invoke();

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
