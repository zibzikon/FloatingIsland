using System.Collections.Generic;
using Enums;

public class TowerBuilding : BuildingWithChilds
{
    public override List<OccupyingCell> OccupyingCells { get; }
    public override TargetType TargetType => TargetType.Tower;
    public override BuildingType BuildingType => BuildingType.Tower;
    public override int Health { get; protected set; }
    public override DamagableType DamagableType => DamagableType.Stone;

    protected override Direction2 Direction { get; set; }
    
    protected override BuildPoints _buildPoints { get; }

    public TowerBuilding(IBuildingsContainer buildingsContainer) : base(buildingsContainer)
    {
    }
}
