using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enums;
using UnityEngine;

public class Turret : DefenceTower , IInteractable
{
    public override List<OccupyingCell> OccupyingCells { get; }
    public override BuildingType BuildingType => BuildingType.Turret;
    
    public override int Health { get; protected set; }
    
    public override DamagableType DamagableType => DamagableType.Stone;

    public override float AttackRadius { get; }
    
    public override EnemyType PreferredEnemyType { get; }
    
    public override float AttackInterval { get; }
    
    public override int AttackStrength { get; }

    public Turret(IBuildingsContainer buildingsContainer) : base(buildingsContainer)
    {
    }

    public override void Attack(ITarget target)
    {
        target?.TakeDamage(AttackStrength);
    }

    public void Interact(IInteractor sender)
    {
        
    }


}
