using System;
using System.Threading.Tasks;
using Enums;
using UnityEngine;

public class Turret : DefenceTower
{
    public override BuildingType BuildingType => BuildingType.Turret;
    protected override TowerStats TowerStats { get; } = new TowerStats
    {
        Health = 100,
        AttackRadius = 10,
        AttackStrength = 40,
        AttackInterval = 1f,
        PreferredEnemyType = EnemyType.None
    };
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, TowerStats.AttackRadius);
    }


    public override void Attack(ITarget target)
    {
        target?.Damage(TowerStats.AttackStrength);
    }
}
