using System;
using System.Collections.Generic;
using UnityEngine;
public class BuildingPointer : MonoBehaviour
{
    public Direction3 Direction { get; private set; }
    
    public void Initialize(Direction3 direction)
    {
        Direction = direction;
    }
}
