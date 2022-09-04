using System;
using Enums;
using UnityEngine;

public abstract class DefenceTower : Building
{
    public event Action Attacked;
    
    public event Action<object> Atacking;
    
    public bool AttackingStarted { get; set; }
    
    public override TargetType TargetType => TargetType.DefenceTower;
    
    protected override Direction2 Direction { get; set; } = Direction2.Foward;

    private ITarget _currentTarget;
    
    public abstract float AttackRadius { get; }

    public abstract EnemyType PreferredEnemyType { get; }
    
    public abstract float AttackInterval { get; }

    public abstract int AttackStrength { get; }
    
    private float _attackInterval;
    
    protected DefenceTower(IBuildingsContainer buildingsContainer) : base(buildingsContainer)
    {
    }
    
    public abstract void Attack(ITarget target);

    public override bool ValidateSetSupportBuilding(IBuildingContainer supportBuilding)
    {
        return true;
    }
    
    private bool TryTrackNearestTarget()
    {
        var position = Transform.Position;
        ITarget nearestTarget = null; 
        ITarget nearestPreferredTarget = null; 
        var distance = Mathf.Infinity;
        var colliders = new Collider[5];
        Physics.OverlapSphereNonAlloc(Transform.Position, AttackRadius, colliders);
        foreach (var collider in colliders)
        {
            var possibleTarget = collider.GetComponent<CollisionObject>();
            
            if (possibleTarget == null ) continue;
            var target = possibleTarget.Parent.GetComponent<IEnemyTarget>();
            if (target == null ) continue;
            
            var distanceToTarget = Vector3.Distance(position, target.Transform.Position);
            if (!(distance > distanceToTarget)) continue;
            
            distance = distanceToTarget;
            nearestTarget = target;
            
            if (target.EnemyType == PreferredEnemyType)
            {
                nearestPreferredTarget = target;
            }
        }

        _currentTarget = nearestPreferredTarget ?? nearestTarget;
        
        return _currentTarget != null;
    }

    public override void OnUpdate()
    {
        if (_currentTarget == null)
        { 
            if (TryTrackNearestTarget())
            {
                _attackInterval = AttackInterval;
            }
        }

        if (_currentTarget != null)
        {
            if (!IsTargetTracked())
            {
                ResetTarget();
                return;
            }
            
            _attackInterval -= Time.deltaTime;
            Atacking?.Invoke(_currentTarget);
            
            if (_attackInterval <= 0)
            {
                _attackInterval = AttackInterval;
                Attack(_currentTarget);
                Attacked?.Invoke();
                Debug.Log("Target enemy is damaged");
            }
        }
    }

    private void OnDisable()
    {
        UnsubscribeAllEvents();
    }

    protected virtual void UnsubscribeAllEvents()
    {
    }

    private void ResetTarget()
    {
        _currentTarget = null;
        _attackInterval = 0;
        AttackingStarted = false;
        Debug.Log($"In Tower{PositionOnGameField}Target Reseted");
    }

    private bool IsTargetTracked()
    {
        if (_currentTarget == null || _currentTarget.IsDestroyed) return false;

        var position = Transform.Position;

        var distanceToTarget = Vector3.Distance(position, _currentTarget.Transform.Position);

        return distanceToTarget < AttackRadius;
    }
}
