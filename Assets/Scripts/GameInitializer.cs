using System.Collections.Generic;
using Factories.Enemy;
using UnityEngine;

public class GameInitializer : MonoBehaviour, IUpdatable
{
    [SerializeField]
    private Player _playerPrefab;

    private Player _player;
    
    [SerializeField]
    private Canvas _generalCanvas;

    [SerializeField] private MainUI _mainUIPrefab;

    [SerializeField] private GameField _gameField;

    [SerializeField] private EnemyFactory _enemyFactory;
    
    private List<IUpdatable> _contentToUpdate = new();

    private EnemySpawner _enemySpawner;
    [SerializeField] private bool spawn;

    private void Update()
    {
        OnUpdate();
    }
    
    private void Start()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        var mainUI = Instantiate(_mainUIPrefab, _generalCanvas.transform);
        mainUI.Initialize(_player);
        
        _player = Instantiate(_playerPrefab, new Vector3(20,0,20), Quaternion.identity);
        _player.Initialize(_gameField, mainUI.transform);
        _contentToUpdate.Add(_player);

        _enemySpawner = new EnemySpawner(_gameField, _enemyFactory, _player);
        _enemySpawner.Initialize();
        _contentToUpdate.Add(_enemySpawner);
    }

    public void OnUpdate()
    {
        if (spawn)
        {
            _enemySpawner.SpawnEnemy();
            spawn = false;
        }
        _contentToUpdate.ForEach(content => content.OnUpdate());
    }
}

