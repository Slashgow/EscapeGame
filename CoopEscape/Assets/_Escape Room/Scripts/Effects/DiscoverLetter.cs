using System.Collections;
using TMPro;
using UnityEngine;

public class DiscoverLetter : MonoBehaviour
{
    [SerializeField] private string word = "Recording";
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField, Range(0f,1f)] private float timeBetweenLetter;
    [SerializeField, Range(0f,10f)] private float timeBetweenEffect;

    private Coroutine coroutine;
    public void StartDiscoverLetterEffect()
    {
        coroutine = StartCoroutine(DiscoverLetterCoroutine());
    }
    public void StopDiscoverLetterEffect()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
    private IEnumerator DiscoverLetterCoroutine()
    {
        text.text = string.Empty;
        while (true)
        {
            for (int i = 0; i < word.Length; i++)
            {
                text.text += word[i];
                yield return new WaitForSeconds(timeBetweenLetter);
            }
            yield return new WaitForSeconds(timeBetweenEffect);
            text.text = string.Empty;
        }
    }
}
