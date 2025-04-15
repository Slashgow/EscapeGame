
using System.Collections.Generic;
using System.Linq;
using PurrNet;
using UnityEngine;

public class PlayerIDHelper : MonoSingleton<PlayerIDHelper>
{
    private PlayerID? otherPlayerID;
    private GameObject otherPlayerGameObject;
    private AudioSource otherPlayerAudioSource;

    private  List<PlayerReference> players = new List<PlayerReference>();

    public void AddPlayerReference(PlayerReference playerReference) => players.Add(playerReference);

    public PlayerID? GetOtherPlayerID(PlayerID? callerPlayerID)
    {
        if(otherPlayerID == null)
        {
            otherPlayerID = players.First(playerReference => playerReference.PlayerID != callerPlayerID).PlayerID;
            return otherPlayerID;
        }
        return otherPlayerID;
       
    }
    public GameObject GetOtherPlayerGameObject(PlayerID? callerPlayerID)
    {
        if(otherPlayerAudioSource == null)
        {
            otherPlayerGameObject = players.First(playerReference => playerReference.PlayerID != callerPlayerID).Player;
            return otherPlayerGameObject;
        }
        return otherPlayerGameObject;
    }

    public AudioSource GetOtherPlayerAudioSource(PlayerID? callerPlayerID)
    {
        if (otherPlayerAudioSource == null)
        {
            otherPlayerAudioSource = players.First(playerReference => playerReference.PlayerID != callerPlayerID).Player.GetComponent<AudioSource>();
            return otherPlayerAudioSource;
        }
        return otherPlayerAudioSource;
    }
}
