using System;
using UnityEngine;

public struct OccupyingCell 
{
    public readonly Vector3Int Position;


    public readonly Neighbors3<bool> SettedNeighbours;

    public OccupyingCell (Vector3Int position, Neighbors3<bool> settedNeighbours)
    {
        Position = position;
        SettedNeighbours = settedNeighbours;
    }
    
}
