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
        return direction switch
        {
            Direction3.Right => Vector3.right,
            Direction3.Left => Vector3.left,
            Direction3.Foward => Vector3.forward,
            Direction3.Back => Vector3.back,
            Direction3.Up => Vector3.up,
            Direction3.Down => Vector3.down,
            _=> throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static Direction3 GetOppositeDirection(this Direction3 direction3)
    {
        return direction3 switch
        {
            Direction3.Back => Direction3.Foward,
            Direction3.Foward => Direction3.Back,
            Direction3.Up => Direction3.Down,
            Direction3.Down => Direction3.Up,
            Direction3.Right => Direction3.Left,
            Direction3.Left => Direction3.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction3), direction3, null)
        };
    }

    public static Direction3 AsDirection3(this Direction2 direction2)
    {
        return direction2 switch
        {
            Direction2.Back => Direction3.Foward,
            Direction2.Foward => Direction3.Back,
            Direction2.Right => Direction3.Left,
            Direction2.Left => Direction3.Right,
            Direction2.Zero => Direction3.Zero,
            _ => throw new ArgumentOutOfRangeException(nameof(direction2), direction2, null)
        };
    }
    
    public static Direction2 AsDirection2(this Direction3 direction3)
    {
        return direction3 switch
        {
            Direction3.Back => Direction2.Foward,
            Direction3.Foward => Direction2.Back,
            Direction3.Right => Direction2.Left,
            Direction3.Left => Direction2.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction3), direction3, null)
        };
    }
}

