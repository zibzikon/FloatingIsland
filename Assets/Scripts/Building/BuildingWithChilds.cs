using System.Collections.Generic;
using System.Linq;
using Factories.Building;
using UnityEngine;

public class BuildingWithChilds : Building, IUpdatable, IBuildingContainer
{
    [SerializeField] protected BuildPoints _buildPoints;

    [SerializeField] private ConstructionsFactory _constructionsFactory;
    
    private void Start()
    {
        foreach (var direction in DirectionExtentions.GetDirectionEnumerable())
        {
            foreach (var buildPoint in _buildPoints.Points[direction])
            {
                buildPoint.Initialize(direction);
            }
        }
    }
    
    private static readonly IEnumerable<BuildingType> BuildingTypesWithChilds = new[]
    {
        BuildingType.SupportPillar,
        BuildingType.Wall
    };

    public IEnumerable<BuildPoint> GetCorrectBuildPoints(Building building)
    {
        var allCorrectBuildPoints = new List<BuildPoint>();
        for (int i = 0; i < Neighbors3<BuildPoint>.Length; i++)
        {
           var correctBuildPoints = 
               _buildPoints.Points[i].Where(buildPoint => buildPoint.WhiteList.Contains(building.BuildingType));
           allCorrectBuildPoints.AddRange(correctBuildPoints);
        }

        return allCorrectBuildPoints;
    }

    public IEnumerable<Direction3> GetGetCorrectBuildPointsDirections(Building building)
    {
        var buildPoints = GetCorrectBuildPoints(building);
        
        var directions = new List<Direction3>();

        foreach (var buildPoint in buildPoints)
        {
            if (!directions.Contains(buildPoint.Direction))
            {
                directions.Add(buildPoint.Direction);
            }
        }

        return directions;
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

