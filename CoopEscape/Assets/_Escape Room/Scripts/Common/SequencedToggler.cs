using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SequencedToggler : MonoBehaviour
{
    [SerializeField] private List<Ignitable> ignitablesToToggle = new List<Ignitable>();
    [SerializeField, Range(0f, 3f)] private float timeBetweenToggle;
    [SerializeField] private bool disableOnStart = true;
    [SerializeField] private bool switchOffOnEnd = true;

    private Coroutine coroutine;
    private int index = 0;

    //private void Awake()
    //{
    //    ignitablesToToggle.ForEach(ignitable => ignitable.Flame.SetActive(!disableOnStart));
    //}

    [ContextMenu("Toggle Sequentially")]
    public void ToggleSequentially()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        StartCoroutine(ToggleSequentiallyCoroutine());
    }

    private IEnumerator ToggleSequentiallyCoroutine()
    {
        index = 0;
       
        while (index < ignitablesToToggle.Count)
        {
            ignitablesToToggle[index].Ignite();
            index++;
            yield return new WaitForSeconds(timeBetweenToggle);
        }


        if (switchOffOnEnd)
            ignitablesToToggle.ForEach(ignitable => ignitable.Unignite());

        yield return null;

      
    }
}
