using System;
using System.Collections.Generic;
using System.Linq;
using Entities.EnemySpawner;
using Enums;
using Extentions;
using Factories.EnemyFactories;
using UnityEngine;

public class EnemySpawner : ITargetContainer, IUpdatable
{
    private readonly EnemyFactory _enemyFactory;
    
    private readonly PathFinder _pathFinder;
    
    private readonly GameField _gameField;
    
    private readonly List<Enemy> _enemies = new();
    
    private readonly List<IUpdatable> _contentToUpdate = new();

    private readonly EnemiesWavesContainer _enemiesWavesContainer;
    
    private readonly Player _player;

    private readonly Timer _timer = new ();

    private Queue<EnemyChunk> _enemyChunksWave;    
    
    private bool _waveStarted;

    public EnemySpawner(GameField gameField, EnemyFactory enemyFactory, EnemiesWavesContainer enemiesWavesContainer, Player player)
    {
        _gameField = gameField;
        _enemyFactory = enemyFactory;
        _pathFinder = new PathFinder(gameField);
        _enemiesWavesContainer = enemiesWavesContainer;
        _player = player;
    }

    public ITarget GetClosestTargetOnLayer(Vector3 startPosition, TargetType preferredTargetType)
    {
        if (preferredTargetType == TargetType.Player) return _player;
        
        var startCell = _gameField.GetCellByPosition(_gameField.ConvertScreenToGameFieldPosition(startPosition));

        var reachableCells = new Stack<Cell>();
        reachableCells.Push(startCell);
        
        var exploredCells = new List<Cell>();
        Cell closestCellWithTarget = null;

        while (reachableCells.Count > 0)
        {
            var currentCell = reachableCells.Pop();
            exploredCells.Add(currentCell);
            var placedBuilding = currentCell.PlacedBuilding;
            if (currentCell.IsBlocked)
            {
                if(placedBuilding.TargetType == preferredTargetType) 
                    return GetBlockingTarget(startCell.Position, currentCell.Position);
                
                closestCellWithTarget ??= currentCell;
            }

            var newReachableCells = currentCell.Neighbours.ToNeighbors2().ToEnumerable();

            foreach (var cell in newReachableCells)
            {
                if (exploredCells.Contains(cell)) continue;

                if (!reachableCells.Contains(cell))
                {
                    reachableCells.Push(cell);
                }
            }

        }

        var blockingTarget = closestCellWithTarget != null ? GetBlockingTarget(startCell.Position, 
            closestCellWithTarget.Position) :null;
        var playerCell = _gameField.GetCellByPosition(_gameField.ConvertScreenToGameFieldPosition(_player.Transform.Position));
        
        return blockingTarget ?? ( closestCellWithTarget?.PlacedBuilding is not null && _player.IsDestroyed ? null 
            : GetBlockingTarget(startCell.Position ,playerCell.Position) ?? _player );
    }

    private ITarget GetBlockingTarget(Vector3Int startCellPosition,Vector3Int endCellPosition)
    {
        var path = _pathFinder.FindPath(startCellPosition, endCellPosition);
        var cell = path.Pop();
        while (path.Count > 0)
        {
            if (cell.IsBlocked)
            {
                return cell.PlacedBuilding;
            }
            cell = path.Pop();
        }
        return cell.PlacedBuilding;
    }

    public void Initialize()
    {
        _pathFinder.Initialize();
    }

    public void SpawnEnemy(EnemyType enemyType, Vector3 position)
    {
        var enemy = _enemyFactory.Get(enemyType, this, position);
        enemy.Destroyed += OnEnemyDied;
        
       _enemies.Add(enemy);
       _contentToUpdate.Add(enemy);
    }

    private void OnEnemyDied(object sender)
    {
        var enemy = (Enemy)sender;
        if (enemy == null) throw new InvalidOperationException();
        
        _enemies.Remove(enemy);
        _contentToUpdate.Remove(enemy);
        enemy.Destroyed -= OnEnemyDied;
    }
    
    public void OnUpdate()
    {
        _contentToUpdate.ForEach(updatable => updatable.OnUpdate());
        
        if (!_waveStarted)
        {
            {
                // todo
                SpawnWave(_enemiesWavesContainer.GetRandomWaveByGeneralGameDifficulty());
            }
        }
        else if (_waveStarted)
        {
            OnWaveStarted();
        }
    }

    private void SpawnWave(EnemiesWave enemiesWave)
    {
        _waveStarted = true;
        
        _enemyChunksWave = new Queue<EnemyChunk>();
        foreach (var enemyChunk in enemiesWave.EnemyChunksWave)
        {
            _enemyChunksWave.Enqueue(enemyChunk);
        }
    }

    private void SpawnEnemyChunk(EnemyChunk enemyChunk)
    {
        for (int i = 0; i < enemyChunk.EnemiesCount; i++)
        {
            SpawnEnemy(enemyChunk.EnemyType, new Vector3(1, 0, 1));
        }
    }

    private void OnWaveStarted()
    {
        {
            //ToDo
            var enemyChunk = _enemyChunksWave.Dequeue();
            SpawnEnemyChunk(enemyChunk);
        }
    }
    
    private float GetRandomEnemiesWaveSpawningTime()
    {
        return 0;
    }
}


