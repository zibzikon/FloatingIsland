using System;
using System.Collections.Generic;
using Extentions;
using UnityEngine;


public class GameField : MonoBehaviour
{

    private Cell[,,] _cells;
    public Vector3Int Size = new(10, 10, 10);
    [SerializeField] private GameFieldCellView _cellViewPrefab;
    [SerializeField] private Transform _debugObjectTransform;

    private void Start()
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


    public bool TrySetBuilding(Building building, IBuildingContainer buildingContainer, BuildPoint buildPoint,
        bool firstSetting)
    {
        buildingContainer.SetBuildPointsPositions();
        if (TrySetCells(building, GetCellByPosition(buildPoint.OccupedCellPosition), firstSetting) == false)
            return false;
        building.SetSupportBuilding(buildingContainer);
        building.SetPositionOnGameField(buildPoint.OccupedCellPosition);
        building.transform.position = buildPoint.BuildPosition;
        Debug.Log("buildingWasSeted");
        return true;
    }

    public bool TrySetBuilding(Building building, Cell startCell, bool firstSetting)
    {
        if (TrySetCells(building, startCell, firstSetting) == false) return false;
        building.transform.position = startCell.WorldPosition;
        building.SetPositionOnGameField(startCell.Position);
        Debug.Log("buildingWasSeted");
        return true;
    }

    private bool TrySetCells(Building building, Cell startCell, bool firstSetting)
    {
        var cells = new List<Cell>();
        var occupingCells = new List<OccupingCell>();

        foreach (var occupyingCell in building.OccupyingCells)
        {
            var occupyingCellPosition = startCell.Position + occupyingCell.Position;
            var cell = GetCellByPosition(occupyingCellPosition);
            if (cell.IsFilled || cell.Ccapacity < building.Weight) return false;
            cells.Add(cell);
            occupingCells.Add(new OccupingCell(occupyingCellPosition));
        }

        foreach (var setedCell in building.SetedCells)
        {
            var cell = GetCellByPosition(setedCell.Position);
            cell.RemoveBuilding(building);
        }

        foreach (var cell in cells)
        {
            cell.SetBuilding(building);
        }

        building.SetedCells = occupingCells;
        return true;
    }

    public Cell GetCellByPosition(Vector3Int position)
    {
        return _cells[position.x, position.y, position.z];
    }

    public Cell GetCellByWorldPosition(Vector3 position)
    {
        var correctPosition =
            position.RoundToVector3Int() / GeneralGameSettings.GameFieldSettings.WorldPositionMultiplier;
        return _cells[correctPosition.x, correctPosition.y, correctPosition.z];
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
}
