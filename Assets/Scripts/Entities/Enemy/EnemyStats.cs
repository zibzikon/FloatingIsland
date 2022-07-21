
using Enums;

public class EnemyStats: EntityStats
{
    public int AttackInterval { get; set; }
    
    public int DamageStrength { get; set; } 

    public float MinRequiredDistanceToTarget { get; set; } 
    
    public TargetType PreferredTargetType { get; set; }
}

