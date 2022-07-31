using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using UnityEngine;


public class GameField : MonoBehaviour, IBuildingsContainer
{

    private Cell[,,] _cells;
    public readonly Vector3Int Size = new(12, 4, 12);
    [SerializeField] private GameFieldCellView _cellViewPrefab;
    [SerializeField] private Transform _debugObjectTransform;

    private void Awake()
    {
        GenerateGameField();
    }

    private void GenerateGameField()
    {
        _cells = new Cell[Size.x, Size.y, Size.z];
        for (var y = 0; y < Size.y; y++)
        {
            for (var z = 0; z < Size.z; z++)
            {
                for (var x = 0; x < Size.x; x++)
                {
                    const int positionMultiplier = GeneralGameSettings.GameFieldSettings.WorldPositionMultiplier;
                    var currentCell = _cells[x, y, z] = new Cell(new Vector3Int(x, y, z));

                    if (GeneralGameSettings.DebugMode)
                    {
                        var cellView = Instantiate(_cellViewPrefab,
                            new Vector3(x * positionMultiplier, y * positionMultiplier, z * positionMultiplier),
                            Quaternion.identity, _debugObjectTransform);

                        cellView.Initialize(currentCell);

                    }

                    if (y > 0)
                    {
                        Cell.SetUpDownNeighbours(currentCell, _cells[x, y - 1, z]);
                    }

                    if (z > 0)
                    {
                        Cell.SetFowardBackNeighbours(currentCell, _cells[x, y, z - 1]);
                    }

                    if (x > 0)
                    {
                        Cell.SetRightLeftNeighbours(currentCell, _cells[x - 1, y, z]);
                    }
                }
            }
        }
    }


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
        Debug.Log("buildingWasSetted");
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
        Debug.Log("buildingWasSetted");
        return true;
    }

    private void SubscribeBuilding(Building building)
    {
        building.Destroyed += OnBuildingDied;
        building.DirectionChanged += OnBuildingDirectionChanged;
    }
    
    private void OnBuildingDirectionChanged(object sender)
    {
        var building = sender as Building;
        if (sender is Building == false) throw new InvalidOperationException();
        TrySetCells(building);
    }

    private void OnBuildingDied(object sender)
    {
        var building = sender as Building;

        if (sender is Building == false) throw new InvalidOperationException();
        building.Destroyed -= OnBuildingDied;
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

                var settedBuildings = cellNeighbour?.SettedBuildings;
                if (settedBuildings == null|| !settedBuildings.Any()) continue;
                foreach (var settedBuilding in settedBuildings)
                {
                    if (NeighbourIsSetted(building, settedBuilding) ||
                        NeighbourIsSetted(settedBuilding, building)) continue;
                    
                    building.AddNeighbour(settedBuilding, direction);
                    settedBuilding.AddNeighbour(building, direction.GetOppositeDirection());
                    
                    bool NeighbourIsSetted(Building startBuilding, Building possibleNeighbour)=>
                     startBuilding.Neighbors[direction]?.Contains(possibleNeighbour) ?? false;
                }
            }
            
            var occupyingCellPosition = startCell.Position + occupyingCell.Position;
            var cell = GetCellByPosition(occupyingCellPosition);
            if (cell.IsFilled || cell.Capacity < building.Weight || 
                !Building.CanBeMergedWithBuildings(building, cell.SettedBuildings)) return false;
            cells.Add(cell);
            occupyingCells.Add(OccupyingCell.Create(occupyingCellPosition, occupyingCell.SettedNeighbours));
        }

        
        
        foreach (var cell in building.SettedCells.Select(settedCell => GetCellByPosition(settedCell.Position)))
        {
            cell.RemoveBuilding(building);
        }

        foreach (var cell in cells)
        {
            cell.SetBuilding(building);
        }

        building.SettedCells = occupyingCells;
        return true;
    }

    public Cell GetCellByPosition(Vector3Int position)
    {
        return _cells[position.x, position.y, position.z];
    }

    public static Vector3Int ConvertWorldToGameFieldPosition(Vector3 position)
    {
        var correctPosition =
            position.RoundToVector3Int() / GeneralGameSettings.GameFieldSettings.WorldPositionMultiplier;
        return correctPosition;
    }
    
    private void OnDisable()
    {
        for (var y = 0; y < Size.y; y++)
        {
            for (var z = 0; z < Size.z; z++)
            {
                for (var x = 0; x < Size.x; x++)
                {
                    _cells[x, y, z].Reset();
                }
            }
        }
    }

    public bool CellsIsFreeToSet(Building building ,List<OccupyingCell> occupyingCells)
    {
        var buildingPosition = building.PositionOnGameField;
        foreach (var occupyingCell in occupyingCells)
        {
            var currentCell = GetCellByPosition(occupyingCell.Position + buildingPosition);
            if (!currentCell.SettedBuildings.Contains(building) && !currentCell.BuildingCanBeSettedOnCell(building))
                return false;
        }

        return true;
    }
    
}
