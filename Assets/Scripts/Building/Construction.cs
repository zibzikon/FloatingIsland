using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Construction : MonoBehaviour,INeighbour, IBuildingContainer
{
    private readonly Neighbors3<List<INeighbour>> _neighbors = new();
    
    [SerializeField] private BuildPoints _buildPoints;
    
    [SerializeField] private List<BuildingDesignator> _buildingDesignators;
    private GameField _gameField;
    
    public Vector3 WorldPosition => transform.position;
    
    public bool CheckByBuildingType(BuildingType buildingType)
    {
        var isPossible = 
            _buildingDesignators.FirstOrDefault(buildingDesignator =>
                buildingDesignator.BuildingType == buildingType)!
            .BuildingType == buildingType;
        return isPossible;
    }

    public bool CheckBuilding(Building building, out Vector3 centerPosition)
    {
        return CheckBuilding(building, building.Neighbors, out centerPosition);
    }

    private bool CheckBuilding(Building building, Neighbors3<List<INeighbour>> neighbors, out Vector3 centerPosition)
    {
        var buildingNeighbors = new Neighbors3<List<Building>>();
        
        var buildings = new List<Building>();
        foreach (var direction in DirectionExtentions.GetDirectionEnumerable())
        {
            if (neighbors[direction] == null) continue;
            foreach (var neighbour in neighbors[direction])
            {
                var buildingNeighbour = neighbour as Building;
                    
                if (!(neighbour is Building)) continue;
                
                buildingNeighbors[direction] ??= new List<Building>();
                buildingNeighbors[direction].Add(buildingNeighbour);
            }
                     
            

            if (buildingNeighbors[direction] == null || !buildingNeighbors[direction].Any()) continue;
            
            if(_buildingDesignators.Where(buildingDesignator => buildingDesignator.Neighbors[direction] != null)
                .Any(buildingDesignator => CheckBuilding(buildingNeighbors[direction],
                    buildingDesignator.Neighbors[direction],
                    buildings)));
            {
                buildings.Add(building);
                centerPosition = GetCenterPosition();
                return true;
            }
        }

        centerPosition = Vector3.positiveInfinity;
        Debug.Log("Yes");
        return false;

        Vector3 GetCenterPosition()
        {
            var centerPosition = new Vector3();
            buildings.ForEach(building => centerPosition += building.transform.position);

            return centerPosition / buildings.Count;
        }
    }

    private bool CheckBuilding(List<Building> buildings, List<BuildingDesignator> buildingDesignators,
        List<Building> trueBuildings)
    {
        if (buildings == null) throw new NullReferenceException();

        var buildingTypesList = buildings?.Select(building => building.BuildingType).ToList();
        var buildingDesignatorsTypesList = buildingDesignators?.Select(buildingDesignator => buildingDesignator.BuildingType).ToList();
      
        for (var i = 0; i < buildingTypesList.Count; i++)
        {
            if (buildingDesignatorsTypesList == null ||
                !buildingDesignatorsTypesList.Contains(buildingTypesList[i])) return false;
            
            trueBuildings.Add(buildings[i]);
            buildingDesignatorsTypesList.Remove(buildingTypesList[i]);
        }

        return true;
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
        for (var i = 0; i < 6; i++)
        {
            foreach (var buildPoint in _buildPoints.Points[i])
            {
                buildPoint.SetPosition(GameField.ConvertWorldToGameFieldPosition(WorldPosition));
            }
        }
    }

    public IEnumerable<BuildPoint> GetCorrectBuildPoints(BuildingType buildingType)
    {
        _buildPoints.Initialize(this);
        var possibleBuildPoints = new List<BuildPoint>();
        foreach (var direction in DirectionExtentions.GetDirectionEnumerable())
        {
            var buildPoints = _buildPoints.Points[direction];
            if (buildPoints == null|| !buildPoints.Any()) continue;

            possibleBuildPoints.AddRange(buildPoints.Where(buildPoint => buildPoint.BuildingCanBeSetted(buildingType)));
        }

        return possibleBuildPoints;
    }

    public void AddNeighbour(INeighbour neighbour, Direction3 direction)
    {
        var neighbourList = _neighbors[direction] ?? (_neighbors[direction] = new List<INeighbour>());
        
        neighbourList.Add(neighbour);
    }
}

