using System.Collections;
using System.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace Units.Behaviours.Diethable
{
    public class TerrestrialEnemyAtackingBehaviour: IAtackable
    {
        private readonly IMovable _movingBehaviour;
        

        private bool _isAtacking;
        
        public bool AtackingStarted { get; private set; }
        
        private readonly EnemyStats _enemyStats;

        private bool _targetWasDied;

        public TerrestrialEnemyAtackingBehaviour(IMovable movingBehaviour, EnemyStats enemyStats)
        {
            _movingBehaviour = movingBehaviour;
            _enemyStats = enemyStats;
        }
        
        public void Atack(ITarget target)
        {
            AtackingStarted = true;
            _targetWasDied = false;
             
            _movingBehaviour.MoveTo(target);
            StartAtack(target);
        }

        private async void StartAtack(ITarget target)
        {
            target.Died += OnDie;
            _targetWasDied = false;
            while (!_isAtacking)
            {
                await Task.Delay(_enemyStats.AtackInterval * 1000); 
               
                if (_targetWasDied ) return;
                if (!_movingBehaviour.TargetWasReached) continue;
                _isAtacking = true;
            }
            
            StartDamaging(target);
        }

        private async void StartDamaging(IDamagable damagable)
        {
            while (!_targetWasDied)
            {
                await Task.Delay(_enemyStats.AtackInterval * 1000);
                if (!_targetWasDied)
                {
                    damagable.Damage(_enemyStats.DamageStrength);
                }
                Debug.Log("Target was damaged");
            }
            
            damagable.Died -= OnDie;
            
            Debug.Log("reseted");

        }

        private void OnDie(object sender)
        {
            _targetWasDied = true;
            _isAtacking = false;
            AtackingStarted = false;
        }
    }
}