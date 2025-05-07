using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum LiftLevelTitle
{
    WAITING_ROOM,
    LABORATORY_ZERO,
    LABORATORY_MINUS_ONE
}

[Serializable]
public struct LiftLevel 
{
    [SerializeField] private LiftLevelTitle liftLevelTitle;
    public LiftLevelTitle LiftLevelTitle => liftLevelTitle;

    [SerializeField] private Transform liftLevelTransform;
    public Transform LiftLevelTransform => liftLevelTransform;
}

public class Ascenceur : MonoBehaviour
{
    [SerializeField] private LiftLevel startLiftLevel;

    [SerializeField] private List<LiftLevel> orderedLiftLevels = new List<LiftLevel>();

    [SerializeField, Range(0f, 10f)] private float timeToChangeLevel = 3f;
    [SerializeField] private Ease easing;
    [SerializeField] private PlayerDetector playerDetector;

    private int currentLiftLevelIndex;

    private void Awake()
    {
        currentLiftLevelIndex = 0;
    }
    public void TranslateDown()
    {
        if(orderedLiftLevels[currentLiftLevelIndex].LiftLevelTitle == LiftLevelTitle.LABORATORY_MINUS_ONE)
            return;

        currentLiftLevelIndex++;
        LockPlayer(true);
        this.transform.DOMove(orderedLiftLevels[currentLiftLevelIndex].LiftLevelTransform.position, timeToChangeLevel).SetEase(easing).
            OnComplete(() => LockPlayer(false));
    }

    public void TranslateUp()
    {
        if (orderedLiftLevels[currentLiftLevelIndex].LiftLevelTitle == LiftLevelTitle.LABORATORY_ZERO ||
            orderedLiftLevels[currentLiftLevelIndex].LiftLevelTitle == LiftLevelTitle.WAITING_ROOM)
            return;

        currentLiftLevelIndex--;
        LockPlayer(true);
        this.transform.DOMove(orderedLiftLevels[currentLiftLevelIndex].LiftLevelTransform.position, timeToChangeLevel).SetEase(easing).
            OnComplete(() => LockPlayer(false));
    }

    public void ParentPlayerToElevator(Transform playerTransform)
    {
        playerTransform.SetParent(this.transform);
    }

    public void UnParentPlayerFromElevator(Transform playerTransform)
    {
        playerTransform.SetParent(null);
    }

    public void LockPlayer(bool isJumpLocked)
    {
        playerDetector.FirstPersonControllers.ForEach(firstPersonController =>
        {
            firstPersonController.IsJumpLocked = isJumpLocked;
            firstPersonController.ToggleCharacterController(!isJumpLocked);
        });
        
    }

}
