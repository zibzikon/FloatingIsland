
public abstract class BuildingItem : Item, IBuildingItem
{
    protected abstract BuildingType BuildingType { get; }
    protected override void OnItemSelected()
    {
        
    }

    protected override void OnItemUnSelected()
    {
        
    }

    public void Build(IBuilder builder)
    {
        builder.Build(BuildingType);
        Destroy();
    }
}
