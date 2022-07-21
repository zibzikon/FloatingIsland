using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

[RequireComponent(typeof( Transform))]
public class BuildPoint: MonoBehaviour
{
    [SerializeField] private List<BuildingType> _whiteList;
    public bool BuildingCanBeSetted(BuildingType buildingType) => 
        _whiteList.Contains(buildingType) && WasSetted == false;
    
    public bool WasSetted { get; private set; }

    public IBuildingContainer BuildingContainer { get; private set; }
    
    
    [SerializeField] private Vector3Int _startOccupedCellPosition;
    public Vector3Int OccupedCellPosition { get; private set; }

    public Direction3 Direction { get; private set; }
    
    public void Initialize(IBuildingContainer buildingContainer, Direction3 direction)
    {
        BuildingContainer = buildingContainer;
        Direction = direction;
    }

    public void SetChild()
    {
        WasSetted = true;
    }
    
    public void Reset()
    {
        WasSetted = false;
    }
    
    public Vector3 GetBuildPosition(Vector3 startPosition)
    {
        return startPosition + transform.localPosition;
    }
    
    public void SetPosition(Vector3Int parentPosition)
    {
        OccupedCellPosition = _startOccupedCellPosition + parentPosition;
    }
}