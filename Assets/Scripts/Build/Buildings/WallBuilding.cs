
using System;
using System.Collections.Generic;
using System.Linq;
using Enums;

public class WallBuilding : BuildingWithChilds
{
    public override int Weight => 30;
    protected override BuildingStats BuildingStats { get; } = new()
    {
        Health = 100,
        DropItems = new List<CountableItem>() { new (ItemType.Wood, 2) }
    };
    public override TargetType TargetType => TargetType.Tower;
    public override BuildingType BuildingType => BuildingType.Wall;
    public override DamagableType DamagableType => DamagableType.Stone;
    protected override Direction2 Direction { get; set; } = Direction2.Foward;


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

        return WallCanBeSettedOnSupportPillar(supportBuilding);
    }

    public static bool WallCanBeSettedOnSupportPillar(IBuildingContainer supportPillar)
    {
        if (WallCanBeSettedOnSupportPillar(supportPillar, Direction3.Right)) return true;
        if (WallCanBeSettedOnSupportPillar(supportPillar, Direction3.Left)) return true;
        if (WallCanBeSettedOnSupportPillar(supportPillar, Direction3.Foward)) return true;
        if (WallCanBeSettedOnSupportPillar(supportPillar, Direction3.Back)) return true;


        return false;
    }

    public static bool WallCanBeSettedOnSupportPillar(IBuildingContainer supportPillar, Direction3 direction)
    {
        var neighbours = supportPillar.Neighbors[direction];
        if (neighbours == null) return false;

        foreach (var neighbour in neighbours)
        {
            if (neighbour is not Building buildingNeighbour) return false;
            if (buildingNeighbour.BuildingType == BuildingType.SupportPillar) return true;
        }

        return false;
    }
    
}
