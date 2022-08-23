using System;
using System.Collections;
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
    
    private readonly CountableItemsList _allItems = new ();

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
        return _allItems.Contains(itemType, itemsCount);
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
        var notEstablishedItemsCount = AddItemsToContainerAndReturnExcess(item, count);
        return notEstablishedItemsCount;
    }

    public bool TryRemoveItems(ItemType itemType, int count)
    {
        var excess = count;
        var possibleCellsToRemoveItems = new List<ItemCell>();
            
        foreach (var itemCell in ItemCells)
        {
            if (itemCell.Item.ItemType != itemType) continue;
            
            excess -= itemCell.ItemsCount;
            possibleCellsToRemoveItems.Add(itemCell);
            if (excess <= 0)
            {
                break;
            }
        }

        if (excess <= 0)
        {
            var itemsCountToSet = count;
            foreach (var itemCell in possibleCellsToRemoveItems)
            {
                 excess = itemCell.RemoveItemsAndReturnExcess(itemsCountToSet);
                _allItems.Remove(itemCell.Item.ItemType, itemsCountToSet - excess);
            }

            return true;
        }

        return false;
    }
    
    private int AddItemsToContainerAndReturnExcess(Item item, int count)
    {
        var itemsExcess = count;
        var freeItemCells  = new List<ItemCell>();

        foreach ( var itemCell in ItemCells)
        {
            var itemCellItemType = itemCell.Item.ItemType;
            if (itemCellItemType == item.ItemType)
            {
                itemsExcess = itemCell.AddItemAndReturnExcess(count);
                _allItems.Add(itemCellItemType, count - itemsExcess);
            }
            if (itemCellItemType == ItemType.None)
            {
                freeItemCells.Add(itemCell);
            }
        }

        foreach (var freeItemCell in freeItemCells)
        {
            if (itemsExcess > 0)
            {
                freeItemCell.SetCellItem(item);
                count = itemsExcess;
                itemsExcess = freeItemCell.AddItemAndReturnExcess(itemsExcess);
                _allItems.Add(freeItemCell.Item.ItemType, count - itemsExcess);
            }
        }
        
        return itemsExcess;
    }
    
    public Item SetItemAndReturnSetted(Item item ,Vector2Int position)
    {
         if (position.x >= Size.x || position.y >= Size.y || 
             position.x < 0 || position.y < 0) throw new IndexOutOfRangeException();

         var cell = ItemCells[position.x, position.y];
         var cellItem = cell.Item;
        
         cell.SetCellItem(item);
        
         return cellItem;
    }
    
    
    public Item GetSettedItem(Vector2Int position)
    {
        return ItemCells[position.x, position.y].Item;
    }
}

public class CountableItemsList
{
    private Dictionary<ItemType, int> _countableItems;

    public CountableItemsList()
    {
        InitializeCountableItemsDictionary();
    }

    private void InitializeCountableItemsDictionary()
    {
        _countableItems = new();
        foreach (var itemType in Enum.GetValues(typeof(ItemType)))
        {
            _countableItems.Add((ItemType)itemType, 0);
        }
    }
    
    public void Add(ItemType item, int count)
    {
        _countableItems[item] += count;
    }

    public bool Contains(ItemType itemType, int count)
    {
        return _countableItems[itemType] >= count;
    }
    
    public void Clear()
    {
        foreach (var itemType in Enum.GetValues(typeof(ItemType)))
        {
            _countableItems[(ItemType)itemType] = 0;
        }
    }

    public bool Remove(ItemType item, int count)
    {
        var itemsCount = _countableItems[item];
        
        if (itemsCount - count < 0)
        {
            return false;
        }
        
        _countableItems[item] -= count;
        return true;
    }


}
