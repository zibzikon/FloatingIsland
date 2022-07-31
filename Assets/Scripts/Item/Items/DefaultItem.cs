
using Enums;

public class DefaultItem : Item
{
    public override ItemType ItemType => ItemType.None;
    public override int ItemStackCount { get; }

    protected override void OnItemSelected()
    {
        
    }

    protected override void OnItemUnSelected()
    {
        
    }
    
}
