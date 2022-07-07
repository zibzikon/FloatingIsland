using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Factories.Building;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public sealed class Player : MonoBehaviour, IUpdatable, ITarget, IDiethable, IPausable
{
    private PlayerInput _input;
    
    public bool WasDied { get; private set; }
    
    public event Action PositionChanged;

    public bool IsPaused { get; private set; }

    public event Action<object> Died;
    
    [SerializeField] private PlayerStats _playerStats;

    public TargetType TargetType { get; } = TargetType.Player;
    public Transform Transform => transform;

    [SerializeField] private BuildingFactory _buildingFactory;

    [SerializeField] private BuildingPointersFactory _buildingPointersFactory;
    
    [SerializeField] private BuildPointsFactory _buildingPointsFactory;

    private readonly List<IUpdatable> _contentToUpdate = new();

    private Builder _builder;
    
    public void Initialize(GameField gameField)
    {
        _input = new PlayerInput();
        _builder = new Builder(new BuilderBehaviour(gameField, _buildingFactory, _buildingPointersFactory, _buildingPointsFactory,
            Camera.main), _input);
        _input.Enable();;
        _builder.Register();
    }
    
    private void OnDisable()
    {
        _input.Disable();
        _builder.UnRegister();
    }

    public void OnUpdate()
    {
        if(IsPaused) return;
        
        foreach (var content in _contentToUpdate)
        {
            content.OnUpdate();
        }
    }
    
    public void Damage(int count)
    {
        _playerStats.Health -= count;
        if (_playerStats.Health > 0) return;
        Die();
    }


    public void Die()
    {
        Died?.Invoke(this);
        WasDied = true;
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