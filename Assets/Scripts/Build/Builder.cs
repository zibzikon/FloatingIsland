using System;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Builder : IUpdatable, IBuilder
{
    private readonly BuilderBehaviour _builderBehaviour;

    private readonly PlayerInput _input;

    public bool IsBuilding => _builderBehaviour.IsBuilding;
    public bool BuildingEndedOnThisFrame => _builderBehaviour.BuildingEndedOnThisFrame;
    
    public Builder(BuilderBehaviour builderBehaviour, PlayerInput input)
    {
        _builderBehaviour = builderBehaviour;
        _input = input;
    }

    public void Register()
    {
        _input.Builder.AcceptSettingBuilding.performed += AcceptSettingBuilding;
        _input.Builder.DeclineSettingBuilding.performed += DeclineSettingBuilding;
        _input.Builder.SetBuilding.performed += SetBuilding;
        
        _input.Builder.AddGeneralPillar.performed += AddSupportPillar;
        _input.Builder.AddWall.performed += AddWall;
        _input.Builder.AddTower.performed += AddTower;
        _input.Builder.AddTurret.performed += AddTurret;
    }

    public void UnRegister()
    {
        _input.Builder.AcceptSettingBuilding.performed -= AcceptSettingBuilding;
        _input.Builder.DeclineSettingBuilding.performed -= DeclineSettingBuilding;
        _input.Builder.SetBuilding.performed -= SetBuilding;
        
        _input.Builder.AddGeneralPillar.performed -= AddSupportPillar;
        _input.Builder.AddWall.performed -= AddWall;
        _input.Builder.AddTower.performed -= AddTower;
        _input.Builder.AddTurret.performed -= AddTurret;
    }

    private void AcceptSettingBuilding(InputAction.CallbackContext context) =>
        _builderBehaviour.AcceptSettingBuilding();
    
    private void DeclineSettingBuilding(InputAction.CallbackContext context) =>
        _builderBehaviour.DeclineSettingBuilding();
    
    private void SetBuilding(InputAction.CallbackContext context)
    {
        if (_builderBehaviour.IsBuilding)
            _builderBehaviour.SetBuilding();
    }
    
    private void AddWall(InputAction.CallbackContext context) =>
        _builderBehaviour.SpawnBuilding(BuildingType.Wall);

    private void AddTower(InputAction.CallbackContext context) =>
        _builderBehaviour.SpawnBuilding(BuildingType.Tower);
    
    private void AddTurret(InputAction.CallbackContext context) =>
        _builderBehaviour.SpawnBuilding(BuildingType.WoodenCrafter);
    
    private void AddSupportPillar(InputAction.CallbackContext context) =>
        _builderBehaviour.SpawnBuilding(BuildingType.SupportPillar);

    public void OnUpdate()
    {
        _builderBehaviour.OnUpdate();
    }

    public void Build(BuildingType buildingType)
    {
        _builderBehaviour.SpawnBuilding(buildingType);
    }
}