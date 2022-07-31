using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesWave", menuName = "Factories/Enemy/EnemiesWave")]

public class EnemiesWave : ScriptableObject
{
    [SerializeField] private EnemyChunk[] _enemyChunksWave;
    public IEnumerable<EnemyChunk> EnemyChunksWave => _enemyChunksWave;
  
}