﻿using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Factories.BuildingFactories;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderBehaviour : IUpdatable, IRecyclable
{
    private readonly Camera _mainCamera;

    private readonly GameField _gameField;
    
    private readonly BuildingFactory _buildingFactory;
    
    private readonly BuildingPointersFactory _buildingPointersFactory;

    private readonly BuildingsColection _placedBuildings = new();

    private bool _currentBuildingIsReadyToSet => CheckBuildingSettingAvailable();
    
    private Building _currentBuilding;
    private Building _selectedBuilding;
    
    private BuildPointer _selectedBuildPointer;
    private List<BuildPointer> _spawnedPointers = new();
    private List<BuildPoint> _correctPoints = new();
    
    private bool _buildingIsPlacedOnGameField;
    public bool IsBuilding { get; private set; }

    public bool BuildingEndedOnThisFrame { get; private set; }

    public BuilderBehaviour(GameField gameField,BuildingFactory buildingFactory, 
        BuildingPointersFactory buildingPointersFactory, Camera mainCamera)
    {
        _gameField = gameField;
        _buildingFactory = buildingFactory;
        _buildingPointersFactory = buildingPointersFactory;
        _mainCamera = mainCamera;
    }

    public void OnUpdate()
    {
        if (!IsBuilding)
        {
            BuildingEndedOnThisFrame = false;
        }
        foreach (var building in _placedBuildings)
        {
            building.OnUpdate();
        }
    }
    
    public void SetBuilding()
    {
        if (_currentBuilding == null)
            throw new NullReferenceException("Current building is null but you tying to access it");
        
        if ( _selectedBuilding == null)
        {
            var selectedBuilding= RayCast.TrySelectItem(_mainCamera)?.GetComponent<BuildingWithChilds>();
            if (selectedBuilding != null && selectedBuilding != _currentBuilding)
            {
                _selectedBuilding = selectedBuilding;
                OnBuildingSelected();
            }
            else if(Building.CanBePlacedOnGameField(_currentBuilding))
            {
                if (_gameField.TrySetBuilding(_currentBuilding, _gameField.ConvertScreenToGameFieldPosition(Mouse.current.position.ReadValue())))
                    _buildingIsPlacedOnGameField = true;
            }
        }
        else if (_selectedBuilding != null && _selectedBuildPointer == null)
        {
            _selectedBuildPointer = RayCast.TrySelectItem(_mainCamera)?.GetComponent<BuildPointer>();
            if (_selectedBuildPointer != null)
            {
                OnPointerSelected(_selectedBuildPointer);
            }
        } 
    }
    
    public void SpawnBuilding(BuildingType type)
    {
        if (_currentBuilding != null) return;
        _currentBuilding = _buildingFactory.GetNewBuilding(type, _gameField);
        IsBuilding = true;
    }

    private List<BuildPointer> SpawnBuildingPointers(Vector3 position, Vector3 size)
    {
      
        var pointerSpawningPositionPosition = position + size;
        var buildingPointers = new List<BuildPointer>();
        var settedDirections = new List<Direction3>();
        foreach (var direction in _correctPoints.Select(point => point.Direction).
                     Where(direction => !settedDirections.Contains(direction)))
        {
            if (direction == Direction3.Zero) continue;

            settedDirections.Add(direction);
            buildingPointers.Add(_buildingPointersFactory.GetNewBuildPointer(direction,
                direction.ToVector3(), pointerSpawningPositionPosition));
        }

        Debug.Log("Building Pointers Was Spawned");
        return buildingPointers;
    }
    
    private bool CheckBuildingSettingAvailable()
    {
        return _currentBuilding != null && (_selectedBuilding != null && _selectedBuildPointer != null) 
               || _buildingIsPlacedOnGameField;
    }
    
    private void OnBuildingSelected()
    {
        if (_selectedBuilding is not BuildingWithChilds buildingWithChilds) return;

        _correctPoints.AddRange(buildingWithChilds.GetCorrectBuildPoints(_currentBuilding.BuildingType)); 
        
        if (_correctPoints == null|| !_correctPoints.Any())
        {
            Recycle();
            return;
        }
        
        Debug.Log("Building Was Selected");
    }

    private void OnPointerSelected(BuildPointer selectedBuildPointer)
    {
        var correctBuildPoint = _correctPoints.
            FirstOrDefault(buildPoint => buildPoint.Direction == selectedBuildPointer.Direction);
        if (correctBuildPoint == null) throw new InvalidOperationException();
        
        _gameField.TrySetBuilding(_currentBuilding, correctBuildPoint);
        Debug.Log("Pointer Was Selected");
    }

    public void AcceptSettingBuilding()
    {
        if (_currentBuildingIsReadyToSet == false)
            return;
        _placedBuildings.AddBuilding(_currentBuilding);
        Reset();
        IsBuilding = false;
        BuildingEndedOnThisFrame = true;
        Debug.Log("Setting Building Was Accepted");
    }

    public void DeclineSettingBuilding()
    {
        if (_currentBuilding == null) return;
        
        _currentBuilding.Destroy();
        Reset();
        Debug.Log("Setting Building Was Declined");
    }

    private void Reset()
    {
        _currentBuilding = null;
        _buildingIsPlacedOnGameField = false;
        Recycle();
        Debug.Log("Fields Was Reseted");
    }

    public void Recycle()
    {
        _selectedBuilding = null;
        _selectedBuildPointer = null;
        IsBuilding = false;
        _spawnedPointers.DestroyObjects();
        _spawnedPointers = null;
        _correctPoints = null;
    }
    
}

