
using Enums;

public class WallBuildingItem : BuildingItem
{
    public override ItemType ItemType => ItemType.WallBuilding;
    public override int ItemStackCount => 24;
    protected override BuildingType BuildingType => BuildingType.Wall;
}
