using Enums;
using UnityEngine;

public abstract class Plant : Building
{
    public override TargetType TargetType { get; }
    protected override Direction2 Direction { get; set; }

    protected abstract PlantStats PlantStats { get; }

    protected override BuildingStats BuildingStats => PlantStats;
    private Timer _timer;

    [SerializeField] private GrowStates _growStates;
    
    protected abstract void OnGrow();

    private void Grow()
    {
        _growStates.MoveNext();
        OnGrow();
    }
    
    public override bool ValidateSetSupportBuilding(IBuildingContainer supportBuilding)
    {
        return false;
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_growStates.StateNumber < PlantStats.MaxGrowState)
        {
            if (_timer.TimeIsOut)
            {
                _timer.Start(PlantStats.TimeToGrow);
                if (BuildingsContainer.CellsIsFreeToSet(this,_growStates.GetNextGrowStateOccupyingCells()))
                {
                    Grow();
                }
            }
            _timer.OnUpdate();
        }
    }
}
