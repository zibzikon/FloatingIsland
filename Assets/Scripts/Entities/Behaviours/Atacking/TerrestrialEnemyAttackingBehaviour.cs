using System;
using UnityEngine;

namespace Units.Behaviours.Diethable
{
    public class TerrestrialEnemyAttackingBehaviour: IAtackable
    {
        private readonly IMovable _movingBehaviour;
        
        private bool _isAtacking;
        
        public bool AttackingStarted { get; private set; }

        private readonly EnemyStats _enemyStats;

        private bool _targetWasDied = true;

        public bool AttackingIsAvailable => _movingBehaviour.TargetWasReached && !_targetWasDied;
        
        private ITarget _currentTarget;

        private float _attackingInterval;
        
        
        public TerrestrialEnemyAttackingBehaviour(IMovable movingBehaviour, EnemyStats enemyStats)
        {
            _movingBehaviour = movingBehaviour;
            _enemyStats = enemyStats;
        }
        
        public void SetAttackingTarget(ITarget target)
        {
            AttackingStarted = true;
            _currentTarget = target;
            _attackingInterval = _enemyStats.AttackInterval;
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

        public void OnUpdate()
        {
            if (_currentTarget != null && _movingBehaviour.TargetWasReached && _currentTarget.IsDestroyed == false) 
            {
                _attackingInterval -= Time.deltaTime;
                if (_attackingInterval <= 0)
                {
                    DamageTarget();
                    _attackingInterval = _enemyStats.AttackInterval;
                }
            }
        }

        private void DamageTarget()
        {
            _currentTarget.TakeDamage(_enemyStats.DamageStrength);
        }
    }
}