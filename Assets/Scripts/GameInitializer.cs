using System.Collections.Generic;
using Entities.EnemySpawner;
using Factories.Enemy;
using UnityEngine;

public class GameInitializer : MonoBehaviour, IUpdatable
{
    [SerializeField]
    private Player _playerPrefab;

    private Player _player;
    
    [SerializeField]
    private Canvas _generalCanvas;
    
    [SerializeField] private GameField _gameField;

    [SerializeField] private EnemyFactory _enemyFactory;
    
    [SerializeField] private PlayerUI _playerUI;

    private List<IUpdatable> _contentToUpdate = new();

    private EnemySpawner _enemySpawner;

    [SerializeField] private EnemiesWavesContainer _enemiesWavesContainer; 

    private void Update()
    {
        OnUpdate();
    }
    
    private void Awake()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        _player = Instantiate(_playerPrefab, new Vector3(20,0,20), Quaternion.identity);
        _player.Initialize(_gameField);
        _contentToUpdate.Add(_player);
        _playerUI.Initialize(_player);
        _contentToUpdate.Add(_playerUI);
        
        _enemySpawner = new EnemySpawner(_gameField, _enemyFactory, _enemiesWavesContainer, _player);
        _enemySpawner.Initialize();
        _contentToUpdate.Add(_enemySpawner);
        
        _player.Inventory.Close();
    }

    public void OnUpdate()
    {
        _contentToUpdate.ForEach(content => content.OnUpdate());
    }
}

