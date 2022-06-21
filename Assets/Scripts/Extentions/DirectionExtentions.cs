using System;
using System.Collections.Generic;
using UnityEngine;
public static class DirectionExtentions
{
    public static IEnumerable<Direction3> GetDirectionEnumerable()
    {
        return new[] { Direction3.Right, Direction3.Left, Direction3.Foward, Direction3.Back, Direction3.Up, Direction3.Down };
    }
    public static Vector3 ToVector3( this Direction3 direction)
    {
        switch (direction)
        {
            case Direction3.Zero:
                throw new NullReferenceException("Direction cannot be zero");

            case Direction3.Right: return Vector3.right;
            case Direction3.Left: return Vector3.left;
            case Direction3.Foward: return Vector3.forward;
            case Direction3.Back: return Vector3.back;
            case Direction3.Up: return Vector3.up;
            default: return Vector3.down;
        }
    }
    
   
}

