using System;
using Enums;
using UnityEngine;

[Serializable]
public struct EnemyChunk
{
   [SerializeField]private EnemyType _enemyType;
   public EnemyType EnemyType => _enemyType;
   [Min(1)]
   [SerializeField] private int _enemiesCount ;
   public int EnemiesCount => _enemiesCount;
   
   [Min(0)]
   [SerializeField] private float _spawnDurationAfterLastChunkSpawning;
   public float SpawnDurationAfterLastChunkSpawning => _spawnDurationAfterLastChunkSpawning;
}
