using System;
using System.Collections;
using PurrNet;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerInventory : NetworkBehaviour
{
    public static PlayerInventory localInventory;

    [SerializeField] private Transform itemAttachPoint;
    [SerializeField] private Transform armTarget;
    [SerializeField] private Rig rig;
    [SerializeField] private float transitionTime;

    private Coroutine coroutine;

    private Item itemInHand;
    public Item ItemInHand => itemInHand;
    protected override void OnSpawned()
    {
        base.OnSpawned();

        if(!isOwner)
            return;

        localInventory = this;
    }

    protected override void OnDespawned()
    {
        base.OnDespawned();

        if(!isOwner )
            return;

        localInventory = null;
    }

    private void Update()
    {
        if (SceneInputHandler.Instance.IsUseInteractablePressed)
            UseItem();
    }

    private void UseItem()
    {
        if (!itemInHand)
            return;

        itemInHand.Use();
    }

    public void EquipItem(Item item)
    {
        if (!item)
            return;

        itemInHand = Instantiate(item, itemAttachPoint.position, itemAttachPoint.rotation, itemAttachPoint);
        itemInHand.SetHoldStatuts(true);
        
        //itemInHand.transform.localRotation =Quaternion.Inverse(item.AttachPoint.localRotation);
        itemInHand.transform.localPosition = -item.AttachPoint.localPosition;
        armTarget.transform.position = itemAttachPoint.position;
        armTarget.transform.rotation = itemAttachPoint.rotation;

        UpdateRigWeight(0f, 1f);

        itemInHand.SetKinematic(true);
        Debug.Log($"equip item {item.ItemName} ");
    }

    public void UnequipedItem(Item item)
    {
        if (!item)
            return;

        if (!itemInHand)
            return;

        if (itemInHand.ItemName != item.ItemName)
            return;

        itemInHand.SetHoldStatuts(false);
        Destroy(itemInHand.gameObject);
        itemInHand = null;

        UpdateRigWeight(1f,0f);
        Debug.Log($"unequip item {item.ItemName} ");
    }


    public bool IsHoldingItem(Item item)
    {
        if(!itemInHand) 
            return false;

        return item = itemInHand;
    }

    [ObserversRpc]
    private void UpdateRigWeight(float start, float end)
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
