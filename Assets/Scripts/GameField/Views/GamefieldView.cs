using System;
using Factories.BuildingFactories;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GamefieldView : MonoBehaviour
{
    [SerializeField] private IsometricTilemap2_5D _tilemap;
    private BuildingTilesFactory _buildingTilesFactory;
    private GameField _gameField;

    private void UpdateView()
    {
        foreach (var cell in _gameField.GetCells())
        {
            var placedBuilding = cell.PlacedBuilding;
            var cellPosition = cell.Position;

            if (cell.IsBlocked == false) continue;

            var tileBuildingType = _tilemap.GetTileBuildingTypeByPosition(cellPosition);
            if (placedBuilding.BuildingType == tileBuildingType) continue;

            var tile = _buildingTilesFactory.Get(tileBuildingType);
            _tilemap.SetTile(tile, cellPosition);
        }
    }

    private void OnGameFieldSizeUpdated()
    {
        foreach (var range in _gameField.GetGameFieldRanges())
            for (var x = range.Min.x; x < range.Max.x; x++)
                for (var y = range.Min.y; y < range.Max.y; y++) 
                    _tilemap.SetTile(_buildingTilesFactory.Get(BuildingType.GameFieldPart), new Vector3Int(x, y, -1));
        
    }
}