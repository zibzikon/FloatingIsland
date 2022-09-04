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
    private GameFieldView _gameFieldView;

    [SerializeField] private EnemiesWavesContainer _enemiesWavesContainer;

    [Space] [Header("Player")]
    
    [SerializeField]
    private PlayerView _playerView;
    
    
    [Space] [Header("UI")]
    
    [SerializeField]
    private Canvas _generalCanvas;

    [SerializeField] private PlayerUI _playerUI;
    
    [Space] [Header("Factories")]
    
    [SerializeField] private EnemyFactory _enemyFactory;

    [SerializeField] private BuildingTilesFactory _buildingTilesFactory;

    [SerializeField] private BuildingPointersFactory _buildingPointersFactory;

    [Space] 
    
    [SerializeField] private bool _spawnBuilding;
    
    [SerializeField] private BuildingType _spawnBuildingType;
    
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
        _gameField.Initialize();
        InitializeFactories();
        _gameFieldView.Initialize(_gameField, _buildingTilesFactory);

        _player = new Player(_gameField, _playerView.Agent, new BuildingFactory(), _buildingPointersFactory);

        /*_player = Instantiate(_playerPrefab, new Vector3(20,0,20), Quaternion.identity);
        _player.Initialize(_gameField);
        _contentToUpdate.Add(_player);
        _playerUI.Initialize(_player);
        
        _contentToUpdate.Add(_playerUI);
        
        _enemySpawner = new EnemySpawner(_gameField, _enemyFactory, _enemiesWavesContainer, _player);
        _enemySpawner.Initialize();
        _contentToUpdate.Add(_enemySpawner);
        
        _player.Inventory.Close();*/
    }

    private void InitializeFactories()
    {
        _buildingTilesFactory.Initialize();
    }
    
    public void OnUpdate()
    {
        _contentToUpdate.ForEach(content => content.OnUpdate());
        
        if (_spawnBuilding)
        {
            _spawnBuilding = false;
            _player.Builder.Build(_spawnBuildingType);
        }
    }
}