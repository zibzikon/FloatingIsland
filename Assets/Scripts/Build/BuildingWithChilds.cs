using System;
using System.Collections.Generic;
using System.Linq;
using Factories.Building;
using UnityEngine;

public abstract class BuildingWithChilds : Building, IUpdatable, IBuildingContainer
{
    [SerializeField] protected BuildPoints _buildPoints;
    
    private List<Building> _childBuildings = new();

    public Vector3 WorldPosition => transform.position;

    private void Awake()
    {
        _buildPoints.Initialize(this);
    }

    public override bool ValidateSetSupportBuilding(IBuildingContainer supportBuilding)
    {
        var allCorrectBuildPoints = new List<BuildPoint>();
        for (var i = 0; i < Neighbors3<BuildPoint>.Length; i++)
        {
            allCorrectBuildPoints.AddRange(
                _buildPoints.Points[i].Where(buildPoint =>
                    buildPoint.BuildingCanBeSetted(supportBuilding.BuildingType)));
        }

        return allCorrectBuildPoints.Any();
    }
    
    public virtual IEnumerable<BuildPoint> GetCorrectBuildPoints(BuildingType buildingType)
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

    public virtual void AddChildBuilding(Building building, Direction3 direction)
    {
        var buildPoint = _buildPoints.Points[direction]
            .FirstOrDefault(buildPoint => buildPoint.BuildingCanBeSetted(building.BuildingType));

        if (buildPoint == null) throw new InvalidOperationException();
        
        _childBuildings.Add(building);

        buildPoint.SetChild();
        AddNeighbour(building, direction);
        building.AddNeighbour(this, direction.GetOppositeDirection());
    }


    public void SetBuildPointsPositions()
    {
        for (int i = 0; i < 6; i++)
        {
            foreach (var buildPoint in _buildPoints.Points[i])
            {
                buildPoint.SetPosition(PositionOnGameField);
            }
        }
    }
    
    public override void Destroy()
    {
        _childBuildings.ForEach(x=>x.Destroy());
        base.Destroy();
    }

    public virtual void OnUpdate()
    {
    }
}

