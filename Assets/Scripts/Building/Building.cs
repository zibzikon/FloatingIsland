using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

public abstract class Building : MonoBehaviour , IUpdatable, IRecyclable, ITarget, IDiethable
{
    [SerializeField] private int _weight;
    public int Weight => _weight;
    
    [SerializeField] protected  BuildingStats _buildingStats;
    
    [SerializeField] private TargetType targetType;
    public event Action PositionChanged;
    public TargetType TargetType => targetType;
    public Transform Transform => this.transform;
    
    [SerializeField] private Vector3Int _startPosition;
    
    public Vector3Int PositionOnGameField { get; private set; }
    
    [SerializeField] private List<OccupingCell> _occupyingCells;
    
    public List<OccupingCell> OccupyingCells => _occupyingCells;

    [SerializeField] private BuildingType _buildingType;

    [SerializeField] private Direction2 _direction = Direction2.Foward;

    [SerializeField] private Direction2 _settingDirection;
    
    public IBuildingsContainer BuildingContainer { get; private set;}
    public BuildingType BuildingType => _buildingType;

    private IBuildingContainer _supportBuilding;

    public event Action<object> Died; 
    
    private static readonly IEnumerable<BuildingType> _buildingTypesCanBeSettedOnGameField = new[]
    {
        BuildingType.SupportPillar,
        BuildingType.Wall
    };

    private List<OccupingCell> _setedCells = new List<OccupingCell>();
    public List<OccupingCell> SetedCells { 
        get=> _setedCells;
        set
        {
            if (value.Count != OccupyingCells.Count) throw new IndexOutOfRangeException();
            _setedCells = value;
        }
    }
    
    
    public void Initialize(IBuildingsContainer buildingContainer)
    {
        BuildingContainer = buildingContainer;
    }
    
    public void OnUpdate()
    {
    }
    
    public static bool CanBeSettedOnGameField(Building building)
    {
        return _buildingTypesCanBeSettedOnGameField.Contains(building._buildingType);
    }
    
    public void SetDirection(Direction2 direction)
    {
        for (int i = 0; i < _occupyingCells.Count; i++)
        {
            var cell = _occupyingCells[i];
            var newPosition = cell.Position.SetDirection(_direction, direction);
            _occupyingCells[i] = new OccupingCell(newPosition);
        }
        
        _direction = direction;
    }

    public void SetSupportBuilding(IBuildingContainer supportBuilding)
    {
        _supportBuilding = supportBuilding;
    }

    public void SetPositionOnGameField(Vector3Int position)
    {
        PositionOnGameField = position;
    }
    
    public void Damage(int count)
    {
        _buildingStats.Health -= count;
        if (_buildingStats.Health > 0) return;
        Die();
    }
     
    public void Recycle()
    {
        
    }
    
    public void Die()
    {
        Died?.Invoke(this);
        Destroy(gameObject);
    }


}

[Serializable]
public struct OccupingCell
{
    [SerializeField] private Vector3Int _position;
    public Vector3Int Position => _position;

    public OccupingCell(Vector3Int position)
    {
        _position = position;
    }
}