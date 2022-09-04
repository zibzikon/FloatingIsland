using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Factories.EnemyFactories
{
    [CreateAssetMenu(fileName = "EnemyFactory", menuName = "Factories/Enemy/EnemyFactory")]
    public class EnemyFactory : ScriptableObject
    {

        public global::Enemy Get(EnemyType enemyType, ITargetContainer targetContainer, Vector3 position)
        {
            return null;
        }

        private global::Enemy CreateEnemy(Enemy enemy, ITargetContainer targetContainer, Vector3 position)
        {
            //ToDo
            return enemy;
        }

    }
}