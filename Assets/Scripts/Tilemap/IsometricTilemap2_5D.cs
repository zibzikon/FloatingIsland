using System;
using System.Collections.Generic;
using Extentions;
using Factories.BuildingFactories;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IsometricTilemap2_5D : MonoBehaviour
{
    private Dictionary<Vector3Int, BuildingTile> _placedTiles = new Dictionary<Vector3Int, BuildingTile>();

    private BuildingTilesFactory _buildingTilesFactory;
    
    public void Initialize(BuildingTilesFactory buildingTilesFactory)
    {
        _buildingTilesFactory = buildingTilesFactory;
    }
    
    public BuildingTile SetTile(BuildingType buildingType, Vector3Int position)
    {
        var buildingTile = _buildingTilesFactory.Get(buildingType);
        
        if (ExistTile(position)) ReplaceTile(buildingTile, position);
        else _placedTiles.Add(position, buildingTile);
        CorrectTilePosition(buildingTile, position);
        
        return buildingTile;
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
        buildingTile.SetWorldPosition(position.IsometricToScreenPosition());
        buildingTile.SetTileDepth(CalculateDepth(position));
    }

    public bool ExistTile(Vector3Int position)
    {
        return _placedTiles.ContainsKey(position);
    }
    
    
    private int CalculateDepth(Vector3Int position)
    {
        return (-position.x -position.y) + position.z;
    }
}