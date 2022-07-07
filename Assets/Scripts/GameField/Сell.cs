using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public event Action Changed;
    public Vector3Int Position { get; }
    public Vector3 WorldPosition => Position * GeneralGameSettings.GameFieldSettings.WorldPositionMultiplier;
    
    public readonly Neighbors3<Cell> Neighbours = new();

    private List<Building> _settedBuildings = new();
    public IEnumerable<Building> SettedBuildings => _settedBuildings;
    
    public bool IsFilled { get; private set; }

    public int Capacity { get; private set; } = 100;

    public bool IsBlocked => Capacity <= 60;
    
    public Cell(Vector3Int position)
    {
        Position = position;
    }

    public void SetBuilding(Building building)
    {
        if (building.Weight > Capacity) throw new IndexOutOfRangeException();
        Capacity -= building.Weight;
        building.Died += OnBuildingDestroyed;
        _settedBuildings.Add(building);
        Changed?.Invoke();
    }
    
    public void RemoveBuilding(Building building)
    {
        building.Died -= OnBuildingDestroyed;
        if (Capacity + building.Weight > 100) throw new IndexOutOfRangeException();
        Capacity += building.Weight;
        _settedBuildings.Remove(building);
        Changed?.Invoke();
    }
    
    private void OnBuildingDestroyed(object sender)
    {
        var building = (Building)sender;
        RemoveBuilding(building);
        
        building.Died -= OnBuildingDestroyed;
    }

    public void Reset()
    {
        _settedBuildings.ForEach(building => building.Died -= OnBuildingDestroyed);
    }
    
    public static void SetRightLeftNeighbours(Cell right, Cell left)
    {
        right.Neighbours.Left = left;
        left.Neighbours.Right = right;
    }
    
    public static void SetFowardBackNeighbours(Cell foward, Cell back)
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


