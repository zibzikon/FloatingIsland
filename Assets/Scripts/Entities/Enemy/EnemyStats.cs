
public class EnemyStats: EntityStats
{
    public int AtackInterval { get; set; } = 3;
    
    public int DamageStrength { get; set; } = 50;

    public float MinRequiredDistanceToTarget { get; set; } = 5f;
}

