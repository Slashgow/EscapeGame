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

    public static Player localPlayerInstance;

    private PlayerReference playerReference;
    protected override void OnSpawned()
    {
        base.OnSpawned();

        if (isOwner)
            localPlayerInstance = this;

        playerReference = new PlayerReference(this.gameObject, owner);
        PlayerIDHelper.Instance.AddPlayerReference(playerReference);
    }


    protected override void OnDespawned()
    {
        base.OnDespawned();

        if(isOwner)
            localPlayerInstance = null;

        PlayerIDHelper.Instance.RemovePlayerReference(playerReference);
        playerReference = null;
    }


}
