
using System;
using Enums;

public abstract class Instrument : Item
{
    public override int ItemStackCount => 1;
    
    public abstract void DamageWoodenBuilding(IDamagable damagable);
    
    public abstract void DamageStoneBuilding(IDamagable damagable);
    
    public abstract void DamageEntity(IDamagable damagable);

    public override void Damage(IDamagable damagable)
    {
        switch (damagable.DamagableType)
        {
            case DamagableType.Wooden: DamageWoodenBuilding(damagable);
                break;
            case DamagableType.Stone: DamageStoneBuilding(damagable);
                break;
            case DamagableType.Entity: DamageEntity(damagable);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
