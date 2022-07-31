using System;
using Enums;
using UnityEngine;

public class ItemCell
{
    public readonly Vector2Int Position;

    public event Action<ItemCell> ContentChanged;
    public Item Item { get; private set; } = new DefaultItem();

    private int FreeSpace;
    public int ItemsCount { get; private set; }
    
    public ItemCell(Vector2Int position)
    {
        Position = position;
    }

    public void SetCellItem(Item item)
    {
        Item = item;
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

        return excess;
    }

    public void Clear()
    {
        
    }
    
    public int AddItemAndReturnNonSetted(int count)
    {
        var settableItemsCount = count;
        var nonSettedItemsCount = count;
        if (FreeSpace < count)
        {
            nonSettedItemsCount -= FreeSpace;
            settableItemsCount = FreeSpace;
        }
        
        ItemsCount = settableItemsCount;
        FreeSpace -= settableItemsCount;
        
        return nonSettedItemsCount;
    }
}

