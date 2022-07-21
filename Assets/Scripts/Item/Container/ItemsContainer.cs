using System;
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

    protected ItemCell[,] ItemCells;
    
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
   
    public Item SetItemAndReturnSetted(Item item ,Vector2Int position)
    {
         if (position.x >= Size.x || position.y >= Size.y || 
             position.x < 0 || position.y < 0) throw new IndexOutOfRangeException();
        
         var cell = ItemCells[position.x, position.y];
        
         cell.SetCellContent(item);
        
         return cell.Content;
    }

    public Item GetSettedItem(Vector2Int position)
    {
        return ItemCells[position.x, position.y].Content;
    }
}
