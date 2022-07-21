using System;
using Enums;
using UnityEngine;

public abstract class DefenceTower : Building
{
    public event Action Attacked;
    
    public event Action<object> Atacking;
    
    public bool AttackingStarted { get; set; }

    protected abstract TowerStats TowerStats { get; }
    
    public override int Weight { get; }

    protected override BuildingStats BuildingStats => TowerStats;
    
    public override TargetType TargetType => TargetType.DefenceTower;
    
    protected override Direction2 Direction { get; set; } = Direction2.Foward;

    private ITarget _currentTarget;

    private float _attackInterval;
    public override bool ValidateSetSupportBuilding(IBuildingContainer supportBuilding)
    {
        return true;
    }

    public override void Initialize()
    {
        base.Initialize();
    }
    
    
    private bool TryTrackNearestTarget()
    {
        var position = transform.position;
        ITarget nearestTarget = null; 
        ITarget nearestPreferredTarget = null; 
        var distance = Mathf.Infinity;
        
        foreach (var collider in Physics.OverlapSphere(transform.position, TowerStats.AttackRadius))
        {
            var possibleTarget = collider.GetComponent<CollisionObject>();
            
            if (possibleTarget == null ) continue;
            var target = possibleTarget.Parent.GetComponent<IEnemyTarget>();
            if (target == null ) continue;
            
            var distanceToTarget = Vector3.Distance(position, target.Transform.position);
            if (!(distance > distanceToTarget)) continue;
            
            distance = distanceToTarget;
            nearestTarget = target;
            
            if (target.EnemyType == TowerStats.PreferredEnemyType)
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
                _attackInterval = TowerStats.AttackInterval;
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
                _attackInterval = TowerStats.AttackInterval;
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

        var position = transform.position;

        var distanceToTarget = Vector3.Distance(position, _currentTarget.Transform.position);

        return distanceToTarget < TowerStats.AttackRadius;
    }

    public abstract void Attack(ITarget target);

}
