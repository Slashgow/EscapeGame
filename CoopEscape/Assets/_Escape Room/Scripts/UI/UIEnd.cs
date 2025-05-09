using System;
using System.Collections;
using DG.Tweening;
using PurrNet;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEnd : MonoBehaviour
{
    [SerializeField] private Image endBackground;
    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField, Range(0f, 10f)] private float transitionTime;
    [PurrScene, SerializeField] private string nextScene;
    [SerializeField, Range(0f, 5f)] private float timeBeforeSwitchScene;

    private void Awake()
    {
        var color = endBackground.color;
        color.a = 0f;
        endBackground.color = color;
        endText.gameObject.SetActive(false);
        endBackground.gameObject.SetActive(false);
    }

    public void ShowEndUI()
    {
        endBackground.gameObject.SetActive(true);
        endBackground.DOFade(1f, transitionTime).OnComplete(() => endText.gameObject.SetActive(true));
        StartCoroutine(SwitchSceneCoroutine());
    }

    private IEnumerator SwitchSceneCoroutine()
    {
        yield return new WaitForSeconds(timeBeforeSwitchScene);
        SceneManager.LoadSceneAsync(nextScene);
    }
}
