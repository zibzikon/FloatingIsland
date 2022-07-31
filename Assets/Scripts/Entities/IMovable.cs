
using System;
using UnityEngine;

public interface IMovable : IUpdatable
{
    public bool TargetWasReached { get; } 
    public bool IsMoving { get; }
    
    public void SetTarget(ITarget target);
    
    public void MoveTo(Vector3 position);

    public void Reset();
}
