using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using UnityEngine;


public class GameField : MonoBehaviour, IBuildingsContainer
{
    private readonly Dictionary<Vector3Int, Cell> _cells = new();
    
    private readonly Dictionary<Vector2Int, Range<Vector2Int>> _range = new();

    public event Action ContentChanged;

    public event Action SizeChanged;

    public int MaxYSize { get; private set; } = 10;
        
    public IEnumerable<Cell> GetCells()
        =>_cells.Select(cell => cell.Value);

    public IEnumerable<KeyValuePair<Vector2Int ,Range<Vector2Int>>> GetGameFieldRanges() => _range;

    public void Initialize()
    {
        GenerateStartGameField(new Vector2Int(10,10));
    }

    private void GenerateStartGameField(Vector2Int size)
    {
        for (var x = 0; x < size.x; x++)
        {
            for (var z = 0; z < size.y; z++)
            {
                for (var y = 0; y < MaxYSize; y++)
                {
                    var position = new Vector3Int(x, y, z);
                    var cell = new Cell(position);
                    _cells.Add(position, cell);
                    var vector2Position = new Vector2Int(x,z);
                    
                    if (_range.ContainsKey(vector2Position) != false) continue;
                    var range = new Range<Vector2Int>(size, Vector2Int.zero);
                    _range.Add(vector2Position, range);
                }
            }
        }
    }
    
    public bool TrySetBuilding(Building building,  BuildPoint buildPoint)
    {
        buildPoint.BuildingContainer.SetBuildPointsPositions();
        building.SetPositionOnGameField(buildPoint.OccupedCellPosition);
        
        if (building.ValidateSetSupportBuilding(buildPoint.BuildingContainer) == false ||
            TryPlaceBuildingOnCells(building) == false) return false;
        
        building.Place(buildPoint.OccupedCellPosition);
        SubscribeBuilding(building);
        Debug.Log("buildingWasPlaced");
        return true;
    }

    public bool TrySetBuilding(Building building, Vector3Int position)
    {
        var startCell = GetCellByPosition(position);
        if (TryPlaceBuildingOnCells(building) == false) return false;
        building.Place(position);        
        SubscribeBuilding(building);
        ContentChanged?.Invoke();
        Debug.Log("buildingWasPlaced");
        return true;
    }

    private void SubscribeBuilding(Building building)
    {
        building.Destroyed += OnBuildingDestroyed;
    }
    
    private void OnBuildingDirectionChanged(object sender)
    {
        var building = sender as Building;
        if (sender is Building == false) throw new InvalidOperationException();
        TryPlaceBuildingOnCells(building);
    }

    private void OnBuildingDestroyed(object sender)
    {
        var building = sender as Building;

        if (sender is Building == false) throw new InvalidOperationException();
        building.Destroyed -= OnBuildingDestroyed;
        ContentChanged?.Invoke();
    }
    

    
    private bool TryPlaceBuildingOnCells(Building building)
    {
        var startCell = GetCellByPosition(building.PositionOnGameField);
        var cells = new List<Cell>(); 
        var occupyingCells = new List<OccupyingCell>();

        foreach (var occupyingCell in building.OccupyingCells)
        {
            
            foreach (var direction in DirectionExtensions.GetDirectionEnumerable())
            {
                if (occupyingCell.SettedNeighbours[direction]) continue;
                
                var cellNeighbour = startCell.Neighbours[direction];

                var placedBuilding = cellNeighbour?.PlacedBuilding;
                if (placedBuilding == null) continue;

                if (NeighbourIsPlaced(building, placedBuilding) ||
                    NeighbourIsPlaced(placedBuilding, building)) continue;
                
                building.SetNeighbour(placedBuilding, direction);
                placedBuilding.SetNeighbour(building, direction.GetOppositeDirection());
                
                bool NeighbourIsPlaced(Building startBuilding, Building possibleNeighbour)
                {
                    var buildingNeighbor = startBuilding.Neighbors[direction];
                    return buildingNeighbor != null && buildingNeighbor == possibleNeighbour;
                }
            }
            
            var occupyingCellPosition = startCell.Position + occupyingCell.Position;
            var cell = GetCellByPosition(occupyingCellPosition);
            if (cell.BuildingCanBeSettedOnCell(building) == false) return false;
            cells.Add(cell);
            occupyingCells.Add( new OccupyingCell(occupyingCellPosition, occupyingCell.SettedNeighbours));
        }
        
        foreach (var cell in building.OccupyingCellsOnGameField.Select(cell => GetCellByPosition(cell.Position)))
            cell.RemoveBuilding();

        foreach (var cell in cells)
            cell.SetBuilding(building);
        
        building.OccupyingCellsOnGameField = occupyingCells;
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
        SizeChanged?.Invoke();
    }
    
    public Vector3Int ConvertScreenToGameFieldPosition(Vector2 position)
    {
        var correctPosition =
            position.ScreenToIsometricPosition();
        
        return correctPosition;
    }
    
    private void OnDisable()
    {
        // ToDo
    }

    public bool CellsIsFreeToSet(Building building , IEnumerable<OccupyingCell> occupyingCells)
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

    public Range(T max, T min )
    {
        Max = max;
        Min = min;
    }
}
