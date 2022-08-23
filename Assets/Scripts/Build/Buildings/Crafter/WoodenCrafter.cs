
using System.Collections.Generic;
using Enums;

public class WoodenCrafter : Crafter
{
    public override int Weight => 100;

    protected override List<CraftItem> AllCraftItems { get; } = new()
    {
        new (new CountableItem(ItemType.Wood, 24),
        new List<CountableItem> { new(ItemType.Wood, 1) }) ,
        
    };
    
    protected override BuildingStats BuildingStats { get; } = new()
    {
        DropItems = new List<CountableItem>() { new(ItemType.Wood, 3) },
        Health = 20
    };

    public override TargetType TargetType => TargetType.None;
    
    public override BuildingType BuildingType => BuildingType.WoodenCrafter;
    
    public override DamagableType DamagableType => DamagableType.Wooden;
    
    protected override Direction2 Direction { get; set; }
    
    public override bool ValidateSetSupportBuilding(IBuildingContainer supportBuilding)
    {
        return true;
    }

}
