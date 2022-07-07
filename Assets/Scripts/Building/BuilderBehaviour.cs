using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Factories.Building;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderBehaviour : IUpdatable, IRecyclable, IBuildingsContainer
{
    private readonly Camera _mainCamera;

    private readonly GameField _gameField;
    
    private readonly BuildingFactory _buildingFactory;
    
    private readonly BuildingPointersFactory _buildingPointersFactory;

    private readonly BuildingsColection _setedBuildings = new();

    private readonly BuildPointsFactory _buildPointsFactory;
    private bool _currentBuildingIsReadyToSet => CheckBuildingSettingAvialible();
    
    private Building _currentBuilding;
    private Building _selectedBuilding;
    
    private BuildingPointer _selectedBuildingPointer;
    private List<BuildingPointer> _spawnedPointers = new();
    private List<BuildPoint> _correctPoints = new();
    
    private bool _buildingSetedOnGameField;
    private bool _firstBuildingSetting = true;
    public BuilderBehaviour(GameField gameField,BuildingFactory buildingFactory, 
        BuildingPointersFactory buildingPointersFactory, BuildPointsFactory buildPointsFactory, Camera mainCamera)
    {
        _gameField = gameField;
        _buildingFactory = buildingFactory;
        _buildingPointersFactory = buildingPointersFactory;
        _buildPointsFactory = buildPointsFactory;
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
                if (_gameField.TrySetBuilding(_currentBuilding, _gameField.GetCellByPosition(GameField.ConvertWorldToGameFieldPosition(hit.point)),
                        _firstBuildingSetting))
                    _buildingSetedOnGameField = true;
                _firstBuildingSetting = false;
            }
        }
        else if (_selectedBuilding != null && _selectedBuildingPointer == null)
        {
            _selectedBuildingPointer = TrySelectItem()?.GetComponent<BuildingPointer>();
            if (_selectedBuildingPointer != null)
            {
                OnPointerSelected(_selectedBuildingPointer);
            }
        } 
    }
    
    public void SpawnBuilding(BuildingType type)
    {
        if (_currentBuilding != null) return;

        _currentBuilding = _buildingFactory.GetNewBuilding(type);
        if (BuildingWithChilds.IsBuildingWithChilds(_currentBuilding));
    }

    private List<BuildingPointer> SpawnBuiildingPointers(Vector3 position, Vector3 size)
    {
      
        var pointerSpawningPositionPosition = position + size;
        var buildingPointers = new List<BuildingPointer>();
        var settedDirections = new List<Direction3>();
        foreach (var direction in _correctPoints.Select(point => point.Direction).
                     Where(direction => !settedDirections.Contains(direction)))
        {
            if (direction == Direction3.Zero) continue;

            settedDirections.Add(direction);
            buildingPointers.Add(_buildingPointersFactory.GetNewBuildPointer(direction,
                direction.ToVector3(), pointerSpawningPositionPosition));
        }

        Debug.Log("Building Pointers Is Spawned");
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
        return _currentBuilding != null && (_selectedBuilding != null && _selectedBuildingPointer != null) 
               || _buildingSetedOnGameField;
    }
    
    private void OnBuildingSelected()
    {
        if (!BuildingWithChilds.IsBuildingWithChilds(_selectedBuilding)) return;
        
        var buildingWithChilds = (BuildingWithChilds)_selectedBuilding;
        _correctPoints.AddRange(_buildPointsFactory.Get(_currentBuilding, _selectedBuilding));
        
        if (_correctPoints == null)
        {
            Recycle();
            return;
        }
        _spawnedPointers = 
            SpawnBuiildingPointers(buildingWithChilds.GetComponent<MeshRenderer>().bounds.center,
                buildingWithChilds.GetComponent<MeshRenderer>().bounds.size);
        Debug.Log("Building Is Selected");
    }

    private void OnPointerSelected(BuildingPointer selectedBuildingPointer)
    {
        var correctBuildPoint = _correctPoints.
            FirstOrDefault(buildPoint => buildPoint.Direction == selectedBuildingPointer.Direction);
        if (correctBuildPoint == null) throw new InvalidOperationException();
        
        _gameField.TrySetBuilding(_currentBuilding, correctBuildPoint,
            _firstBuildingSetting);
        _firstBuildingSetting = false;
        Debug.Log("Pointer Is Selected");
    }

    public void AcceptSettingBuilding()
    {
        if (_currentBuildingIsReadyToSet == false)
            return;
        _setedBuildings.AddBuilding(_currentBuilding);
        Reset();
        Debug.Log("Setting Building Is Accepted");

    }

    public void DeclineSettingBuilding()
    {
        _currentBuilding.Die();
        Reset();
        Debug.Log("Setting Building Is Declined");
    }

    private void Reset()
    {
        _currentBuilding = null;
        _buildingSetedOnGameField = false;
        _firstBuildingSetting = true;
        Recycle();
        Debug.Log("Fields Is Reseted");
    }

    public void Recycle()
    {
        _selectedBuilding = null;
        _selectedBuildingPointer = null;
        _spawnedPointers.DestroyObjects();
        _spawnedPointers.Clear();
        _correctPoints.Clear();
    }

    public Building TryGetBuildingByPosition(Vector3Int position)
    {
        foreach (var building in _setedBuildings)
        {
            if (building.PositionOnGameField == position)
            {
                return building;
            }
        }

        return null;
    }
}

