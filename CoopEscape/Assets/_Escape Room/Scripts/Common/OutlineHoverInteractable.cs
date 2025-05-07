using PurrNet;
using QuickOutline;
using UnityEngine;

[RequireComponent(typeof(Outline))]

public class OutlineHoverInteractable : MonoBehaviour
{
    [SerializeField] private AInteractable interactable;
    private Outline outline;
    private void OnEnable()
    {
        outline = GetComponent<Outline>();
        //interactable = GetComponent<AInteractable>();
       
        //Debug.Log($"subscribe to event outline {interactable.name}");
        interactable.OnHoverStart += Interactable_OnHoverStart;
        interactable.OnHoverStop += Interactable_OnHoverStop; 
        outline.enabled = false;
    }

    private void OnDisable()
    {
        if(interactable == null)
            return;

        //Debug.Log($"unsubscribe to event outline {interactable.name}");
        interactable.OnHoverStart -= Interactable_OnHoverStart;
        interactable.OnHoverStop -= Interactable_OnHoverStop;
    }

    private void Interactable_OnHoverStop()
    {
        //Debug.Log($"hover stop, disable outline {interactable.name}");
        outline.enabled = false;
    }

    private void Interactable_OnHoverStart()
    {
        //Debug.Log($"hover start, enable outline {interactable.name}");
        outline.enabled = true;
    }
}
