using System;
using UnityEngine;

[Serializable]
public struct OccupyingCell 
{
    [SerializeField] private Vector3Int _position;
    public Vector3Int Position => _position;

    [SerializeField] private bool _right;
    [SerializeField] private bool _left;
    [SerializeField] private bool _forward;
    [SerializeField] private bool _back;
    [SerializeField] private bool _up;
    [SerializeField] private bool _down;

    private Neighbors3<bool> _settedNeighbours => GetNeighbours();
    
    public Neighbors3<bool> SettedNeighbours => _settedNeighbours;

    public static OccupyingCell Create(Vector3Int position, Neighbors3<bool> settedNeighbours)
    {
        return new OccupyingCell()
        {
            _position = position,
            _right = settedNeighbours.Right,
            _left = settedNeighbours.Left,
            _back = settedNeighbours.Back,
            _forward = settedNeighbours.Forward,
            _up = settedNeighbours.Up,
            _down = settedNeighbours.Down,
        };
    }
    
    private Neighbors3<bool> GetNeighbours()
    {
        var neighbour = new Neighbors3<bool>()
        {
            Right = _right,
            Left = _left,
            Forward = _forward,
            Back = _back,
            Up = _up,
            Down = _down
        };
        return neighbour;
    }
}
