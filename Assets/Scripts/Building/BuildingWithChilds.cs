using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingWithChilds : Building, IUpdatable, IBuildingContainer
{
    [SerializeField] protected BuildPoints _buildPoints;
    
    private List<Building> _childBuildings = new();

    public Vector3 WorldPosition => transform.position;

    private void Awake()
    {
        _buildPoints.Initialize(this);
    }
    
    private static readonly IEnumerable<BuildingType> BuildingTypesWithChilds = new[]
    {
        BuildingType.SupportPillar,
        BuildingType.Wall
    };

    public IEnumerable<BuildPoint> GetCorrectBuildPoints(BuildingType buildingType)
    {
        var allCorrectBuildPoints = new List<BuildPoint>();
        for (var i = 0; i < Neighbors3<BuildPoint>.Length; i++)
        {
           var correctBuildPoints = 
               _buildPoints.Points[i].Where(buildPoint => buildPoint.BuildingCanBeSetted(buildingType));
           allCorrectBuildPoints.AddRange(correctBuildPoints);
        }

        return allCorrectBuildPoints;
    }

    public void AddChildBuilding(Building building, Direction3 direction)
    {
        var buildPoint = _buildPoints.Points[direction]
            .FirstOrDefault(buildPoint => buildPoint.BuildingCanBeSetted(building.BuildingType));

        if (buildPoint == null) throw new InvalidOperationException();
        
        buildPoint.WasSetted = true;
        
        AddNeighbour(building, direction);
        building.AddNeighbour(this, direction.GetOppositeDirection());
    }


    public void SetBuildPointsPositions()
    {
        for (int i = 0; i < 6; i++)
        {
            foreach (var buildPoint in _buildPoints.Points[i])
            {
                buildPoint.SetPosition(this.PositionOnGameField);
            }
        }
    }
    

    public virtual void OnUpdate()
    {
    }
    
    public static bool IsBuildingWithChilds(Building building)
    {
        return BuildingTypesWithChilds.Contains(building.BuildingType);
    }

}

