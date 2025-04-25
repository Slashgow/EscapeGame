using System;
using PurrNet;
using Steamworks;
using UnityEngine;

public class PlayerVoiceChat : NetworkBehaviour
{
    public static PlayerVoiceChat localPlayerVoiceChat;

    [SerializeField]
    private PlayerInputHandler playerInputHandler;

    [SerializeField, Range(0f,20f)]
    private float distanceToRecordProximityChat = 10f;

    [SerializeField]
    private AudioSource playerAudioSource;

    [SerializeField]
    private AudioLowPassFilter lowPassFilter;

    [SerializeField]
    private AudioHighPassFilter highPassFilter;

    private Talkie talkie;

    public event Action OnStartPushToTalk;
    public event Action OnEndPushToTalk;

    private bool isPushToTalkRecording;
    private bool isProximityChatRecording;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner;

        talkie = FindAnyObjectByType<Talkie>();

        if (isOwner)
        {
            localPlayerVoiceChat = this;
            talkie.RegisterEvent();
        }
            
    }

    private void Update()
    {
        if (!isPushToTalkRecording && playerInputHandler.PushToTalkTriggered)
        {
            SteamUser.StartVoiceRecording();
            ToggleReceiverAudioFilters(PlayerIDHelper.Instance.GetOtherPlayerID(owner), true);
            Debug.Log("Start Recording");
            isPushToTalkRecording = true;
            StartRecording();
            OnStartPushToTalk?.Invoke();
        }
        else if(isPushToTalkRecording && !playerInputHandler.PushToTalkTriggered)
        {
            if(!isProximityChatRecording)
                SteamUser.StopVoiceRecording();
            Debug.Log("Stop Recording");
            ToggleReceiverAudioFilters(PlayerIDHelper.Instance.GetOtherPlayerID(owner), false);
            isPushToTalkRecording = false;
            StopRecording();
            OnEndPushToTalk?.Invoke();
        }
        else if (isPushToTalkRecording)
        {
            HandleVoiceRecording(false);
            return;
        }

        else if (!isProximityChatRecording && PlayerIDHelper.Instance.DistanceBetweenPlayer() < distanceToRecordProximityChat)
        {
            //Debug.Log("min distance OK");
            isProximityChatRecording = true;
            ToggleSenderAudioFilters(false);
            SteamUser.StartVoiceRecording();
        }
        else if (isProximityChatRecording && PlayerIDHelper.Instance.DistanceBetweenPlayer() >= distanceToRecordProximityChat)
        {
            isProximityChatRecording = false;
            ToggleSenderAudioFilters(true);
            SteamUser.StopVoiceRecording();
        }
        else if (isProximityChatRecording)
        {
            HandleVoiceRecording(true);
        }
    }

    private void HandleVoiceRecording(bool writeOnSenderAudioSource)
    {
        EVoiceResult voiceResult = SteamUser.GetAvailableVoice(out uint compressed);
        //Debug.Log($"voice result : {voiceResult.ToString()}");  
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
        //Debug.Log($"Send voice to other player server : {info.sender}");
        SendVoiceToOtherPlayer_Target((PlayerID)target, writeOnSenderAudioSource, data, bytesWritten);
    }

    [TargetRpc]
    private void SendVoiceToOtherPlayer_Target(PlayerID target, bool writeOnSenderAudioSource, byte[] data, uint bytesWritten, RPCInfo info = default)
    {
       // Debug.Log($"send voice to other player target {target}");
       // Debug.Log($"send voice to other player target, Sender: {info.sender}");

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

    [ObserversRpc]
    public void StartRecording()
    {
        Debug.Log("Start record - play talkie sound");
        talkie.TalkieSFXAudioSource.Play();
    }

    [ObserversRpc]
    public void StopRecording()
    {
        Debug.Log("Stop record - play talkie sound");
        talkie.TalkieSFXAudioSource.Play();
    }

    [ObserversRpc]
    private void ToggleSenderAudioFilters(bool enable)
    {
        Debug.Log($"toggle audio filters to {enable}");
        lowPassFilter.enabled = enable;
        highPassFilter.enabled = enable;
    }

    [ObserversRpc]
    private void ToggleReceiverAudioFilters(PlayerID? playerID, bool enable)
    {
        PlayerIDHelper.Instance.GetOtherPlayerHighPassFilter(playerID).enabled = enable;
        PlayerIDHelper.Instance.GetOtherPlayerLowPassFilter(playerID).enabled = enable;
    }
}
