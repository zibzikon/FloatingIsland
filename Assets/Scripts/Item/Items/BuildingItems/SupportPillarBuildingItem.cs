
using Enums;

public class SupportPillarBuildingItem : BuildingItem
{
    public override ItemType ItemType => ItemType.SupportPillarBuilding;
    public override int ItemStackCount => 24;
    protected override BuildingType BuildingType => BuildingType.SupportPillar;
}
