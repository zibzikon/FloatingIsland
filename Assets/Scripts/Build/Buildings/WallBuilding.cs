
using System;
using System.Collections.Generic;
using System.Linq;
using Enums;

public class WallBuilding : BuildingWithChilds
{
    public override List<OccupyingCell> OccupyingCells { get; }
    public override TargetType TargetType => TargetType.Tower;
    public override BuildingType BuildingType => BuildingType.Wall;
    public override int Health { get; protected set; }
    public override DamagableType DamagableType => DamagableType.Stone;
    protected override Direction2 Direction { get; set; } = Direction2.Foward;

    protected override BuildPoints _buildPoints { get; }
    
    public WallBuilding(IBuildingsContainer buildingsContainer) : base(buildingsContainer)
    {
    }

    public override bool ValidateSetSupportBuilding(IBuildingContainer supportBuilding)
    {
        var correctBuildPoints = new List<BuildPoint>();
        for (var i = 0; i < Neighbors3<BuildPoint>.Length; i++)
        {
            correctBuildPoints.AddRange(
                _buildPoints.Points[i].Where(buildPoint =>
                    buildPoint.BuildingCanBeSetted(supportBuilding.BuildingType)));
        }

        if (!correctBuildPoints.All(point => point.BuildingCanBeSetted(BuildingType.SupportPillar)))
            return correctBuildPoints.Any();
        
        return false;
    }
    

}
