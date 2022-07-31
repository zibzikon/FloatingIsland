using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

public abstract class Enemy : Entity,  IUpdatable, IPausable, IEnemyTarget, ISelectable
{
    public event Action PositionChanged;

    public DamagableType DamagableType => DamagableType.Entity;

    public TargetType TargetType => TargetType.Enemy;
    
    public Transform Transform => transform;
    
    public bool IsPaused { get; private set; }

    public bool IsDestroyed { get; private set; }

    public event Action<object> Destroyed;

    public abstract EnemyType EnemyType { get; }
    
    protected abstract EnemyStats EnemyStats { get; }
    
    protected override EntityStats Stats => EnemyStats;
    
    public event Action Damaged;
    
    protected IAtackable AtackBehaviour { get; set;}
    
    protected IMovable MovingBehaviour { get; set;}
    
    protected ITargetContainer TargetContainer { get; private set; }

    protected ITarget CurrentTarget;

    protected List<IUpdatable> ContentToUpdate = new();

    private float _attackInterval;
    
    public void Initialize(ITargetContainer targetContainer)
    {
        TargetContainer = targetContainer;
        InitializeEnemyStats();
        InitializeBehaviours();
        
        ContentToUpdate.Add(AtackBehaviour);
        ContentToUpdate.Add(MovingBehaviour);
    }

    protected abstract void InitializeEnemyStats();
    
    public virtual void TakeDamage(int count)
    {
        EnemyStats.Health -= count;
        Damaged?.Invoke();
        if (EnemyStats.Health <= 0)
        {
            Destroy();
        }
    }

    protected abstract void InitializeBehaviours();

    protected virtual void ResetBehaviours()
    {
        MovingBehaviour.Reset();
    }
    public virtual void Destroy()
    {
        IsDestroyed = true;
        ResetBehaviours();
        Destroyed?.Invoke(this);
        if (gameObject != null)
            Destroy(gameObject);
    }

    public virtual void OnUpdate()
    {
        if (IsDestroyed) throw new InvalidCastException("Enemy cannot be updated after destroying");

        if (IsPaused) return;
        
        if (MovingBehaviour.IsMoving) PositionChanged?.Invoke();

        ContentToUpdate.ForEach(updatable => updatable.OnUpdate());
        
        if (CurrentTarget == null || CurrentTarget.IsDestroyed)
        { 
            if (TryTrackNearestTarget())
            {
                CurrentTarget.Destroyed += OnTargetDestroyed;
                AtackBehaviour.SetAttackingTarget(CurrentTarget);
            }
        }
    }

    private void OnDisable()
    {
        UnsubscribeAllEvents();
    }

    protected virtual void UnsubscribeAllEvents()
    {
        if(CurrentTarget!= null) CurrentTarget.Destroyed -= OnTargetDestroyed;
    }

    
    private void OnTargetDestroyed(object sender)
    {
        CurrentTarget.Destroyed -= OnTargetDestroyed;
        CurrentTarget = null;
    }

    protected abstract bool TryTrackNearestTarget();

    public void Pause()
    {
        IsPaused = true;
    }

    public void UnPause()
    {
        IsPaused = false;
    }

    public void Select()
    {
        
    }

    public void Deselect()
    {
        
    }
}

