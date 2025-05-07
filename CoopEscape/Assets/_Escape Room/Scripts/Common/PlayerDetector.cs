
using System.Collections.Generic;
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

    private List<PlayerID?> playerIDs = new List<PlayerID?>();

    private List<FirstPersonController> firstPersonControllers = new List<FirstPersonController>();
    public List<FirstPersonController> FirstPersonControllers => firstPersonControllers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (!playerIDs.Contains(player.PlayerID))
            {
                playerIDs.Add(player.PlayerID);
                firstPersonControllers.Add(player.GetComponent<FirstPersonController>());
                PlayerCount++;
            }

            AnyPlayerEnter();

            if(player != Player.localPlayerInstance)
                return;

            //Debug.Log($"on player enter {this.name} || {player.localPlayer}");
            OnPlayerEnter?.Invoke(player.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (playerIDs.Contains(player.PlayerID))
            {
                playerIDs.Remove(player.PlayerID);
                firstPersonControllers.Remove(player.GetComponent<FirstPersonController>());
                PlayerCount--;
            }
           

            if (PlayerCount <= 0)
                OnLastPlayerExit?.Invoke();

            AnyPlayerExit();

            if (player != Player.localPlayerInstance)
                return;

            //Debug.Log($"on player exit {this.name} || {player.localPlayer}");
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
