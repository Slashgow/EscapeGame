using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Talkie : MonoBehaviour
{
    [SerializeField] private Transform restTransform;
    [SerializeField] private Transform talkingTransform;
    [SerializeField] private AudioSource talkieSFXAudioSource;
    [SerializeField, Range(0f, 3f)] private float timeToSwitchPosition;
    [SerializeField] private Ease easing;
    public AudioSource TalkieSFXAudioSource => talkieSFXAudioSource;

    public UnityEvent ShowTalkie;
    public UnityEvent HideTalkie;

    private float timeElapsed = 0.0f;
    private Tween translateTween;

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
        if(translateTween != null)
            translateTween.Kill();

        translateTween = this.transform.DOLocalMove(restTransform.localPosition, timeToSwitchPosition).SetEase(easing);

        //this.transform.localPosition = restTransform.localPosition;
        //this.transform.localRotation = restTransform.localRotation;
    }

    [ContextMenu("Move to talk position")]
    private void LocalPlayerVoiceChat_OnStartPushToTalk()
    {
        if (translateTween != null)
            translateTween.Kill();

        translateTween = this.transform.DOLocalMove(talkingTransform.localPosition, timeToSwitchPosition).SetEase(easing);

        //this.transform.localPosition = talkingTransform.localPosition;
        //this.transform.localRotation = talkingTransform.localRotation;
        ShowTalkie?.Invoke();
    }

}
