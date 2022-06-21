
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameOfLife : MonoBehaviour
{
    [SerializeField] private GameOfLifeCellView _cellPefab;
    [SerializeField] private Size _size;
    [SerializeField] private float timeBetweenGenerations;

    [SerializeField] private Button StartStopButton;
    [SerializeField] private Button GoNextGenerationButton;
    private bool _simulationVasStarted;
    private GameOfLifeBoard _gameBoard;
    void Start()
    {
        _gameBoard = new GameOfLifeBoard(_size);
        _gameBoard.Generate();
        StartStopButton.onClick.AddListener(OnStartStopButtonPressed);
        GoNextGenerationButton.onClick.AddListener(GoNextGeneration);
        GenerateCellViews();
    }

    private void OnStartStopButtonPressed()
    {
        if (_simulationVasStarted)
        {
            StopCoroutine(SimulateLife());
            _simulationVasStarted = false;
        }
        else
        {
            StartCoroutine(SimulateLife());
            _simulationVasStarted = true;
        }
    }
    
    private void GenerateCellViews()
    {
        for (int x = 0; x < _gameBoard.Size.Width; x++)
        {
            for (int y = 0; y < _gameBoard.Size.Height; y++)
            {
                  var  cellView = Instantiate(_cellPefab, new Vector3(x, 0, y), Quaternion.identity);
                  cellView.Initialize(_gameBoard.Cells[x, y]);
            }
        }
    }
    
    private void GoNextGeneration()
    {
        var tempGameBoard = (GameOfLifeCell[,]) (_gameBoard.Cells.Clone());
        for (int x = 0; x < _gameBoard.Size.Width; x++)
        {
            for (int y = 0; y < _gameBoard.Size.Height; y++)
            {
                var tempCell = tempGameBoard[x, y];
                _gameBoard.Cells[x,y].IsFilled = tempCell.CheckRoules();
            }
        }
    }

    public IEnumerator SimulateLife()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenGenerations);
            GoNextGeneration();
        }
    }
}
public class GameOfLifeBoard : IGameOfLifeCellContainer
{
    public Size Size { get; }
    
    public GameOfLifeCell[,] Cells { get; }

    public GameOfLifeBoard(Size size)
    {
        Size = size;
        Cells = new GameOfLifeCell[Size.Width, Size.Height];
    }
    
    public void Generate()
    {
        for (int x = 0; x < Size.Width; x++)
        {
            for (int y = 0; y < Size.Height; y++)
            {
                var currentCell = Cells[x, y] = new GameOfLifeCell(x,y);
                 
                if (y > 0)
                {
                    GameOfLifeCell.SetFowardBackNeighbours(currentCell, Cells[x, y - 1]);
                }
                if (x > 0)
                {
                    GameOfLifeCell.SetRightLeftNeighbours(currentCell, Cells[x - 1, y]);
                }
            }
        }
    }
}

public interface IGameOfLifeCellContainer
{
    public GameOfLifeCell[,] Cells { get; }
}

public class GameOfLifeCell
{
    public event Action Changed;

    private Neighbors2<GameOfLifeCell> _neighbours = new();

    private IGameOfLifeCellContainer _container;

    public Vector2Int Position { get; }

    private bool _isFilled;

    public bool IsFilled
    {
        get => _isFilled;
        set
        {
            _isFilled = value;
            Changed?.Invoke();
        }
    }

    public GameOfLifeCell(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }

    private IEnumerable<GameOfLifeCell> GetFilledNeighbours()
    {
        var neighbours = new List<GameOfLifeCell>();

        for (int i = 0; i < Neighbors2<GameOfLifeCell>.Length; i++)
        {
            var cell = _neighbours[i];
            if (cell!= null && cell._isFilled)
            {
                neighbours.Add(cell);
            }
        }

        return neighbours;
    }

    public bool CheckRoules()
    {
        var neighbours = GetFilledNeighbours().Count();

        if (!IsFilled && neighbours == 3)
        {
            return true;
        }

        if (IsFilled && neighbours == 3 || neighbours == 2)
        {
            return true;
        }

        return false;
    }

    public static void SetRightLeftNeighbours(GameOfLifeCell right, GameOfLifeCell left)
    {
        right._neighbours.Left = left;
        left._neighbours.Right = right;
    }

    public static void SetFowardBackNeighbours(GameOfLifeCell foward, GameOfLifeCell back)
    {
        foward._neighbours.Back = back;
        back._neighbours.Foward = foward;
    }

}

[Serializable]
public struct Size
{
    public int Width;
    public int Height;
}