
using System;
using UnityEngine;

public interface IMovable : IUpdatable
{
    public bool TargetWasReached { get; } 
    public bool IsMoving { get; }
    
    public void MoveTo(ITarget target);
}
