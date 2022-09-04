using System;
using Factories.BuildingFactories;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameFieldView : MonoBehaviour
{
    [SerializeField] private IsometricTilemap2_5D _tilemap;
    private GameField _gameField;

    public void Initialize(GameField gameField, BuildingTilesFactory buildingTilesFactory)
    {
        _gameField = gameField;
        _tilemap.Initialize(buildingTilesFactory);
        SubscribeEvents();
        UpdateView();
        OnGameFieldSizeChanged();
    }

    private void SubscribeEvents()
    {
        _gameField.ContentChanged += UpdateView;
        _gameField.SizeChanged += OnGameFieldSizeChanged;
    }
    
    private void UnSubscribeEvents()
    {
        _gameField.ContentChanged -= UpdateView;
        _gameField.SizeChanged -= OnGameFieldSizeChanged;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    private void UpdateView()
    {
        foreach (var cell in _gameField.GetCells())
        {
            var placedBuilding = cell.PlacedBuilding;
            
            if (placedBuilding == null) continue;
            
            var buildingPosition = placedBuilding.PositionOnGameField;
            var cellPosition = new Vector3Int(buildingPosition.x, buildingPosition.z, buildingPosition.y);

            if (cell.IsBlocked == false) continue;

            var tileBuildingType = _tilemap.GetTileBuildingTypeByPosition(cellPosition);
            var currentBuildingType = placedBuilding.BuildingType;
            if (currentBuildingType == tileBuildingType) continue;
            
            var tile = _tilemap.SetTile(currentBuildingType, cellPosition);
        }
    }

    private void OnGameFieldSizeChanged()
    {
        foreach (var (position, range) in _gameField.GetGameFieldRanges())
        {
            var tilePosition = new Vector3Int(position.x, position.y, -1);
            if (_tilemap.ExistTile(tilePosition) == false)
                _tilemap.SetTile(BuildingType.GameFieldPart, tilePosition);
        }
    }
}