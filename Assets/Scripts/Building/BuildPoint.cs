using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof( Transform))]
public class BuildPoint: MonoBehaviour
{
    [SerializeField] private List<BuildingType> _whiteList;
    public List<BuildingType> WhiteList => _whiteList;
    
    [SerializeField] private Vector3Int _startOccupedCellPosition;
    public Vector3Int OccupedCellPosition { get; private set; }

    public Direction3 Direction { get; private set; }
    
    public Vector3 BuildPosition => GetComponent<Transform>().position;

    public void Initialize(Direction3 direction)
    {
        Direction = direction;
    }
    
    public void SetPosition(Vector3Int parentPosition)
    {
        OccupedCellPosition = _startOccupedCellPosition + parentPosition;
    }
}