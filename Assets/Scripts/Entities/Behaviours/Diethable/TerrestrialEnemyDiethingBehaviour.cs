using System;
using UnityEngine.Events;

namespace Units.Behaviours.Diethable
{
    public class TerrestrialEnemyDiethingBehaviour : IDiethable
    {
        public event Action<object> Died;

        public void Die()
        {
            
        }
    }
}