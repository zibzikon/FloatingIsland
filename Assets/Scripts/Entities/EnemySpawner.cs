using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Extentions;
using Factories.Enemy;
using UnityEngine;

public class EnemySpawner : ITargetContainer, IUpdatable
{
    private readonly EnemyFactory _enemyFactory;
    
    private readonly PathFinder _pathFinder;
    
    private readonly GameField _gameField;
    
    private readonly List<Enemy> _enemies = new();
    
    private readonly Player _player;
    
    public EnemySpawner(GameField gameField, EnemyFactory enemyFactory,Player player)
    {
        _gameField = gameField;
        _enemyFactory = enemyFactory;
        _pathFinder = new PathFinder(gameField);
        _player = player;
    }

    public ITarget GetClosestTargetOnLayer(Vector3 startPosition, TargetType preferredTargetType)
    {
        if (preferredTargetType == TargetType.Player) return _player;
        
        var startCell = _gameField.GetCellByWorldPosition(startPosition);

        var reachableCells = new Stack<Cell>();
        reachableCells.Push(startCell);
        
        var exploredCells = new List<Cell>();
        Cell closestCellWithTarget = null;

        while (reachableCells.Count > 0)
        {
            var currentCell = reachableCells.Pop();
            exploredCells.Add(currentCell);
            
            var cellSettedBuildings = currentCell.SetedBuildings;

            var settedBuildings = cellSettedBuildings as Building[] ?? cellSettedBuildings.ToArray();
            if (settedBuildings.Any())
            {
                if (settedBuildings.FirstOrDefault(building => 
                        building.TargetType == preferredTargetType) != null)
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

        ITarget blockingTarget = closestCellWithTarget != null ? GetBlockingTarget(startCell.Position, 
            closestCellWithTarget.Position) :null;
        var playerCell = _gameField.GetCellByWorldPosition(_player.Transform.position);
        
        return blockingTarget ?? ( closestCellWithTarget?.SetedBuildings.First() ?? _player.WasDied ? null 
            :( GetBlockingTarget(startCell.Position ,playerCell.Position) ?? _player ));
    }

    private ITarget GetBlockingTarget(Vector3Int startCellPosition,Vector3Int endCellPosition)
    {
        var path = _pathFinder.FindPath(startCellPosition, endCellPosition);
        var cell = path.Pop();
        while (path.Count > 0)
        {
            if (cell.IsBlocked)
            {
                return cell.SetedBuildings.FirstOrDefault();
            }
            cell = path.Pop();
        }
        return cell.SetedBuildings.FirstOrDefault();
    }
    

    public void Initialize()
    {
        _pathFinder.Initialize();
    }

    public void SpawnEnemy()
    {
       _enemies.Add(_enemyFactory.Get(EnemyType.Garry, this, new Vector3(1,0,1)));
    }
    
    public void OnUpdate()
    {
        _enemies.ForEach(enemy=>enemy.OnUpdate());
    }
}


