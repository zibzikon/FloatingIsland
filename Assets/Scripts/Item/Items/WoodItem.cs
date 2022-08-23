
using Enums;

public class WoodItem : Item
{
    public override ItemType ItemType => ItemType.Wood;

    public override int ItemStackCount => 24;

    protected override void OnItemSelected()
    {
    }

    protected override void OnItemUnSelected()
    {
    }
}
