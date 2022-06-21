using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Construction : BuildingWithChilds
{
    [SerializeField] private List<BuildingCell> _occupyingBuildingCells;

    public Construction TryGetConstruction(Building building, 
        IBuildingsContainer buildingsContainer)
    {
        if (ValidateBuilding(building.PositionOnGameField, buildingsContainer) == false)
            return null;
        var buildPoints = new List<BuildPoint>();
        for (int i = 0; i < Neighbors3<BuildPoints>.Length; i++)
        {
            foreach (var buildPoint in _buildPoints.Points[i].Where(buildPoint =>
                         buildPoint.WhiteList.Contains(building.BuildingType)))
            {
               
                buildPoints.Add(buildPoint);
            }
        }

        return null;
    }

    public bool ValidateBuilding(Vector3Int startPosition, IBuildingsContainer buildingsContainer)
    {
        foreach (var occupyingCell in _occupyingBuildingCells)
        {
            var correcBuilding = buildingsContainer.TryGetBuildingByPosition(startPosition + occupyingCell.Position);
            if (correcBuilding == null && correcBuilding.BuildingType != occupyingCell.BuildingType)
            {
                return false;
            }
        }

        return true;
    }
}

[Serializable]
public struct BuildingCell
{
    [SerializeField] private Vector3Int _position; 
    public Vector3Int Position => _position;
    
    [SerializeField] private BuildingType _buildingType;
    public BuildingType BuildingType => _buildingType;
}