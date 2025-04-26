using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequencedSoundManager : MonoBehaviour
{
    [SerializeField] private List<SoundOnButtonClick> orderedSoundOnButtonClicks = new List<SoundOnButtonClick>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField, Range(0f, 3f)] private float timeBetweenToggle;

    public UnityEvent OnTriggerAllInGoodOrder;

    private int index;
    private int currentInteractableIndex;
    private Coroutine coroutine;

    private void Start()
    {
        orderedSoundOnButtonClicks.ForEach(orderedSoundOnButtonClick => orderedSoundOnButtonClick.InteractableButton.OnClickInteractable += OnClickInteractableButton);
    }
    private void OnDisable()
    {
        orderedSoundOnButtonClicks.ForEach(orderedSoundOnButtonClick => orderedSoundOnButtonClick.InteractableButton.OnClickInteractable -= OnClickInteractableButton);
    }

    private void OnClickInteractableButton(InteractableButton interactableButton)
    {
        if(interactableButton == orderedSoundOnButtonClicks[currentInteractableIndex].InteractableButton)
        {
            currentInteractableIndex++;
            if (currentInteractableIndex >= orderedSoundOnButtonClicks.Count)
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

        while (index < orderedSoundOnButtonClicks.Count)
        {
            audioSource.clip = orderedSoundOnButtonClicks[index].AudioClip;
            audioSource.Play();
            index++;
            yield return new WaitForSeconds(timeBetweenToggle + audioSource.clip.length);
        }

        yield return null;
    }
}
