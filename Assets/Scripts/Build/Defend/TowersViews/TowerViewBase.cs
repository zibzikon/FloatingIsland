using System;
using UnityEngine;

public abstract class TowerViewBase : MonoBehaviour
{
    [SerializeField] protected Transform Head;
    [SerializeField] protected Transform Boudy;
    [SerializeField] protected DefenceTower Tower;
    
    private void OnEnable()
    {
        SubscribeTowerEvents();
    }

    private void OnDisable()
    {
        UnsubscribeTowerEvents();
    }

    private void SubscribeTowerEvents()
    {
        Tower.Destroyed += OnDied;
        Tower.Damaged += OnDamaged;
        Tower.Atacking += OnAttacking;
        Tower.Attacked += OnAttacked;
    }

    private void UnsubscribeTowerEvents()
    {
        Tower.Destroyed -= OnDied;
        Tower.Damaged -= OnDamaged;
        Tower.Atacking -= OnAttacking;
        Tower.Attacked -= OnAttacked;    
    }
    
    protected abstract void OnDied(object sender);
    protected abstract void OnAttacked();
    protected abstract void OnAttacking(object args);
    protected abstract void OnDamaged();
}

