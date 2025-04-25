using System.Collections;
using PurrNet;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigUpdater : NetworkBehaviour
{
    [SerializeField] private Transform armTarget;
    [SerializeField] private Rig rig;
    [SerializeField] private float transitionTime;

    private Coroutine coroutine;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    [ObserversRpc]
    public void UpdateRigWeight(float start, float end)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine(UpdateRigWeightCoroutine(start, end));
    }

    private IEnumerator UpdateRigWeightCoroutine(float start, float end)
    {
        float elapsedTime = 0;

        while (elapsedTime < transitionTime)
        {
            rig.weight = Mathf.Lerp(start, end, (elapsedTime / transitionTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
