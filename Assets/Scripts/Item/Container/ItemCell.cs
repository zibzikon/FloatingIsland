using System;
using UnityEngine;

public class ItemCell
{
    public readonly Vector2Int Position;

    public event Action<ItemCell> ContentChanged;
    public Item Content { get; private set; }

    public ItemCell(Vector2Int position, Item content = null)
    {
        SetCellContent(content);
        Position = position;
    }

    public void SetCellContent(Item content)
    {
        Content = content;
        ContentChanged?.Invoke(this);
    }
}

