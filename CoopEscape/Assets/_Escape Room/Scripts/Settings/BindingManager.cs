using UnityEngine;

public class BindingManager : PersistentMonoSingleton<BindingManager>
{
    [SerializeField] private bool holdToTalk = false;
    public bool HoldToTalk => holdToTalk;

    public void SetHoldToTalk(bool enableHoldToTalk)
    {
        holdToTalk = enableHoldToTalk;
    }

}
