
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class WoodenCrafter : Crafter
{
    public WoodenCrafter(IBuildingsContainer buildingsContainer) : base(buildingsContainer)
    {
    }
    
    protected override List<CraftItem> AllCraftItems { get; } = new()
    {
        new (new CountableItem(ItemType.Wood, 24),
        new List<CountableItem> { new(ItemType.Wood, 1) }) ,
        
    };

    public override List<OccupyingCell> OccupyingCells { get; }
    public override TargetType TargetType => TargetType.None;
    
    public override BuildingType BuildingType => BuildingType.WoodenCrafter;

    public override int Health { get; protected set; }
    
    public override DamagableType DamagableType => DamagableType.Wooden;
    
    protected override Direction2 Direction { get; set; }
    
    public override bool ValidateSetSupportBuilding(IBuildingContainer supportBuilding)
    {
        return true;
    }


}
