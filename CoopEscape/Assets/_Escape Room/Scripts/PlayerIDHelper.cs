using PurrNet;
using UnityEngine;

public class PlayerIDHelper : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("player id controller awake");
        NetworkManager.main.onPlayerJoined += Main_onPlayerJoined;
    }

    private void OnDestroy()
    {
        NetworkManager.main.onPlayerJoined -= Main_onPlayerJoined;
    }

    private void Main_onPlayerJoined(PlayerID player, bool isReconnect, bool asServer)
    {
        Debug.Log("player joined");
    }
}
