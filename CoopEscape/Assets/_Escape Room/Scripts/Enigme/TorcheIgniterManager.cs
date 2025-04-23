
using System.Collections.Generic;
using PurrNet;
using UnityEngine;
using UnityEngine.Events;

public class TorcheIgniterManager : MonoBehaviour
{
    [SerializeField] private List<Ignitable> orderedIgnitables = new  List<Ignitable> ();
    [SerializeField] private List<MeshRenderer> orderedColorIndicators = new List<MeshRenderer> ();
    [SerializeField] private Material wrongColorIndicatorMaterial, goodColorIndicatorMaterial, defaultColorIndicatorMaterial;
    [SerializeField] private List<Transform> orderedClockIndicators = new List<Transform>();
    [SerializeField] private Transform needle;

    public UnityEvent OnIgniteAllInGoodOrder;

    private int currentIgnitableIndex;
    private int currentColorIndex;

    private void Start()
    {
        orderedIgnitables.ForEach(ignitable =>
        { ignitable.OnStartIgnite += Ignitable_OnStartIgnite;
            ignitable.OnUnignite += Ignitable_OnUnignite;
        });
        ResetAll();
    }

    private void OnDisable()
    {
        orderedIgnitables.ForEach(ignitable =>
        {
            ignitable.OnStartIgnite -= Ignitable_OnStartIgnite;
            ignitable.OnUnignite -= Ignitable_OnUnignite;
        });
    }
    private void Ignitable_OnUnignite() => ResetAll();

    private void Ignitable_OnStartIgnite(Ignitable ignitable)
    {
        if(ignitable == orderedIgnitables[currentIgnitableIndex])
        {
            SwitchColorIndicatorMaterial(orderedColorIndicators[currentColorIndex], goodColorIndicatorMaterial);
            currentIgnitableIndex++;
            currentColorIndex++;

            if (currentIgnitableIndex >= orderedIgnitables.Count)
            {
                OnIgniteAllInGoodOrder?.Invoke();
                Debug.Log("On Complete ignite torches");
            }
        }
        else
        {
            SwitchColorIndicatorMaterial(orderedColorIndicators[currentColorIndex], wrongColorIndicatorMaterial);
            currentColorIndex++;
        }

        if (currentColorIndex >= orderedIgnitables.Count)
            return; 

        UpdateNeedleRotation(currentColorIndex);
    }


    [ContextMenu("Reset All")]
    public void ResetAll()
    {
        orderedIgnitables.ForEach(ignitable => ignitable.Unignite());
        orderedColorIndicators.ForEach(colorIndicator => SwitchColorIndicatorMaterial(colorIndicator, defaultColorIndicatorMaterial));
        currentIgnitableIndex = 0;
        currentColorIndex = 0;
        UpdateNeedleRotation(currentColorIndex);
    }

    private void SwitchColorIndicatorMaterial(MeshRenderer meshRenderer, Material targetMaterial)
    {
        meshRenderer.sharedMaterial = targetMaterial;
    }

    private void UpdateNeedleRotation(int index)
    {
        needle.rotation = Quaternion.FromToRotation(-transform.up, orderedClockIndicators[index].up);
    }
}
