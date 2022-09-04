using System.Collections.Generic;
using System.Linq;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public abstract class Plant : Building, IPlantGrowStateSwitcher
{
    public override TargetType TargetType { get; }
    
    protected override Direction2 Direction { get; set; }

    protected abstract ReadOnlyArray<IPlantGrowState> GrowStates { get; }

    private int _stateIndex = 0;

    public int State => _stateIndex;
    
    private Timer _timer;

    protected IPlantGrowState GrowState;

    private bool _growing;
    
    private List<IUpdatable> _contentToUpdate = new ();
    
    protected Plant(IBuildingsContainer buildingsContainer) : base(buildingsContainer)
    {
        StartGrowing();
    }
    protected virtual void Grow()
    {
        GoNextGrowState();
    }
    
    public override bool ValidateSetSupportBuilding(IBuildingContainer supportBuilding)
    {
        return false;
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        _contentToUpdate.ForEach(_contentToUpdate => _contentToUpdate.OnUpdate());
        GrowOnTick();
    }
    
    private void GrowOnTick()
    {
        if (_growing == false ) return;
        
        if (BuildingsContainer.CellsIsFreeToSet(this, GetNextGrowState().OccupyingCells))
        {
            Grow();
        }
    }
    
    private void StartGrowing()
    {
        _growing = true;
        
        GrowState = GrowStates[_stateIndex];
    }
    
    private void StopGrowing()
    {
        _growing = false;
    }
    
    public void GoNextGrowState()
    {
        if (_stateIndex >= GrowStates.Count)
        {
            StopGrowing();
            return;
        }
        
        var current = GrowStates[_stateIndex];
        
        GrowState = current;
    }

    public IPlantGrowState GetNextGrowState()
        => GrowStates[_stateIndex + 1];
}

public interface IPlantGrowState
{
    IEnumerable<OccupyingCell> OccupyingCells { get; }
    float TimeToGrow { get; }
    void Start();
    void Stop();
}

public interface IPlantGrowStateSwitcher
{
    void GoNextGrowState();
    
    IPlantGrowState GetNextGrowState();
} 