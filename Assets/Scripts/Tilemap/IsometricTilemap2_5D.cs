using System;
using System.Collections.Generic;
using Extentions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IsometricTilemap2_5D : MonoBehaviour
{
    private Dictionary<Vector3Int, Tile> _placedTiles = new Dictionary<Vector3Int, Tile>();
    public void SetTile(Tile tile, Vector3Int position)
    {
        if (_placedTiles.ContainsKey(position)) ReplaceTile(tile, position);
        else _placedTiles.Add(position, tile);
        CorrectTilePosition(tile, position);
        
    }

    private BuildingType GetTileBuildingTypeByPosition(Vector3Int position)
    {
        return _placedTiles.ContainsKey(position) ? _placedTiles[position].TileBuildingType : BuildingType.None;
    }
    
    private void ReplaceTile(Tile tile, Vector3Int position)
    {
        _placedTiles[position].Destroy();
        _placedTiles[position] = tile;
    }

    private void CorrectTilePosition(Tile tile, Vector3Int position)
    {
        tile.SetWorldPosition(GetWorldPositionByIsometricPosition(position));
        tile.SetTileDepth(CalculateDepth(position));
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