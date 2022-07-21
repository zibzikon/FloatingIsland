using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Factories.Building;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderBehaviour : IUpdatable, IRecyclable
{
    private readonly Camera _mainCamera;

    private readonly GameField _gameField;
    
    private readonly BuildingFactory _buildingFactory;
    
    private readonly BuildingPointersFactory _buildingPointersFactory;

    private readonly BuildingsColection _setedBuildings = new();

    private bool _currentBuildingIsReadyToSet => CheckBuildingSettingAvialible();
    
    private Building _currentBuilding;
    private Building _selectedBuilding;
    
    private BuildPointer _selectedBuildPointer;
    private List<BuildPointer> _spawnedPointers = new();
    private List<BuildPoint> _correctPoints = new();
    
    private bool _buildingSettedOnGameField;
    
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
        foreach (var building in _setedBuildings)
        {
            building.OnUpdate();
        }
    }
    
    public void SetBuilding()
    {
        if (_currentBuilding == null) return;
        
        if ( _selectedBuilding == null)
        {            

            var selectedBuilding= TrySelectItem()?.GetComponent<BuildingWithChilds>();
            if (selectedBuilding != null && selectedBuilding != _currentBuilding)
            {
                _selectedBuilding = selectedBuilding;
                OnBuildingSelected();
            }
            else if(Building.CanBeSettedOnGameField(_currentBuilding))
            {
                var hit = GetRaycastHitByMousePosition(_mainCamera);
                _currentBuilding.SetPositionOnGameField(_gameField.GetCellByPosition
                    (GameField.ConvertWorldToGameFieldPosition(hit.point)).Position);
                if (_gameField.TrySetBuilding(_currentBuilding))
                    _buildingSettedOnGameField = true;
            }
        }
        else if (_selectedBuilding != null && _selectedBuildPointer == null)
        {
            _selectedBuildPointer = TrySelectItem()?.GetComponent<BuildPointer>();
            if (_selectedBuildPointer != null)
            {
                OnPointerSelected(_selectedBuildPointer);
            }
        } 
    }
    
    public void SpawnBuilding(BuildingType type)
    {
        if (_currentBuilding != null) return;
        _currentBuilding = _buildingFactory.GetNewBuilding(type);
        _currentBuilding.Initialize();
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

    private Transform TrySelectItem()
    {
        var hitedCollider = GetRaycastHitByMousePosition(_mainCamera).collider;
        if (hitedCollider == null) return null;       
            var collisionObject = hitedCollider.GetComponent<CollisionObject>();
        return collisionObject != null ? collisionObject.Parent : null;
    }

    private static RaycastHit GetRaycastHitByMousePosition(Camera camera)
    {
        var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        return Physics.Raycast(ray, out var hit) ? hit : new RaycastHit();
    }
    
    private bool CheckBuildingSettingAvialible()
    {
        return _currentBuilding != null && (_selectedBuilding != null && _selectedBuildPointer != null) 
               || _buildingSettedOnGameField;
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
        _spawnedPointers = 
            SpawnBuildingPointers((buildingWithChilds.CenterPosition + buildingWithChilds.transform.position) / 2,
                buildingWithChilds.Size);
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
        _setedBuildings.AddBuilding(_currentBuilding);
        Reset();
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
        _buildingSettedOnGameField = false;
        Recycle();
        Debug.Log("Fields Was Reseted");
    }

    public void Recycle()
    {
        _selectedBuilding = null;
        _selectedBuildPointer = null;
        _spawnedPointers.DestroyObjects();
        _spawnedPointers.Clear();
        _correctPoints.Clear();
    }
    
}

