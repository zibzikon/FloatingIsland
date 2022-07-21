using System;
using UnityEngine;

[Serializable]
public struct DirectionRotation
{
    [SerializeField] private Vector3 _right;
    [SerializeField] private Vector3 _left;
    [SerializeField] private Vector3 _forward;
    [SerializeField] private Vector3 _back;
    [SerializeField] private Vector3 _up;
    [SerializeField] private Vector3 _down;

    public Vector3 GeRotationByDirection(Direction3 direction)
    {
        return direction switch
        {
            Direction3.Back => _back,
            Direction3.Right => _right,
            Direction3.Left => _left,
            Direction3.Foward => _forward,
            Direction3.Up => _up,
            Direction3.Down => _down,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}
