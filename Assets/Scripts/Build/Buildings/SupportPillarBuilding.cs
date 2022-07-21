using System;
using System.Collections.Generic;
using System.Linq;
using Enums;

public class SupportPillarBuilding : BuildingWithChilds
{
    protected override BuildingStats BuildingStats { get; } = new BuildingStats
    {
        Health = 100
    };
    public override TargetType TargetType => TargetType.Tower;
    public override int Weight => 20;
    public override BuildingType BuildingType => BuildingType.SupportPillar;
    protected override Direction2 Direction { get; set; } = Direction2.Foward;
    
    public override void AddChildBuilding(Building building, Direction3 direction)
    {
        if (Neighbors[direction] == null || Neighbors[direction].Contains(building)) return;

            base.AddChildBuilding(building, direction);
        if (building.BuildingType != BuildingType.Wall) return;
        
        var neighbourSupportPillarBuilding = (BuildingWithChilds)Neighbors[direction].FirstOrDefault(neighbour => 
            ((BuildingWithChilds)neighbour).BuildingType == BuildingType.SupportPillar);
            
        if (neighbourSupportPillarBuilding == null) throw new InvalidOperationException();
        
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

        var trueCorrectBuildPoints = new List<BuildPoint>();
        foreach (var buildPoint in allCorrectBuildPoints)
        {
            if (buildPoint == null || !buildPoint.BuildingCanBeSetted(BuildingType.Wall))
            { 
                trueCorrectBuildPoints.Add(buildPoint);
                continue; 
            }
            
            if (WallBuilding.WallCanBeSettedOnSupportPillar(this, buildPoint.Direction)) trueCorrectBuildPoints.Add(buildPoint);
        }
        
        return trueCorrectBuildPoints;
    }
    
}
