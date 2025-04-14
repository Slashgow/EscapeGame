using PurrLobby;
using PurrNet.Steam;
using UnityEngine;

public class SteamPeerToPeerInitializer : MonoBehaviour
{
    [SerializeField]
    private SteamTransport steamTransport;

    private LobbyDataHolder lobbyDataHolder;

    private void Awake()
    {
        lobbyDataHolder = FindAnyObjectByType<LobbyDataHolder>();

        steamTransport.address = lobbyDataHolder.CurrentLobby.Members[0].Id;
    }
}
