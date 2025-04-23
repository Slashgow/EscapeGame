using PurrNet;
using UnityEngine;

public abstract class Pin : NetworkBehaviour
{
    public abstract bool IsUnlocked { get; }
}
