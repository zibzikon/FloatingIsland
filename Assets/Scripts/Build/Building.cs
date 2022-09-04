using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Extentions;
using UnityEngine;

public abstract class Building : IUpdatable, IRecyclable, ITarget
{
    public abstract int Health { get; protected set; }
    
    protected virtual Dictionary<Direction2, Vector3> DirectionRotation { get; }
    
    protected readonly IBuildingsContainer BuildingsContainer;

    public Neighbors3<Building> Neighbors { get; } = new();
    
    public event Action<object> DirectionChanged; 

    public event Action PositionChanged;
    
    public event Action<object> Destroyed;

    public event Action Damaged;

    public FloatingIslandTransform Transform { get; } = new ();
    
    public abstract List<OccupyingCell> OccupyingCells { get; }
    
    public Vector3Int PositionOnGameField { get; private set; }
    
    public abstract TargetType TargetType { get; }
    
    public abstract BuildingType BuildingType { get; }
    
    public abstract DamagableType DamagableType { get; }

    private List<Building> _supportingBuildings;
    
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

    private List<OccupyingCell> _occupyingCellsOnGameField = new ();
    private Transform _transform1;

    public List<OccupyingCell> OccupyingCellsOnGameField { 
        get=> _occupyingCellsOnGameField;
        set
        {
            if (value.Count != OccupyingCells.Count) throw new IndexOutOfRangeException();
            _occupyingCellsOnGameField = value;
        }
    }

    protected Building(IBuildingsContainer buildingsContainer)
    {
        BuildingsContainer = buildingsContainer;
    }
    
    public virtual void OnUpdate(){}
    
    public static bool CanBePlacedOnGameField(Building building)
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

    public void Place(Vector3Int position)
    {
        PositionOnGameField = position;
        Transform.Position = position.IsometricToScreenPosition();
    }
    
    public void SetDirection(Direction2 direction)
    {
        DirectionChanged?.Invoke(this);
        Direction = direction;
    }

    public abstract bool ValidateSetSupportBuilding(IBuildingContainer supportBuilding);
    
    public virtual void SetPositionOnGameField(Vector3Int position)
    {
        PositionOnGameField = position;
    }
    
    public virtual void TakeDamage(int count)
    {
        Damaged?.Invoke();
        Health -= count;
        
        if (Health > 0) return;
        Destroy();
    }
     
    public void Recycle()
    {
        
    }
    
    public virtual void Destroy()
    {
        IsDestroyed = true;
        Destroyed?.Invoke(this);
    }
    
    public void SetNeighbour(Building neighbour, Direction3 direction)
    {
        Neighbors[direction] = neighbour;
    }
    
    public void Select()
    {
        
    }

    public void Deselect()
    {
        
    }
}