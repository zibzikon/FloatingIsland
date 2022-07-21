using Enums;
using Units.Behaviours;
using Units.Behaviours.Diethable;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Garry : Enemy
{
    public override EnemyType EnemyType => EnemyType.Garry;
    
    private EnemyStats _enemyStats;
    
    protected override EnemyStats EnemyStats => _enemyStats;

    protected override void InitializeEnemyStats()
    {
        _enemyStats = new EnemyStats()
        {
            AttackInterval = 2,
            DamageStrength = 10,
            PreferredTargetType = TargetType.Tower,
            MinRequiredDistanceToTarget = 5,
            Health = 100,
        };
    }
    
    protected override void InitializeBahaviours()
    {
        MovingBehaviour = new TerrestrialEnemyMovingBehaviour(GetComponent<NavMeshAgent>(), EnemyStats.MinRequiredDistanceToTarget);
        AtackBehaviour = new TerrestrialEnemyAttackingBehaviour(MovingBehaviour, EnemyStats);
    }

    protected override bool TryTrackNearestTarget()
    { 
        CurrentTarget = TargetContainer.GetClosestTargetOnLayer(transform.position, EnemyStats.PreferredTargetType);
        return CurrentTarget != null;
    }
}