using Enums;

public class WoodenAxe : Axe
{
    public override ItemType ItemType => ItemType.WoodenAxe;
    
    protected override int WoodenBuildingDamageStrength => 20;

    protected override int EntityDamageStrength => 40;

    protected override void OnItemSelected()
    {

    }

    protected override void OnItemUnSelected()
    {

    }
}

