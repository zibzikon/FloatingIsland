using System;
using Enums;

public abstract class Item : IDestroyable
{
    public abstract ItemType ItemType { get; }

    public event Action<Item> ItemSelected;
    
    public event Action<Item> ItemUnSelected;
    
    public event Action<object> Destroyed;

    public bool IsDestroyed { get; }

    public void Select()
    {
        ItemSelected?.Invoke(this);
        OnItemSelected();
    }

    public void UnSelect()
    {
        ItemUnSelected?.Invoke(this);
        OnItemUnSelected();
    }

    protected abstract void OnItemSelected();
    
    protected abstract void OnItemUnSelected();



    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }
}
