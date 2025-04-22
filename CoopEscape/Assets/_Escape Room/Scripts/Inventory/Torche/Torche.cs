using UnityEngine;

public class Torche : Item
{
    [SerializeField, Range(0f,10f)] private float interactionDistance = 4;
    [SerializeField] private LayerMask ignitableLayerMask;
    
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public override void Use()
    {
        base.Use();

        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit raycastHit, interactionDistance, ignitableLayerMask))
            return;

        var ignitables = raycastHit.collider.GetComponents<IIgnitable>();
        foreach (var ignitable in ignitables)
        {
            ignitable.Ignite();
        }
    }
}
