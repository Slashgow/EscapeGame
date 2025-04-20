using System.Collections;
using TMPro;
using UnityEngine;

public class WaterForbidden : MonoBehaviour
{
    [SerializeField] private Transform resetPoint;

    [SerializeField, Range(0f, 5f)] private float textDuration;
    [SerializeField] private TextMeshProUGUI warningText;

    public void DoActionWaterForbidden(Transform player)
    {
        ResetTransform(player);
        StartCoroutine(ShowWarningTextCoroutine());
    }

    public void ResetTransform(Transform player)
    {
        Player.localPlayerInstance.GetComponent<FirstPersonController>().MovePlayerTo(resetPoint);
    }

    private IEnumerator ShowWarningTextCoroutine()
    {
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        warningText.gameObject.SetActive(false);
    }
}
