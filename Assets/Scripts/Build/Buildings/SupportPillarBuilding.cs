using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Extentions;
using UnityEngine;

public class SupportPillarBuilding : BuildingWithChilds
{
    public override List<OccupyingCell> OccupyingCells { get; } = new()
    {
        new OccupyingCell(new Vector3Int(0, 0, 0),new Neighbors3<bool>(){Up = true}),
        new OccupyingCell(new Vector3Int(0, 1, 0), new Neighbors3<bool>(){Down = true})
    };

    
    public override TargetType TargetType => TargetType.Tower;
    public override BuildingType BuildingType => BuildingType.SupportPillar;
    protected override Direction2 Direction { get; set; } = Direction2.Foward;

    public override int Health { get; protected set; }
    public override DamagableType DamagableType => DamagableType.Wooden;
    
    protected override BuildPoints _buildPoints { get; }

    public SupportPillarBuilding(IBuildingsContainer buildingsContainer) : base(buildingsContainer)
    {
    }
    public override void AddChildBuilding(Building building, Direction3 direction)
    {
        if (Neighbors[direction] == null || Neighbors[direction] == building) return;

            base.AddChildBuilding(building, direction);
        if (building.BuildingType != BuildingType.Wall) return;
        
        var neighbourBuilding = Neighbors[direction];
            
        if (neighbourBuilding is not BuildingWithChilds neighbourSupportPillarBuilding || 
            neighbourSupportPillarBuilding.BuildingType != BuildingType.Wall) throw new InvalidOperationException();
        
        neighbourSupportPillarBuilding.AddChildBuilding(building, direction.GetOppositeDirection());
    }


    public override IEnumerable<BuildPoint> GetCorrectBuildPoints(BuildingType buildingType)
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


}
