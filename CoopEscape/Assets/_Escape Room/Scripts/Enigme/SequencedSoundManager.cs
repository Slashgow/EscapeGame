using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequencedSoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> orderedAudioClips = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField, Range(0f, 3f)] private float timeBetweenToggle;
    [SerializeField] private List<InteractableButton> orderedInteractableButtons = new List<InteractableButton>();

    public UnityEvent OnTriggerAllInGoodOrder;

    private int index;
    private int currentInteractableIndex;
    private Coroutine coroutine;

    private void Start()
    {
        orderedInteractableButtons.ForEach(button => button.OnClickInteractable += OnClickInteractableButton);
    }
    private void OnDisable()
    {
        orderedInteractableButtons.ForEach(button => button.OnClickInteractable -= OnClickInteractableButton);
    }

    private void OnClickInteractableButton(InteractableButton interactableButton)
    {
        if(interactableButton == orderedInteractableButtons[currentInteractableIndex])
        {
            currentInteractableIndex++;
            if (currentInteractableIndex >= orderedInteractableButtons.Count)
            {
                OnTriggerAllInGoodOrder?.Invoke();
                Debug.Log("On Complete sequenced sound");
            }
        }
        else
        {
            ResetAll();
        }
    }

    public void ResetAll() => currentInteractableIndex = 0;

    public void PlaySequencedSound()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        coroutine = StartCoroutine(PlaySequentiallyCoroutine());
    }

    private IEnumerator PlaySequentiallyCoroutine()
    {
        index = 0;

        while (index < orderedAudioClips.Count)
        {
            audioSource.clip = orderedAudioClips[index];
            audioSource.Play();
            index++;
            yield return new WaitForSeconds(timeBetweenToggle + audioSource.clip.length);
        }

        yield return null;
    }
}
