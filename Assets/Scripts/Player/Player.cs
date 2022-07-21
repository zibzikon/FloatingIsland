using System;
using System.Collections.Generic;
using Enums;
using Factories.Building;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;


public sealed class Player : MonoBehaviour, IUpdatable, ITarget, IDestroyable, IPausable
{
    private PlayerInput _input;
    
    public bool IsDestroyed { get; private set; }
    
    public event Action PositionChanged;

    public bool IsPaused { get; private set; }

    public event Action<object> Destroyed;
    
    private PlayerStats _playerStats;

    private Inventory _inventory;

    public TargetType TargetType => TargetType.Player;
    
    public Transform Transform => transform;

    [SerializeField] private BuildingFactory _buildingFactory;

    [SerializeField] private BuildingPointersFactory _buildingPointersFactory;
    
    private PlayerUI _playerUI;

    private readonly List<IUpdatable> _contentToUpdate = new();

    private Builder _builder;

    
    public void Initialize(GameField gameField, PlayerUI playerUI)
    {
        
        InitializePlayerStats();
        _input = new PlayerInput();
        

        
        _builder = new Builder(new BuilderBehaviour(gameField, _buildingFactory, _buildingPointersFactory,
            Camera.main), _input);
        
        _inventory = new Inventory(_builder);
        _inventory.Close();

        _playerUI = playerUI;
        _playerUI.Initialize(_inventory);
        _inventory.Close();

        _input.Enable();;
        _builder.Register();
        _contentToUpdate.Add(_builder);
    }


    private void InitializePlayerStats()
    {
        _playerStats = new PlayerStats
        {
            Health = 2000
        };
    }
    
    private void OnDisable()
    {
        _input.Disable();
        _builder.UnRegister();
    }

    public void OnUpdate()
    {
        if(IsPaused) return;
        

        if (Keyboard.current.eKey.wasPressedThisFrame )
        {
            switch (_inventory.CurrentState)
            {
                case ItemsContainer.ContainerState.Closed:
                    _inventory.Open();
                    break;
                case ItemsContainer.ContainerState.Opened:
                    _inventory.Close();
                    break;
            }
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (_inventory.CurrentState == ItemsContainer.ContainerState.Opened) _inventory.Close();
        }

        foreach (var content in _contentToUpdate)
        {
            content.OnUpdate();
        }
    }
    
    public void Damage(int count)
    {
        _playerStats.Health -= count;
        if (_playerStats.Health > 0) return;
        Destroy();
    }


    public void Destroy()
    {
        Destroyed?.Invoke(this);
        IsDestroyed = true;
        Debug.Log("player was died");
    }

    public void Pause()
    {
        IsPaused = true;
    }

    public void UnPause()
    {
        IsPaused = false;
    }
}