using UnityEngine;

public class SoundOnButtonClick : MonoBehaviour
{
    [SerializeField] private InteractableButton interactableButton;
    public InteractableButton InteractableButton => interactableButton;

    [SerializeField] private AudioClip audioClip;
    public AudioClip AudioClip => audioClip;

    [SerializeField] private AudioSource audioSource;

    private void OnEnable() => interactableButton.OnClick.AddListener(OnClickButton);
    private void OnDisable() => interactableButton.OnClick.RemoveListener(OnClickButton);

    private void OnClickButton()
    {
        if(audioSource.clip != audioClip)
            audioSource.clip = audioClip;
        
        audioSource.Play();
    }
}
