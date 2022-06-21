using System;
using System.Collections.Generic;
using Enums;
using Factories.Building;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

public sealed class Player : MonoBehaviour, IUpdatable, ITarget, IDiethable, IPausable
{
    public bool WasDied { get; private set; }
    
    public event Action PositionChanged;

    public bool IsPaused { get; private set; }

    public event Action<object> Died;
    
    [SerializeField] private PlayerStats _playerStats;

    public TargetType TargetType { get; } = TargetType.Player;
    public Transform Transform => transform;

    [SerializeField] private BuildingFactory _buildingFactory;

    [SerializeField] private BuildingPointersFactory _buildingPointersFactory;

    private readonly List<IUpdatable> _contentToUpdate = new();

    private Builder _builder;

    public void Initialize(GameField gameField, Transform plyerUi)
    {
        _builder = new Builder(new BuilderBehaviour(gameField, _buildingFactory, _buildingPointersFactory,
            Camera.main));
        _contentToUpdate.Add(_builder);
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