using System;
using System.Collections.Generic;
using Enums;
using Factories.BuildingFactories;
using Interfaces;
using Units.Behaviours;
using UnityEngine;
using UnityEngine.AI;

public sealed class Player : Entity, IUpdatable, ITarget, IPausable, IInteractor
{
    [SerializeField] private NavMeshAgent _agent;
    
    [SerializeField] private BuildingFactory _buildingFactory;

    [SerializeField] private BuildingPointersFactory _buildingPointersFactory;

    [SerializeField] private bool _build;
    
    [SerializeField] private BuildingType _buildingType;
    
    protected override EntityStats Stats { get; }

    public DamagableType DamagableType => DamagableType.Entity;
    
    public TargetType TargetType => TargetType.Player;

    public bool IsDestroyed { get; private set; }

    public bool IsPaused { get; private set; }

    public event Action PositionChanged;

    public event Action<object> Destroyed;
    
    public event Action<ISelectable> SelectableObjectSelected;
    
    public event Action SelectableObjectDeselected;

    public Inventory Inventory { get; private set; }

    private PlayerStats _playerStats;
    
    public Transform Transform => transform;

    private int _radiusToCollectDroppedItems = 2;
    
    private PlayerInput _input;

    private readonly List<IUpdatable> _contentToUpdate = new();

    private Builder _builder;

    private Item _selectedItem = new WoodenAxe();

    private IMovable _movingBehaviour;
    
    private ISelectable _selectedObject;

    private ITarget _selectedTarget;

    private IInteractable _selectedInteractableObject;
    
    private bool _targetAttackingStarted;

    private Timer _timer;
    
    public void Initialize(GameField gameField)
    {
        InitializePlayerStats();
        _movingBehaviour = new TerrestrialEntityMovingBehaviour(_agent, 6);
        _contentToUpdate.Add(_movingBehaviour);
        
        _input = new PlayerInput();
        

        _timer = new Timer();
        _contentToUpdate.Add(_timer);
        
        _buildingFactory.Initialize();
        _builder = new Builder(new BuilderBehaviour(gameField, _buildingFactory, _buildingPointersFactory,
            Camera.main), _input);
        
        Inventory = new Inventory(_builder);

        _input.Enable();;
        _builder.Register();
        _contentToUpdate.Add(_builder);
    }


    private void InitializePlayerStats()
    {
        _playerStats = new PlayerStats
        {
            Health = 20000000
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
        
        foreach (var content in _contentToUpdate)
        {
            content.OnUpdate();
        }
        
        if (_build)
        {
            _build = false;
            _builder.Build(_buildingType);
        }
        
        if (_movingBehaviour.IsMoving) PositionChanged?.Invoke();

        CollectDroppedItems();
        AttackTarget();
    }
    
    
    private void AttackTarget()
    {
        if (!_targetAttackingStarted) return;
        
        if (_selectedTarget == null)
        {
            throw new NullReferenceException();
        }
            
        if (_selectedTarget.IsDestroyed)
        {
            ResetSelectedObjects();
        }
        else if (_timer.TimeIsOut && _movingBehaviour.TargetWasReached)
        {
            _timer.Start(2);
            Debug.Log("TargetWasAttacked");
            _selectedItem.Damage(_selectedTarget);
        }
    }
    
    public bool TryOpenInventory()
    {
        if (_builder.IsBuilding || _builder.BuildingEndedOnThisFrame || Inventory.CurrentState == ItemsContainer.ContainerState.Opened)
            return false;
        
        Inventory.Open();
        return true;
    }
     
    public void CloseInventory()
    {
        Inventory.Close();
    }

    private void CollectDroppedItems()
    {
        var colliders = Physics.OverlapSphere(transform.position, _radiusToCollectDroppedItems);
        
        foreach (var collision in colliders)
        {
            var collisionObject = collision.gameObject.GetComponent<CollisionObject>();
            if (collisionObject == null) continue;
  
            var itemDrop = collisionObject.Parent.GetComponent<ItemDrop>();
            if (itemDrop == null) continue;

            itemDrop.Collect(Inventory);
        }
    }
    
    public bool TrySelectSelectableObject()
    {
        ResetSelectedObjects();
        if (_builder.IsBuilding) return false;

        var selectedTransform = RayCast.TrySelectItem(Camera.main);
        if (selectedTransform == null) return false;

        _selectedObject = selectedTransform.GetComponent<ISelectable>();
        if (_selectedObject == null) return false;
        
        _selectedObject.Select();
        SelectableObjectSelected?.Invoke(_selectedObject);
        OnSelectableObjectSelected();
       
        return true;
    }

    private void ResetSelectedObjects()
    {
        _movingBehaviour.Reset();
        if (_selectedObject != null)
        {
            _selectedObject.Deselect();
            SelectableObjectDeselected?.Invoke();
        }
        _selectedObject = null;
        _selectedTarget = null;
        _selectedInteractableObject = null;
        _targetAttackingStarted = false;
    }
    
    private void OnSelectableObjectSelected()
    {
        if (_selectedObject == null|| _selectedObject.Transform == null) 
            throw new NullReferenceException();
        
        var selectedObjectTransform = _selectedObject.Transform;
        
        _selectedTarget = selectedObjectTransform.GetComponent<ITarget>();
        _selectedInteractableObject = selectedObjectTransform.GetComponent<IInteractable>();
    }

    public bool TryMoveToHitGameFieldPoint()
    {
        if (_builder.IsBuilding) return false;

        var hit = RayCast.GetRaycastHitByMousePosition(Camera.main);
        var collider = hit.collider;
        if (collider != null)
        {
            var collisionObject = hit.collider.GetComponent<CollisionObject>();
            if (collisionObject != null)
            {
                var gameField = collisionObject.Parent.GetComponent<GameField>();
                if (gameField != null)
                {
                    var hitPoint = hit.point;
                    _movingBehaviour.MoveTo(hitPoint);
                    ResetSelectedObjects();
                    return true;
                }
            }
        }

        return false;
    }

    public void StartAttackingTarget()
    {
        _targetAttackingStarted = true;
        _movingBehaviour.SetTarget(_selectedTarget);
    }

    public void TakeDamage(int count)
    {
        _playerStats.Health -= count;
        if (_playerStats.Health > 0) return;
        Destroy();
    }


    public void Destroy()
    {
        Destroyed?.Invoke(this);
        IsDestroyed = true;
        Pause();
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


    public bool TryInteractWithSelectedInteractable()
    {
        if (_selectedInteractableObject == null)
            return false;
        
        _selectedInteractableObject.Interact(this);
        return true;
    }

}