using System.Linq;
using PurrNet;
using Steamworks;
using UnityEngine;

public class PlayerVoiceChat : NetworkBehaviour
{
    [SerializeField]
    private PlayerInputHandler playerInputHandler;

    private bool isPushToTalkRecording;

    private PlayerID otherPlayerID;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner;
    }

    private void Update()
    {
        if (!isPushToTalkRecording && playerInputHandler.PushToTalkTriggered)
        {
            Debug.Log(otherPlayerID.ToString());
            if (otherPlayerID == PlayerID.Server)
            {
                otherPlayerID = networkManager.players.FirstOrDefault(playerId => playerId != this.localPlayer);
                Debug.Log(otherPlayerID.ToString());
                if (otherPlayerID ==  PlayerID.Server)
                    return;
            }
               

            SteamUser.StartVoiceRecording();
            Debug.Log("Start Recording");
            isPushToTalkRecording = true;
        }
        else if(isPushToTalkRecording && !playerInputHandler.PushToTalkTriggered)
        {
            SteamUser.StopVoiceRecording();
            Debug.Log("Stop Recording");
            isPushToTalkRecording = false;
        }
        else if (isPushToTalkRecording)
        {
            EVoiceResult voiceResult = SteamUser.GetAvailableVoice(out uint compressed);
            if(voiceResult == EVoiceResult.k_EVoiceResultOK && compressed > 1024)
            {
                Debug.Log(compressed);
                byte[] destBuffer = new byte[1024];
                voiceResult = SteamUser.GetVoice(true, destBuffer, 1024, out uint bytesWritten);
                if(voiceResult == EVoiceResult.k_EVoiceResultOK &&  bytesWritten > 0)
                {
                    SendVoiceToOtherPlayer(bytesWritten);
                }
            }
        }
    }

    [ServerRpc]
    private void SendVoiceToOtherPlayer(uint bytesWritten, RPCInfo info = default)
    {
        Debug.Log($"Send voice to other player server : {info.sender}");
        SendVoiceToOtherPlayer_Target(otherPlayerID, bytesWritten);
    }

    [TargetRpc]
    private void SendVoiceToOtherPlayer_Target(PlayerID target, uint bytesWritten, RPCInfo info = default)
    {
        Debug.Log($"send voice to other player target {target}");
        Debug.Log($"send voice to other player target, Sender: {info.sender}");
    }
}
