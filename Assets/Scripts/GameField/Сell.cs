using System;
using UnityEngine;

public class Cell
{
    public event Action Changed;
    public Vector3Int Position { get; }
    public Vector3 WorldPosition => Position * GeneralGameSettings.GameFieldSettings.WorldPositionMultiplier;
    
    public readonly Neighbors3<Cell> Neighbours = new();

    private Building _placedBuilding;
    public Building PlacedBuilding => _placedBuilding;
    
    public bool IsBlocked => PlacedBuilding != null;
    
    public Cell(Vector3Int position)
    {
        Position = position;
    }

    public bool BuildingCanBeSettedOnCell(Building building) =>_placedBuilding == null || Building.CanBeMergedWithBuildings(building, new []{_placedBuilding} );

    public void SetBuilding(Building building)
    {
        building.Destroyed += OnBuildingDestroyed;
        building.DirectionChanged += OnBuildingDestroyed;
        _placedBuilding =building;
        Changed?.Invoke();
    }
    
    public void RemoveBuilding()
    {
        Reset();
        Changed?.Invoke();
    }
    
    private void OnBuildingDestroyed(object sender)
    {
        _placedBuilding = null;
    }

    public void Reset()
    {
        _placedBuilding.Destroyed -= OnBuildingDestroyed;
        _placedBuilding = null;
    }
    
    public static void SetRightLeftNeighbours(Cell right, Cell left)
    {
        right.Neighbours.Left = left;
        left.Neighbours.Right = right;
    }
    
    public static void SetForwardBackNeighbours(Cell foward, Cell back)
    {
        foward.Neighbours.Back = back;
        back.Neighbours.Forward = foward;
    }
    
    public static void SetUpDownNeighbours(Cell up, Cell down)
    {
        up.Neighbours.Down = down;
        down.Neighbours.Up = up;
    }
}


