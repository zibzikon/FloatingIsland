using System;
using System.Collections;
using System.Collections.Generic;

public class BuildingsColection : IEnumerable<Building>
{
    private List<Building> _colection = new List<Building>();

    public event Action<Building> BuildingRemoved; 
    public event Action<Building> BuildingAdded; 

    public Building this[int index]
    {
        get
        {
            return _colection[index];
        }
        set
        {
            _colection[index] = value;
        }
    }

    public void AddBuilding(Building building)
    {
        building.Died += OnBuildingDestroyed;
        BuildingAdded?.Invoke(building);
        _colection.Add(building);
    }
    
    public void RemoveBuilding(Building building)
    {
        building.Died -= OnBuildingDestroyed;
        BuildingRemoved?.Invoke(building);
        _colection.Remove(building);
    }
    
    private void OnBuildingDestroyed(object sender)
    {
        var building = (Building)sender;
        _colection.Remove(building);
        
        building.Died -= OnBuildingDestroyed;
    }
    
    public IEnumerator<Building> GetEnumerator()
    {
       return _colection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
