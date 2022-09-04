using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class OakSapling : Plant
{
    public override int Health { get; protected set; }
    public override DamagableType DamagableType => DamagableType.Wooden;

    protected override Dictionary<Direction2, Vector3> DirectionRotation { get; }

    public override List<OccupyingCell> OccupyingCells { get; }
    public override BuildingType BuildingType { get; }
    
    protected override ReadOnlyArray<IPlantGrowState> GrowStates { get; }
    
    public OakSapling(IBuildingsContainer buildingsContainer) : base(buildingsContainer)
    {
    }
}
