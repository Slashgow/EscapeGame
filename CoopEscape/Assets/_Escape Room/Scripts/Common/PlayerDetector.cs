using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    public UnityEvent<Transform> OnPlayerEnter;
    public UnityEvent<Transform> OnPlayerExit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
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
            if (player != Player.localPlayerInstance)
                return;

            Debug.Log($"on player exit {this.name} || {player.localPlayer}");
            OnPlayerExit?.Invoke(player.transform);
        }
    }
}
