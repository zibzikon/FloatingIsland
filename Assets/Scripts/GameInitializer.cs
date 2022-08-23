using System.Collections.Generic;
using Entities.EnemySpawner;
using Factories.BuildingFactories;
using Factories.EnemyFactories;
using UnityEngine;

public class GameInitializer : MonoBehaviour, IUpdatable
{

    
    [Header("Game")]
    
    [SerializeField] 
    private GameField _gameField;

    [SerializeField]
    private Player _playerPrefab;

    [SerializeField] private EnemiesWavesContainer _enemiesWavesContainer;

    [Space]
    
    [Header("UI")]
    
    [SerializeField]
    private Canvas _generalCanvas;

    [SerializeField] private PlayerUI _playerUI;
    
    [Space]

    [Header("Factories")]
    
    [SerializeField] private EnemyFactory _enemyFactory;

    [SerializeField] private BuildingTilesFactory _buildingTilesFactory;
    
    [Space]
    
    private readonly List<IUpdatable> _contentToUpdate = new();
    
    private EnemySpawner _enemySpawner;
    
    private Player _player;
    
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
        InitializeFactories();
        
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

    private void InitializeFactories()
    {
        _enemyFactory.Initialize();
        _buildingTilesFactory.Initialize();
    }
    
    public void OnUpdate()
    {
        _contentToUpdate.ForEach(content => content.OnUpdate());
    }
}

