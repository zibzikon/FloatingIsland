using System;
using System.Collections.Generic;
using UnityEngine;
public class BuildPointer : MonoBehaviour
{
    [SerializeField] private DirectionRotation _directionRotation;
    public Direction3 Direction { get; private set; }
    
    public void Initialize(Direction3 direction)
    {
        Direction = direction;
        transform.rotation = Quaternion.Euler(_directionRotation.GeRotationByDirection(direction));
    }
}
