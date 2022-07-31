
public abstract class BuildingItem : Item, IBuildingItem
{
    protected abstract BuildingType BuildingType { get; }
    protected override void OnItemSelected()
    {
        
    }

    protected override void OnItemUnSelected()
    {
        
    }

    public bool TryBuild(IBuilder builder)
    {
      
        builder.Build(BuildingType);
        return true;
    }
}
