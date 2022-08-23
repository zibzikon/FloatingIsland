using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Factories.EnemyFactories
{
    [CreateAssetMenu(fileName = "EnemyFactory", menuName = "Factories/Enemy/EnemyFactory")]
    public class EnemyFactory : ScriptableObject
    {
        [SerializeField] private List<Enemy> _enemies;

        private Dictionary<EnemyType, Enemy> _enemiesDictionary;
        
        public void Initialize()
        {
            _enemiesDictionary = new();
            foreach (var enemy in _enemies)
            {
                _enemiesDictionary.Add(enemy.EnemyType, enemy);
            }
        }
        
        public global::Enemy Get(EnemyType enemyType, ITargetContainer targetContainer, Vector3 position)
        {
            return CreateEnemy(_enemiesDictionary[enemyType], targetContainer, position);
        }

        private global::Enemy CreateEnemy(Enemy enemy, ITargetContainer targetContainer, Vector3 position)
        {
            var instance = Instantiate(enemy, position, Quaternion.identity);
            instance.Initialize(targetContainer);
            return instance;
        }

    }
}