using System.Collections.Generic;
using Factories.Item.View;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public abstract class ContainerView : MonoBehaviour , ISwitchable
{
    [SerializeField] private GridLayout _itemsContainerLayout;
  
    [SerializeField] private ItemCellView _itemCellViewPrefab;

    [SerializeField] private ItemsSpriteFactory _itemsSpriteFactory;
    protected abstract ItemsContainer ItemsContainerModel { get; }

    protected ItemCellView[,] ItemCellViews;

    [SerializeField] private List<MonoBehaviour> _visualItems;

    private readonly List<ISwitchable> _contentToSwitch = new();

    protected void Initialize()
    {
        var size = ItemsContainerModel.Size;
        
        _itemsContainerLayout.Initialize(size);
        
        ItemCellViews = new ItemCellView[size.x, size.y];
        GenerateContainer();
        
        switch (ItemsContainerModel.CurrentState)
        {
            case ItemsContainer.ContainerState.Opened: Enable();
                break;
            case ItemsContainer.ContainerState.Closed: Disable();
                break;
        }
    }
    

    private void GenerateContainer()
    {
        for (var x = 0; x < ItemsContainerModel.Size.x; x++)
        {
            for (var y = 0; y < ItemsContainerModel.Size.y; y++)
            {
                var item = ItemsContainerModel.GetSettedItem(new Vector2Int(x, y));
                var itemCell = ItemsContainerModel.ItemCells[x,y];

                var cellView = ItemCellViews[x, y] = Instantiate(_itemCellViewPrefab, _itemsContainerLayout.transform);
                cellView.Initialize(itemCell, _itemsSpriteFactory);
                _contentToSwitch.Add(cellView);
                _itemsContainerLayout.SetElement(cellView.RectTransform, new Vector2Int(x, y));
            }
        }
    }

    public virtual void Enable()
    {
        _contentToSwitch.ForEach(switchable=> switchable.Enable());
        _visualItems.ForEach(x => x.enabled = true);
        _itemsContainerLayout.enabled = true;
    }

    public virtual void Disable()
    {
        _contentToSwitch.ForEach(switchable=> switchable.Disable());
        _visualItems.ForEach(x => x.enabled = false);
        _itemsContainerLayout.enabled = false;
    }
}
