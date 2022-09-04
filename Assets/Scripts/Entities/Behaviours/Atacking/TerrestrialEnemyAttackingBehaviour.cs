using System;
using UnityEngine;

namespace Units.Behaviours.Diethable
{
    public class TerrestrialEnemyAttackingBehaviour : IAtackable
    {
        private readonly IMovable _movingBehaviour;
        
        private bool _isAtacking;
        
        public bool AttackingStarted { get; private set; }

        private bool _targetWasDied = true;

        public bool AttackingIsAvailable => _movingBehaviour.TargetWasReached && !_targetWasDied;
        
        private ITarget _currentTarget;

        private float _attackingInterval;
        
        private readonly int _damageStrength;

        private Timer _timer;
        
        
        public TerrestrialEnemyAttackingBehaviour(IMovable movingBehaviour, float attackingInterval, int damageStrength)
        {
            _attackingInterval = attackingInterval;
            _damageStrength = damageStrength;
            _movingBehaviour = movingBehaviour;
        }
        
        public void SetAttackingTarget(ITarget target)
        {
            AttackingStarted = true;
            _currentTarget = target;
            _movingBehaviour.SetTarget(target);
            target.Destroyed += OnTargetDestroyed;
        }

        private void OnTargetDestroyed(object sender)
        {
            if (sender is not ITarget target) throw new InvalidOperationException();
            
            target.Destroyed += OnTargetDestroyed;
            _isAtacking = false;
            AttackingStarted = false;
            _currentTarget = null;
        }

        private async void StartAttacking()
        {
        }
        
     
        private void DamageTarget()
        {
            _currentTarget.TakeDamage(_damageStrength);
        }
    }
}