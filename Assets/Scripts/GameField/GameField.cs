using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using UnityEngine;


public class GameField : MonoBehaviour, IBuildingsContainer
{

    private Dictionary<Vector3Int, Cell> _cells = new();
    
    private Dictionary<Vector2Int, Range<Vector2Int>> _range = new();

    public int MaxYSize { get; private set; } = 10;
    public IEnumerable<Cell> GetCells()
        =>_cells.Select(cell => cell.Value);

    public IEnumerable<Range<Vector2Int>> GetGameFieldRanges() => _range.Select(x => x.Value);
    
    public bool TrySetBuilding(Building building,  BuildPoint buildPoint)
    {
        buildPoint.BuildingContainer.SetBuildPointsPositions();
        building.SetPositionOnGameField(buildPoint.OccupedCellPosition);
        
        if (building.ValidateSetSupportBuilding(buildPoint.BuildingContainer) == false ||
            TrySetCells(building) == false)
            return false;

        SubscribeBuilding(building);
        building.SetSupportBuilding(buildPoint, buildPoint.Direction);
        building.transform.position = buildPoint.GetBuildPosition(buildPoint.BuildingContainer.WorldPosition);
        Debug.Log("buildingWasPlaced");
        return true;
    }

    public bool TrySetBuilding(Building building)
    {
        var startCell = GetCellByPosition(building.PositionOnGameField);
        if (TrySetCells(building) == false) return false;
        SubscribeBuilding(building);
        building.DirectionChanged += OnBuildingDirectionChanged;
        building.Destroyed += OnBuildingDirectionChanged;
        building.transform.position = startCell.WorldPosition;
        Debug.Log("buildingWasPlaced");
        return true;
    }

    private void SubscribeBuilding(Building building)
    {
        building.Destroyed += OnBuildingDestroyed;
        building.DirectionChanged += OnBuildingDirectionChanged;
    }
    
    private void OnBuildingDirectionChanged(object sender)
    {
        var building = sender as Building;
        if (sender is Building == false) throw new InvalidOperationException();
        TrySetCells(building);
    }

    private void OnBuildingDestroyed(object sender)
    {
        var building = sender as Building;

        if (sender is Building == false) throw new InvalidOperationException();
        building.Destroyed -= OnBuildingDestroyed;
        building.DirectionChanged -= OnBuildingDirectionChanged;
    }
    
    private bool TrySetCells(Building building)
    {
        var startCell = GetCellByPosition(building.PositionOnGameField);
        var cells = new List<Cell>();
        var occupyingCells = new List<OccupyingCell>();

        foreach (var occupyingCell in building.OccupyingCells)
        {
            
            foreach (var direction in DirectionExtentions.GetDirectionEnumerable())
            {
                if (occupyingCell.SettedNeighbours[direction]) continue;
                
                var cellNeighbour = startCell.Neighbours[direction];

                var placedBuilding = cellNeighbour?.PlacedBuilding;
                if (placedBuilding == null) continue;

                if (NeighbourIsPlaced(building, placedBuilding) ||
                    NeighbourIsPlaced(placedBuilding, building)) continue;
                
                building.AddNeighbour(placedBuilding, direction);
                placedBuilding.AddNeighbour(building, direction.GetOppositeDirection());
                
                bool NeighbourIsPlaced(Building startBuilding, Building possibleNeighbour)=> 
                    startBuilding.Neighbors[direction]?.Contains(possibleNeighbour) ?? false;
                
            }
            
            var occupyingCellPosition = startCell.Position + occupyingCell.Position;
            var cell = GetCellByPosition(occupyingCellPosition);
            if (cell.BuildingCanBeSettedOnCell(building) == false) return false;
            cells.Add(cell);
            occupyingCells.Add(OccupyingCell.Create(occupyingCellPosition, occupyingCell.SettedNeighbours));
        }
        
        foreach (var cell in building.SettedCells.Select(placedCell => GetCellByPosition(placedCell.Position)))
            cell.RemoveBuilding();
        

        foreach (var cell in cells)
            cell.SetBuilding(building);
        
        building.SettedCells = occupyingCells;
        return true;
    }

    public Cell GetCellByPosition(Vector3Int position)
    {
        if (_cells.ContainsKey(position) == false)
            _cells.Add(position, new Cell(position));
        
        return _cells[position];
    }

    public bool PositionEntersInRange(Vector2Int position)
        => _range.ContainsKey(position);
    
    public Range<Vector2Int> GetGameFieldRangeByPosition(Vector2Int position)
    {
        if (_range.ContainsKey(position) == false) 
            throw new IndexOutOfRangeException();
        
        return _range[position];
    }

    public void IncreaseGameFieldSize(Vector2Int position, Direction2 direction)
    {
        var range = _range[position];
        if (direction == Direction2.Foward || direction == Direction2.Left)
            range.Max += direction.ToVector2();
        else 
            range.Min += direction.ToVector2();

        _range[position] = range;
    }
    public static Vector3Int ConvertWorldToGameFieldPosition(Vector3 position)
    {
        var correctPosition =
            position.RoundToVector3Int() / GeneralGameSettings.GameFieldSettings.WorldPositionMultiplier;
        return correctPosition;
    }
    
    private void OnDisable()
    {
        // ToDo
    }

    public bool CellsIsFreeToSet(Building building ,List<OccupyingCell> occupyingCells)
    {
        var buildingPosition = building.PositionOnGameField;
        foreach (var occupyingCell in occupyingCells)
        {
            var currentCell = GetCellByPosition(occupyingCell.Position + buildingPosition);
            if (!currentCell.PlacedBuilding.Equals(building) && !currentCell.BuildingCanBeSettedOnCell(building))
                return false;
        }

        return true;
    }
}

public struct Range<T>
{
    public T Max;
    public T Min;
}
