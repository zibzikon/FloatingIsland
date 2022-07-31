using System;
using System.Collections.Generic;
using Enums;
using Factories.Item;
using UnityEngine;


public abstract class ItemsContainer
{
    public enum ContainerState
    {
        Opened,
        Closed
    }
    
    public event Action Opened; 
    
    public event Action Closed; 

    public abstract Vector2Int Size { get; }
    
    private readonly ItemsFactory _itemsFactory = new();
    
    public ItemCell[,] ItemCells { get; private set; }
    
    private readonly List<CountableItem> _allItems = new ();

    public ContainerState CurrentState { get; private set; }
    
    protected void Initialize()
    {
        ItemCells = new ItemCell[Size.x, Size.y];
       
        for (var x = 0; x < Size.x; x++)
        {
            for (var y = 0; y < Size.y; y++)
            {
                ItemCells[x, y] = new ItemCell(new Vector2Int(x, y));
            }
        }
    }

    public bool ContainsItem(ItemType itemType, int itemsCount)
    {
        foreach (var item in _allItems)
        {
            if (item.ItemType == itemType && item.ItemsCount >= itemsCount)
            {
                return true;
            }
        }

        return false;
    }
    
    public void Open()
    { 
        OnOpen();
        Opened?.Invoke();
        CurrentState = ContainerState.Opened;
    }
   
    protected abstract void OnOpen();

    public void Close()
    { 
        OnClose();
        Closed?.Invoke();
        CurrentState = ContainerState.Closed;
    }
   
    protected abstract void OnClose();
    
    public int CreateAndAddItemsToContainerAndReturnNotEstablished(ItemType itemType, int count)
    {
        var item = _itemsFactory.Get(itemType);
        var notEstablishedItemsCount = AddItemsToContainerAndReturnNotEstablished(item, count);
        return notEstablishedItemsCount;
    }

    public bool TryRemoveItems(ItemType itemType, int count)
    {
        var excess = count;
        var possibleCellsToRemoveItems = new List<ItemCell>();
        foreach (var itemCell in ItemCells)
        {
            if (itemCell.Item.ItemType == itemType)
            {
                excess -= itemCell.ItemsCount;
                possibleCellsToRemoveItems.Add(itemCell);
                if (excess <= 0)
                {
                    break;
                }
            }
        }

        if (excess <= 0)
        {
            var itemsCountToSet = count;
            foreach (var itemCell in possibleCellsToRemoveItems)
            {
                itemsCountToSet = itemCell.RemoveItemsAndReturnExcess(itemsCountToSet);
            }
        }

        return false;
    }
    
    private int AddItemsToContainerAndReturnNotEstablished(Item item, int count)
    {
        var notEstablishedItemsCount = count;
        var freeItemCells  = new List<ItemCell>();

        foreach ( var itemCell in ItemCells)
        {
            var itemCellItemType = itemCell.Item.ItemType;
            if (itemCellItemType == item.ItemType)
            {
                notEstablishedItemsCount = itemCell.AddItemAndReturnNonSetted(count);
            }
            if (itemCellItemType == ItemType.None)
            {
                freeItemCells.Add(itemCell);
            }
        }

        foreach (var freeItemCell in freeItemCells)
        {
            if (notEstablishedItemsCount > 0)
            {
                freeItemCell.SetCellItem(item);
                notEstablishedItemsCount = freeItemCell.AddItemAndReturnNonSetted(notEstablishedItemsCount);
            }
        }
        
        return notEstablishedItemsCount;
    }
    
    public Item SetItemAndReturnSetted(Item item ,Vector2Int position)
    {
         if (position.x >= Size.x || position.y >= Size.y || 
             position.x < 0 || position.y < 0) throw new IndexOutOfRangeException();
        
         var cell = ItemCells[position.x, position.y];
        
         cell.SetCellItem(item);
        
         return cell.Item;
    }
    
    
    public Item GetSettedItem(Vector2Int position)
    {
        return ItemCells[position.x, position.y].Item;
    }
}
