using Enums;
using Units.Behaviours;
using Units.Behaviours.Diethable;
using UnityEngine;
using UnityEngine.AI;

public class Garry : Enemy
{
    public override EnemyType EnemyType => EnemyType.Garry;
    public override int Health { get; protected set; } = 100;

    public override float AttackInterval { get; protected set; } = 4f;

    public override int DamageStrength { get; protected set; } = 5;

    public override TargetType PreferredTargetType { get; protected set; } = TargetType.Tower;

    public override float MinRequiredDistanceToTarget { get; protected set; } = 5f;

    protected override void InitializeBehaviours()
    {
       //ToDo
    }

    protected override bool TryTrackNearestTarget()
    { 
        CurrentTarget = TargetContainer.GetClosestTargetOnLayer(Transform.Position, PreferredTargetType);
        return CurrentTarget != null;
    }
}