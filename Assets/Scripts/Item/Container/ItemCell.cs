using System;
using Enums;
using UnityEngine;

public class ItemCell
{
    public readonly Vector2Int Position;

    public event Action<ItemCell> ContentChanged;
    
    
    public Item Item { get; private set; } = new DefaultItem();

    private int _freeSpace;
    
    public int ItemsCount { get; private set; }
    
    public ItemCell(Vector2Int position)
    {
        Position = position;
    }

    public void SetCellItem(Item item)
    {
        Item = item;
        _freeSpace = item.ItemStackCount;
        ContentChanged?.Invoke(this);
    }

    public int RemoveItemsAndReturnExcess(int count)
    {
        var excess = count;
        excess = count - ItemsCount;
        ItemsCount -= count;
        if (ItemsCount <= 0)
        {
            Clear();
        }

        ContentChanged?.Invoke(this);
        return excess;
    }

    public void Clear()
    {
        ContentChanged?.Invoke(this);
        Item = new DefaultItem();
        _freeSpace = 0;
        ItemsCount = 0;
    }
    
    public int AddItemAndReturnExcess(int count)
    {
        var settableItemsCount = count;
        var nonSettedItemsCount = count;
        if (_freeSpace < count)
        {
            nonSettedItemsCount -= _freeSpace;
            settableItemsCount = _freeSpace;
        }
        
        ItemsCount += settableItemsCount;
        nonSettedItemsCount -= settableItemsCount;
        _freeSpace -= settableItemsCount;
        
        ContentChanged?.Invoke(this);

        return nonSettedItemsCount;
    }
}

