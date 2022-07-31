using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.EnemySpawner
{
    [CreateAssetMenu(fileName = "EnemiesWavesContainer", menuName = "Factories/Enemy/EnemiesWavesContainer")]
    public class EnemiesWavesContainer : ScriptableObject
    {
        [SerializeField] private EnemiesWave[] _enemiesWaves; 
        public EnemiesWave GetRandomWaveByGeneralGameDifficulty()
        {
            if (_enemiesWaves.Any() == false) throw new NullReferenceException();

            var randomIndex = Random.Range(0, _enemiesWaves.Length);
            return _enemiesWaves[randomIndex];
        }
    }
}