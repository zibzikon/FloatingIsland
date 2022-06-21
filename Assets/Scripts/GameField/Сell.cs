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

    private List<Building> _setedBuildings = new();
    public IEnumerable<Building> SetedBuildings => _setedBuildings;
    
    public bool IsFilled { get; private set; }

    public int Ccapacity { get; private set; } = 100;

    public bool IsBlocked => Ccapacity <= 60;
    
    public Cell(Vector3Int position)
    {
        Position = position;
    }

    public void SetBuilding(Building building)
    {
        if (building.Weight > Ccapacity) throw new IndexOutOfRangeException();
        Ccapacity -= building.Weight;
        building.Died += OnBuildingDestroyed;
        _setedBuildings.Add(building);
        Changed?.Invoke();
    }
    
    public void RemoveBuilding(Building building)
    {
        building.Died -= OnBuildingDestroyed;
        if (Ccapacity + building.Weight > 100) throw new IndexOutOfRangeException();
        Ccapacity += building.Weight;
        _setedBuildings.Remove(building);
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
        _setedBuildings.ForEach(building => building.Died -= OnBuildingDestroyed);
    }
    
    public static void SetRightLeftNeighbours(Cell right, Cell left)
    {
        right.Neighbours.Left = left;
        left.Neighbours.Right = right;
    }
    
    public static void SetFowardBackNeighbours(Cell foward, Cell back)
    {
        foward.Neighbours.Back = back;
        back.Neighbours.Foward = foward;
    }
    
    public static void SetUpDownNeighbours(Cell up, Cell down)
    {
        up.Neighbours.Down = down;
        down.Neighbours.Up = up;
    }
}


