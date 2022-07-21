
using Enums;

public class WallBuildingItem : BuildingItem
{
    public override ItemType ItemType => ItemType.WalBuilding;
    protected override BuildingType BuildingType => BuildingType.Wall;
}
