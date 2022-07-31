using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Extentions;
using UnityEngine;

public abstract class Building : MonoBehaviour, INeighbour, IUpdatable, IRecyclable, ITarget, ISelectable
{
    protected int Health;

    [SerializeField] private ItemsDroppingFactory _itemsDroppingFactory;
    
    [SerializeField] private DirectionRotation _directionRotation;

    [SerializeField] private Vector3 _size;
    public Vector3 Size => _size;
    
    [SerializeField] private Vector3 _centerPosition;
    public Vector3 CenterPosition => _centerPosition;
    
    protected IBuildingsContainer BuildingsContainer;

    public Neighbors3<IEnumerable<INeighbour>> Neighbors { get; } = new();
    
    public abstract int Weight { get; }

    public event Action<object> DirectionChanged; 

    public event Action PositionChanged;
    
    public event Action<object> Destroyed; 
    
    public event Action Damaged;

    public Transform Transform => transform;

    [SerializeField] private List<OccupyingCell> _occupyingCells;

    public List<OccupyingCell> OccupyingCells => _occupyingCells;
    
    public Vector3Int PositionOnGameField { get; private set; }
    
    protected abstract BuildingStats BuildingStats { get; }
    
    public abstract TargetType TargetType { get; }
    
    public abstract BuildingType BuildingType { get; }
    
    public abstract DamagableType DamagableType { get; }

    protected BuildPoint ParentBuildPoint { get; set; }
    
    public bool IsDestroyed { get; private set; }

    protected abstract Direction2 Direction { get; set; }
    
    private static readonly IEnumerable<BuildingType> _buildingTypesCanBeSettedOnGameField = new[]
    {
        BuildingType.SupportPillar,
        BuildingType.WoodenCrafter,
        BuildingType.Tower,
    };

    private static readonly Dictionary<BuildingType, BuildingType> _nonMergeableBuildingTypes = new()
    {
        [BuildingType.SupportPillar] = BuildingType.SupportPillar
    };

    private List<OccupyingCell> _settedCells = new ();

    public List<OccupyingCell> SettedCells { 
        get=> _settedCells;
        set
        {
            if (value.Count != OccupyingCells.Count) throw new IndexOutOfRangeException();
            _settedCells = value;
        }
    }

    public virtual void Initialize()
    {
        Health = BuildingStats.Health;
    }
   
    public virtual void OnUpdate(){}
    
    public static bool CanBeSettedOnGameField(Building building)
    {
        return _buildingTypesCanBeSettedOnGameField.Contains(building.BuildingType);
    }
    
    public static bool CanBeMergedWithBuildings(Building building, IEnumerable<Building> buildings)
    {
        if (!ContainsKeyOrValue(building.BuildingType)) return true;
            
        foreach (var building1 in buildings)
        {
            if (!ContainsKeyOrValue(building1.BuildingType)) continue;
                
            if(_nonMergeableBuildingTypes[building1.BuildingType] == building.BuildingType||
               _nonMergeableBuildingTypes[building.BuildingType] == building1.BuildingType) return false;
        }

        bool ContainsKeyOrValue(BuildingType buildingType) =>
            _nonMergeableBuildingTypes.ContainsKey(buildingType) ||
            _nonMergeableBuildingTypes.ContainsValue(buildingType);

        return true;
    }
    
    public void SetDirection(Direction3 direction3)
    {
        Direction2 direction;

        try
        {
            direction = direction3.AsDirection2();
        }
        catch
        {
            direction = Direction;
        }
        
        for (var i = 0; i < OccupyingCells.Count; i++)
        {
            var cell = OccupyingCells[i];
            var newPosition = cell.Position.SetDirection(Direction, direction);
            OccupyingCells[i] = OccupyingCell.Create(newPosition, new Neighbors3<bool>());
        }

        transform.rotation = Quaternion.Euler(_directionRotation.GeRotationByDirection(direction.AsDirection3()));
        DirectionChanged?.Invoke(this);
        Direction = direction;
    }

    public virtual void SetSupportBuilding(BuildPoint parentBuildPoint, Direction3 settingDirection)
    {
        if (ValidateSetSupportBuilding(parentBuildPoint.BuildingContainer) == false) throw new InvalidOperationException();
        SetDirection(settingDirection);
        ParentBuildPoint = parentBuildPoint;
        parentBuildPoint.BuildingContainer.AddChildBuilding(this, settingDirection);
    }

    
    public abstract bool ValidateSetSupportBuilding(IBuildingContainer supportBuilding);
    
    public void SetPositionOnGameField(Vector3Int position)
    {
        PositionOnGameField = position;
    }
    
    public void TakeDamage(int count)
    {
        var newBuildingStats = BuildingStats;
        Health -= count;
        Damaged?.Invoke();
        if (Health > 0) return;
        Destroy();
    }
     
    public void Recycle()
    {
        
    }
    
    public virtual void Destroy()
    {
        ParentBuildPoint?.Reset();
        IsDestroyed = true;
        Destroyed?.Invoke(this);
        
        BuildingStats.DropItems.ForEach(itemType => _itemsDroppingFactory.Get(itemType, transform.position));
        if(gameObject != null)
            Destroy(gameObject);
    }


    public void AddNeighbour(INeighbour neighbour, Direction3 direction)
    {
        var neighbourList = (Neighbors[direction] ?? (Neighbors[direction] = new List<INeighbour>())).ToList();
        
        neighbourList.Add(neighbour);

        Neighbors[direction] = neighbourList;
    }

    public void Select()
    {
        
    }

    public void Deselect()
    {
        
    }
}