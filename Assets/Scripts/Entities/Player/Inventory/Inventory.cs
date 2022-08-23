using UnityEngine;

public class Inventory : ItemsContainer
{
    public override Vector2Int Size => new(4,5);

    private IBuilder _builder;
    
    private Item _selectedItem;
    
    public Inventory(IBuilder builder)
    {
        Initialize();
        _builder = builder;

        foreach (var itemCell in ItemCells)
        {
            itemCell.ContentChanged += OnCellContentChanged;
            
            if (itemCell.Item != null)
            {
                itemCell.Item.ItemSelected += OnItemSelected;
            }
        }
    }

    private void OnCellContentChanged(ItemCell sender)
    {
        sender.Item.ItemSelected += OnItemSelected;
    }

    private void OnItemSelected(Item sender)
    {
        _selectedItem = sender;
        
        if (_selectedItem is IBuildingItem selectedBuildingItem)
        {
            selectedBuildingItem.TryBuild(_builder);  
        }
        
        Close();
    }

    protected override void OnOpen()
    {
    }

    protected override void OnClose()
    {
    }

}
