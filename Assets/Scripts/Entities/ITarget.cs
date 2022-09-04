using System;
using Enums;
using UnityEngine;

public interface ITarget : IDamagable
{
    public event Action PositionChanged;
    
    public TargetType TargetType { get; }
    public FloatingIslandTransform Transform { get; }
}
