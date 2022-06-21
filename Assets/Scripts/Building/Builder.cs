using System;
using System.Collections.Generic;
using UnityEngine;

sealed class Builder : IUpdatable
{
    private BuilderBehaviour _builderBehaviour;
    
    public Builder(BuilderBehaviour builderBehaviour)
    {
        _builderBehaviour = builderBehaviour;
    }

    public void OnUpdate()
    {
        _builderBehaviour.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _builderBehaviour.SpawnBuilding(BuildingType.SupportPillar);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _builderBehaviour.SpawnBuilding(BuildingType.Wall);
        }
    }
}