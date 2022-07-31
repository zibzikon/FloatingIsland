
public abstract class Axe : Instrument
{
    protected abstract int WoodenBuildingDamageStrength { get; }
    protected abstract int EntityDamageStrength { get; }
    
    public override void DamageWoodenBuilding(IDamagable damagable)
    {
        damagable.TakeDamage(WoodenBuildingDamageStrength);
    }

    public override void DamageStoneBuilding(IDamagable damagable)
    {
        damagable.TakeDamage(1);
    }

    public override void DamageEntity(IDamagable damagable)
    {
        damagable.TakeDamage(EntityDamageStrength);
    }
}
