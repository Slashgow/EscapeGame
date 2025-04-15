using System;
using PurrNet;
using Steamworks;
using UnityEngine;

public class PlayerVoiceChat : NetworkBehaviour
{
    [SerializeField]
    private PlayerInputHandler playerInputHandler;

    private bool isPushToTalkRecording;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner;
    }

    private void Update()
    {
        if (!isPushToTalkRecording && playerInputHandler.PushToTalkTriggered)
        {
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

    [ObserversRpc]
    private void SendVoiceToOtherPlayer(uint bytesWritten)
    {


        Debug.Log("Receiving voice");
    }
}
