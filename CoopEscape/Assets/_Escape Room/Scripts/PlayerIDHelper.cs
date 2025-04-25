
using System.Collections.Generic;
using System.Linq;
using PurrNet;
using UnityEngine;

public class PlayerIDHelper : MonoSingleton<PlayerIDHelper>
{
    private PlayerID? otherPlayerID;
    private GameObject otherPlayerGameObject;
    private AudioSource otherPlayerAudioSource;
    private AudioLowPassFilter otherPlayerLowPassFilter;
    private AudioHighPassFilter otherPlayerHighPassFilter;

    private  List<PlayerReference> players = new List<PlayerReference>();

    public float DistanceBetweenPlayer()
    {
        if (players.Count < 2)
            return 100f;

        return Vector3.Distance(players[0].Player.transform.position, players[1].Player.transform.position);
    }

    public void AddPlayerReference(PlayerReference playerReference) => players.Add(playerReference);
    public void RemovePlayerReference(PlayerReference playerReference) => players.Remove(playerReference);

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
    public AudioLowPassFilter GetOtherPlayerLowPassFilter(PlayerID? callerPlayerID)
    {
        if (otherPlayerLowPassFilter == null)
        {
            otherPlayerLowPassFilter = players.First(playerReference => playerReference.PlayerID != callerPlayerID).Player.GetComponent<AudioLowPassFilter>();
            return otherPlayerLowPassFilter;
        }
        return otherPlayerLowPassFilter;
    }
    public AudioHighPassFilter GetOtherPlayerHighPassFilter(PlayerID? callerPlayerID)
    {
        if (otherPlayerHighPassFilter == null)
        {
            otherPlayerHighPassFilter = players.First(playerReference => playerReference.PlayerID != callerPlayerID).Player.GetComponent<AudioHighPassFilter>();
            return otherPlayerHighPassFilter;
        }
        return otherPlayerHighPassFilter;
    }
}
