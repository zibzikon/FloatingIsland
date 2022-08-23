using System;
using System.Collections.Generic;
using Extentions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IsometricTilemap2_5D : MonoBehaviour
{
    private Dictionary<Vector3Int, BuildingTile> _placedTiles = new Dictionary<Vector3Int, BuildingTile>();
    public void SetTile(BuildingTile buildingTile, Vector3Int position)
    {
        if (_placedTiles.ContainsKey(position)) ReplaceTile(buildingTile, position);
        else _placedTiles.Add(position, buildingTile);
        CorrectTilePosition(buildingTile, position);
        
    }

    public BuildingType GetTileBuildingTypeByPosition(Vector3Int position)
    {
        return _placedTiles.ContainsKey(position) ? _placedTiles[position].BuildingType : BuildingType.None;
    }
    
    private void ReplaceTile(BuildingTile buildingTile, Vector3Int position)
    {
        _placedTiles[position].Destroy();
        _placedTiles[position] = buildingTile;
    }

    private void CorrectTilePosition(BuildingTile buildingTile, Vector3Int position)
    {
        buildingTile.SetWorldPosition(GetWorldPositionByIsometricPosition(position));
        buildingTile.SetTileDepth(CalculateDepth(position));
    }

    private Vector2 GetWorldPositionByIsometricPosition(Vector3Int position)
    {
        var tileSize = GeneralGameSettings.Tilemap.TileSize;
        
        var tileX = position.x / tileSize.y;
        var tileY = position.y / tileSize.y;
        
        var x = (tileX - tileY) * tileSize.x / 2 + tileSize.x / 2;
        var y = ((tileX + tileY) * tileSize.y / 2) + position.z * (tileSize.y / 2);
        
        return new Vector2(x, y);
    }

    private int CalculateDepth(Vector3Int position)
    {
        return (-position.x -position.y) + position.z;
    }
}