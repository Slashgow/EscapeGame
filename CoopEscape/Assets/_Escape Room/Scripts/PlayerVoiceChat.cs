using PurrNet;
using Steamworks;
using UnityEngine;

public class PlayerVoiceChat : NetworkBehaviour
{
    [SerializeField]
    private PlayerInputHandler playerInputHandler;

    [SerializeField, Range(0f,20f)]
    private float distanceToRecordProximityChat = 10f;

    [SerializeField]
    private AudioSource playerAudioSource;


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
            HandleVoiceRecording(false);
        }
        else if (PlayerIDHelper.Instance.DistanceBetweenPlayer < distanceToRecordProximityChat)
        {
            HandleVoiceRecording(true);
        }
    }

    private void HandleVoiceRecording(bool writeOnSenderAudioSource)
    {
        EVoiceResult voiceResult = SteamUser.GetAvailableVoice(out uint compressed);
        if (voiceResult == EVoiceResult.k_EVoiceResultOK && compressed > 1024)
        {
            Debug.Log(compressed);
            byte[] destBuffer = new byte[1024];
            voiceResult = SteamUser.GetVoice(true, destBuffer, 1024, out uint bytesWritten);
            if (voiceResult == EVoiceResult.k_EVoiceResultOK && bytesWritten > 0)
            {
                SendVoiceToOtherPlayer(PlayerIDHelper.Instance.GetOtherPlayerID(owner), writeOnSenderAudioSource, destBuffer, bytesWritten);
            }
        }
    }

    [ServerRpc]
    private void SendVoiceToOtherPlayer(PlayerID? target, bool writeOnSenderAudioSource, byte[] data, uint bytesWritten, RPCInfo info = default)
    {
        Debug.Log($"Send voice to other player server : {info.sender}");
        SendVoiceToOtherPlayer_Target((PlayerID)target, writeOnSenderAudioSource, data, bytesWritten);
    }

    [TargetRpc]
    private void SendVoiceToOtherPlayer_Target(PlayerID target, bool writeOnSenderAudioSource, byte[] data, uint bytesWritten, RPCInfo info = default)
    {
        Debug.Log($"send voice to other player target {target}");
        Debug.Log($"send voice to other player target, Sender: {info.sender}");

        byte[] destBuffer2 = new byte[22050 * 2];
        uint bytesWritten2;
        EVoiceResult ret = SteamUser.DecompressVoice(data, bytesWritten, destBuffer2, (uint)destBuffer2.Length, out bytesWritten2, 22050);
        if (ret == EVoiceResult.k_EVoiceResultOK && bytesWritten2 > 0)
        {
            AudioSource audioSource = writeOnSenderAudioSource ? playerAudioSource : PlayerIDHelper.Instance.GetOtherPlayerAudioSource(owner);

            audioSource.clip = AudioClip.Create(UnityEngine.Random.Range(100, 1000000).ToString(), 22050, 1, 22050, false);

            float[] test = new float[22050];
            for (int i = 0; i < test.Length; i++)
            {
                test[i] = (short)(destBuffer2[i * 2] | destBuffer2[i * 2 + 1] << 8) / 32768.0f;
            }
            audioSource.clip.SetData(test, 0);
            audioSource.Play();
        }
    }
}
