using Enums;
using Units.Behaviours;
using Units.Behaviours.Diethable;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Garry : Enemy
{
    protected override EnemyStats EnemyStats { get; } = new EnemyStats();
    
    protected override TargetType PreferredTargetType => TargetType.Tower;
        
    protected override IDiethable DieBehaviour { get; set; } 

    protected override IAtackable AtackBehaviour { get; set; }

    protected override IMovable MovingBehaviour { get; set; }

    protected override void InitializeBahaviours()
    {
        MovingBehaviour = new TerrestrialEnemyMovingBehaviour(GetComponent<NavMeshAgent>(), EnemyStats.MinRequiredDistanceToTarget);
        AtackBehaviour = new TerrestrialEnemyAtackingBehaviour(MovingBehaviour, EnemyStats);
        DieBehaviour = new TerrestrialEnemyDiethingBehaviour();
    }

    protected override ITarget GetTarget()
    { 
        return TargetContainer.GetClosestTargetOnLayer(transform.position, PreferredTargetType);
    }
}
