using System;
using Enums;
using UnityEngine;

namespace Factories.Enemy
{
    [CreateAssetMenu(fileName = "EnemyFactory", menuName = "Factories/Enemy/EnemyFactory")]
    public class EnemyFactory : ScriptableObject
    {
        [SerializeField] private global::Garry _garry;
        
        public global::Enemy Get(EnemyType enemyType, ITargetContainer targetContainer, Vector3 position)
        {
            return enemyType switch
            {
                EnemyType.Garry => CreateEnemy(_garry, targetContainer, position),
                _ => throw new IndexOutOfRangeException()
            };
        }

        private global::Enemy CreateEnemy(global::Enemy enemy, ITargetContainer targetContainer, Vector3 position)
        {
            var instance = Instantiate(enemy, position, Quaternion.identity);
            instance.Initialize(targetContainer);
            return instance;
        }
    }
}