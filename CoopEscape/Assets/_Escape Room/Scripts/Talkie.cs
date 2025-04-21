using UnityEngine;
using UnityEngine.Events;

public class Talkie : MonoBehaviour
{
    [SerializeField] private Transform restTransform;
    [SerializeField] private Transform talkingTransform;

    public UnityEvent ShowTalkie;
    public UnityEvent HideTalkie;

    public void RegisterEvent()
    {
        PlayerVoiceChat.localPlayerVoiceChat.OnStartPushToTalk += LocalPlayerVoiceChat_OnStartPushToTalk;
        PlayerVoiceChat.localPlayerVoiceChat.OnEndPushToTalk += LocalPlayerVoiceChat_OnEndPushToTalk; 

        MoveToRestPosition();
    }

    private void OnDisable()
    {
        PlayerVoiceChat.localPlayerVoiceChat.OnStartPushToTalk -= LocalPlayerVoiceChat_OnStartPushToTalk;
        PlayerVoiceChat.localPlayerVoiceChat.OnEndPushToTalk -= LocalPlayerVoiceChat_OnEndPushToTalk;
    }
    private void LocalPlayerVoiceChat_OnEndPushToTalk()
    {
        MoveToRestPosition();
        HideTalkie?.Invoke();
    }

    [ContextMenu("Move to rest position")]
    private void MoveToRestPosition()
    {
        this.transform.localPosition = restTransform.localPosition;
        this.transform.localRotation = restTransform.localRotation;
    }

    [ContextMenu("Move to talk position")]
    private void LocalPlayerVoiceChat_OnStartPushToTalk()
    {
        this.transform.localPosition = talkingTransform.localPosition;
        this.transform.localRotation = talkingTransform.localRotation;
        ShowTalkie?.Invoke();
    }
}
