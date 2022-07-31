using Enums;

public class TowerBuilding : BuildingWithChilds
{
    public override int Weight => 70;
    protected override BuildingStats BuildingStats { get; } = new BuildingStats
    {
        Health = 100
    };
    public override TargetType TargetType => TargetType.Tower;
    public override BuildingType BuildingType => BuildingType.Tower;
    public override DamagableType DamagableType => DamagableType.Stone;

    protected override Direction2 Direction { get; set; }
}
