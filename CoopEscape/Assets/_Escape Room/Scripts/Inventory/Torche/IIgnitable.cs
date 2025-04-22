using System;
using UnityEngine;

public interface IIgnitable 
{
    public bool IsIgnited { get; set; }
    public event Action<Ignitable> OnStartIgnite;
    public void Ignite();
    public void Unignite();
}
