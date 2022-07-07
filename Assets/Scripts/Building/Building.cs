using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Extentions;
using UnityEngine;

public abstract class Building : MonoBehaviour, INeighbour, IUpdatable, IRecyclable, ITarget, IDiethable
{
    public Neighbors3<List<INeighbour>> Neighbors = new();
    
    [SerializeField] private int _weight;
    public int Weight => _weight;
    
    [SerializeField] protected  BuildingStats _buildingStats;
    
    [SerializeField] private TargetType targetType;
    public event Action PositionChanged;
    public TargetType TargetType => targetType;
    public Transform Transform => transform;
    
    public Vector3Int PositionOnGameField { get; private set; }
    
    [SerializeField] private List<OccupyingCell> _occupyingCells;
    
    public List<OccupyingCell> OccupyingCells => _occupyingCells;

    [SerializeField] private BuildingType _buildingType;

    [SerializeField] private Direction2 _direction = Direction2.Foward;
    
    public BuildingType BuildingType => _buildingType;

    private IBuildingContainer _supportBuilding;

    public event Action<object> Died; 
    
    private static readonly IEnumerable<BuildingType> _buildingTypesCanBeSettedOnGameField = new[]
    {
        BuildingType.SupportPillar,
    };

    private static readonly Dictionary<BuildingType, BuildingType> _dontMergeableBuildingTypes = new() {[BuildingType.SupportPillar] = BuildingType.SupportPillar };
    
    
    private List<OccupyingCell> _settedCells = new List<OccupyingCell>();
    public List<OccupyingCell> SettedCells { 
        get=> _settedCells;
        set
        {
            if (value.Count != OccupyingCells.Count) throw new IndexOutOfRangeException();
            _settedCells = value;
        }
    }
    
    
    public virtual void OnUpdate(){}
    
    
    public static bool CanBeSettedOnGameField(Building building)
    {
        return _buildingTypesCanBeSettedOnGameField.Contains(building._buildingType);
    }
    
    public static bool CanBeMergedWithBuildings(Building building, IEnumerable<Building> buildings)
    {
        if (!ContainsKeyOrValue(building._buildingType)) return true;
            
        foreach (var building1 in buildings)
        {
            if (!ContainsKeyOrValue(building1.BuildingType)) continue;
                
            if(_dontMergeableBuildingTypes[building1.BuildingType] == building._buildingType||
               _dontMergeableBuildingTypes[building.BuildingType] == building1._buildingType) return false;
        }

        bool ContainsKeyOrValue(BuildingType buildingType) =>
            _dontMergeableBuildingTypes.ContainsKey(buildingType) ||
            _dontMergeableBuildingTypes.ContainsValue(buildingType);

        return true;
    }
    
    public void SetDirection(Direction2 direction)
    {
        for (int i = 0; i < _occupyingCells.Count; i++)
        {
            var cell = _occupyingCells[i];
            var newPosition = cell.Position.SetDirection(_direction, direction);
            _occupyingCells[i] = OccupyingCell.Create(newPosition, new Neighbors3<bool>());
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


    public void AddNeighbour(INeighbour neighbour, Direction3 direction)
    {
        var neighbourList = Neighbors[direction] ?? (Neighbors[direction] = new List<INeighbour>());
        
        neighbourList.Add(neighbour);
    }
}