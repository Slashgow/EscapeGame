using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencedToggler : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjectsToToggle = new List<GameObject>();
    [SerializeField, Range(0f, 3f)] private float timeBetweenToggle;
    [SerializeField] private bool disableOnStart = true;

    private Coroutine coroutine;
    private int index = 0;

    private void Awake()
    {
        gameObjectsToToggle.ForEach(gameObjectToToggle => gameObjectToToggle.SetActive(!disableOnStart));
    }

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
       
        while (index < gameObjectsToToggle.Count)
        {
            gameObjectsToToggle[index].SetActive(disableOnStart);
            index++;
            yield return new WaitForSeconds(timeBetweenToggle);
        }

        yield return null;
    }
}
