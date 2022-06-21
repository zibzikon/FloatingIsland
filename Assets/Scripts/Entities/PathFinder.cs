using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Extentions;
using UnityEngine;


public class PathFinder
{
    private PathFindingField _pathFindingField;

    public PathFinder(GameField _gameField)
    {
        _pathFindingField = new PathFindingField(_gameField);
    }
    
    public void Initialize()
    {
        _pathFindingField.GeneratePathFindingField();
    }
    
    public Stack<Cell> FindPath(Vector3Int startCellPosition, Vector3Int endCellPosition)
    {
        var startCell = _pathFindingField.GetPathCellByPosition(startCellPosition);
        var endCell = _pathFindingField.GetPathCellByPosition(endCellPosition);
        startCell.Cost = 0;
        
        var reachableCells = new List<PathCell>() {startCell};
        var exploredCells = new List<PathCell>();

        while (reachableCells.Count > 0)
        {
            var currentCell = ChooseCell();

            if (currentCell == endCell)
            {
                return BuildPath(currentCell);
            }

            reachableCells.Remove(currentCell);
            exploredCells.Add(currentCell);

            var newReachableCells = currentCell.Neighbors.ToNeighbors2().ToEnumerable();

            foreach (var cell in newReachableCells)
            {
                if (exploredCells.Contains(cell)) continue;

                if (!reachableCells.Contains(cell))
                {
                    reachableCells.Add(cell);
                }

                if (currentCell.Cost + 1 >= cell.Cost) continue;
                cell.PreviousCell = currentCell;
                cell.Cost = currentCell.Cost + 1;
            }

        }
        
        PathCell ChooseCell()
        {
            var minCost = Mathf.Infinity;
            PathCell bestCell = reachableCells[0];

            foreach (var cell in reachableCells)
            {
                var fullCost = cell.GetFullCost(endCell);
                if (minCost > fullCost)
                {
                    minCost = fullCost;
                    bestCell = cell;
                }
            }

            return bestCell;
        }
        return null;
    } 
    
    private Stack<Cell> BuildPath(PathCell cell)
    {
        var path = new Stack<Cell>();
        var currentCell = cell;
        while (currentCell != null)
        {
            path.Push(currentCell.GameFieldCell);
            currentCell = currentCell.PreviousCell;
        }

        return path;
    }
}

internal class PathFindingField
{
    private readonly GameField _gameField;
    
    private PathCell[,,] _pathCells;

    public PathFindingField (GameField gameField)
    {
        _gameField = gameField;
    }
    
    public void GeneratePathFindingField()
    {
        var size = _gameField.Size;
        
        _pathCells = new PathCell[size.x, size.y, size.z];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    var currentCell = _pathCells[x, y, z] =
                        new PathCell(_gameField.GetCellByPosition(new Vector3Int(x, y, z)));
                    
                    if (y > 0)
                    {
                        PathCell.SetUpDownNeighbours(currentCell, _pathCells[x, y - 1, z]);
                    }

                    if (z > 0)
                    {
                        PathCell.SetFowardBackNeighbours(currentCell, _pathCells[x, y, z - 1]);
                    }

                    if (x > 0)
                    {
                        PathCell.SetRightLeftNeighbours(currentCell, _pathCells[x - 1, y, z]);
                    }
                }
            }
        }
    }

    public PathCell GetPathCellByPosition(Vector3Int position)
    {
        return _pathCells[position.x, position.y, position.z];
    }
}

internal class PathCell
{
    public Neighbors3<PathCell> Neighbors { get; } = new Neighbors3<PathCell>();

    public readonly Cell GameFieldCell;

    public PathCell PreviousCell;
    
    public int Cost = Int32.MaxValue;
    
    private readonly Vector3Int _position;

    public PathCell(Cell gameFieldCell)
    {
        _position = gameFieldCell.Position;
        GameFieldCell = gameFieldCell;
    }
    
    public int GetFullCost(PathCell endCell)
    {
        var length = (int)(_position - endCell._position).magnitude;
        return Cost * GetCellMultiplier() + length;
    }

    private int GetCellMultiplier()
    {
        return GameFieldCell.IsBlocked ? 4 : 1;
    }
    
    public static void SetRightLeftNeighbours(PathCell right, PathCell left)
    {
        right.Neighbors.Left = left;
        left.Neighbors.Right = right;
    }
    
    public static void SetFowardBackNeighbours(PathCell foward, PathCell back)
    {
        foward.Neighbors.Back = back;
        back.Neighbors.Foward = foward;
    }
    
    public static void SetUpDownNeighbours(PathCell up, PathCell down)
    {
        up.Neighbors.Down = down;
        down.Neighbors.Up = up;
    }
}

