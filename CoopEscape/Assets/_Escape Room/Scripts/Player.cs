using System;
using PurrNet;
using UnityEngine;

[Serializable]
public class  PlayerReference
{
    private GameObject player;
    private PlayerID? playerID;

    public PlayerReference(GameObject player, PlayerID? playerID)
    {
        this.player = player;
        this.playerID = playerID;
    }

    public GameObject Player => player;
    public PlayerID? PlayerID => playerID;
}

public class Player : NetworkBehaviour
{
    protected override void OnSpawned()
    {
        base.OnSpawned();


        PlayerIDHelper.Instance.AddPlayerReference(new PlayerReference(this.gameObject, owner));
    }
}
