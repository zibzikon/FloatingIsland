using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GrowStates
{
    [SerializeField] private List<GrowState> growStates;
    public int StateNumber { get; private set; }
    
    public bool CanBeMovedNext() =>
        StateNumber < growStates.Count;

    public GrowState GetCurrentGrowState()
    {
        return growStates[StateNumber];
    }
    public void MoveNext()
    {
        if (!CanBeMovedNext()) throw new IndexOutOfRangeException();
        
        StateNumber += 1;
    }
    
    public List<OccupyingCell> GetNextGrowStateOccupyingCells()
    {
        return growStates[StateNumber + 1].OccupyingCells;
    }
}

[Serializable]
public struct GrowState
{
    [SerializeField] private List<OccupyingCell> _occupyingCells;
    public List<OccupyingCell> OccupyingCells => _occupyingCells;
}