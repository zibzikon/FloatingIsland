using System;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

sealed class Builder 
{
    private BuilderBehaviour _builderBehaviour;

    private PlayerInput _input;
    
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
    }

    public void UnRegister()
    {
        _input.Builder.AcceptSettingBuilding.performed -= AcceptSettingBuilding;
        _input.Builder.DeclineSettingBuilding.performed -= DeclineSettingBuilding;
        _input.Builder.SetBuilding.performed -= SetBuilding;
        
        _input.Builder.AddGeneralPillar.performed -= AddSupportPillar;
        _input.Builder.AddWall.performed -= AddWall;
    }

    private void AcceptSettingBuilding(InputAction.CallbackContext context) =>
        _builderBehaviour.AcceptSettingBuilding();
    private void DeclineSettingBuilding(InputAction.CallbackContext context) =>
        _builderBehaviour.DeclineSettingBuilding();
    private void SetBuilding(InputAction.CallbackContext context) =>
        _builderBehaviour.SetBuilding();
    private void AddWall(InputAction.CallbackContext context) =>
        _builderBehaviour.SpawnBuilding(BuildingType.Wall);
    private void AddSupportPillar(InputAction.CallbackContext context) =>
        _builderBehaviour.SpawnBuilding(BuildingType.SupportPillar);
}