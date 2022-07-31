using System;
using Enums;

public abstract class Item : IDestroyable
{
    public abstract ItemType ItemType { get; }

    public abstract int ItemStackCount { get; }
    
    public event Action<Item> ItemSelected;
    
    public event Action ItemsCountChanged;
    
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

    public virtual void Damage(IDamagable damagable)
    {
        switch (damagable.DamagableType)
        {
            case DamagableType.Wooden: damagable.TakeDamage(1);
                break;
            case DamagableType.Stone: damagable.TakeDamage(1);
                break;
            case DamagableType.Entity: damagable.TakeDamage(1);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }
}
