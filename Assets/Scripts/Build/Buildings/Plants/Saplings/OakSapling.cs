using System.Collections.Generic;
using UnityEngine;

public class OakSapling : Plant
{
    public override int Weight { get; }
    protected override PlantStats PlantStats { get; } = new()
    {
        Health = 20,
        MaxGrowState = 2,
        TimeToGrow = 5
    };
    public override BuildingType BuildingType { get; }

    protected override void OnGrow()
    {
        
    }
    
}
