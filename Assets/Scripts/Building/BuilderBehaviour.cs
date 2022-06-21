using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Factories.Building;
using UnityEngine;

public class BuilderBehaviour : IUpdatable, IRecyclable, IBuildingsContainer
{
    private readonly Camera _mainCamera;

    private readonly GameField _gameField;
    
    private readonly BuildingFactory _buildingFactory;
    
    private readonly BuildingPointersFactory _buildingPointersFactory;

    private readonly BuildingsColection _setedBuildings = new();
    
    private bool _currentBuildingIsReadyToSet => CheckBuildingSettingAvialible();
    
    private Building _currentBuilding;
    private Building _selectedBuilding;
    
    private BuildingPointer _selectedBuildingPointer;
    private List<BuildingPointer> _spawnedPointers = new();
    
    private bool _buildingSettedOnGamefield;
    private bool _firstBuildingSetting = true;
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
        if (_currentBuilding != null)
        {
            if (Input.GetKeyDown(KeyCode.E) && _currentBuildingIsReadyToSet)
            {
                AcceptSettingBuilding();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                DeclineSettingBuilding();
            }
            else if ( Input.GetMouseButtonDown(0) && _selectedBuilding == null)
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
                     if (_gameField.TrySetBuilding(_currentBuilding, _gameField.GetCellByWorldPosition(hit.point), _firstBuildingSetting))
                        _buildingSettedOnGamefield = true;
                     _firstBuildingSetting = false;
                }
            }
            else if (Input.GetMouseButtonDown(0) && _selectedBuilding != null && _selectedBuildingPointer == null)
            {
                _selectedBuildingPointer = TrySelectItem()?.GetComponent<BuildingPointer>();
                if (_selectedBuildingPointer != null)
                {
                    OnPointerSelected(_selectedBuildingPointer,(BuildingWithChilds)_selectedBuilding);
                }
            }
        }
    }
    

    public void SpawnBuilding(BuildingType type)
    {
        if (_currentBuilding != null) return;

        _currentBuilding = _buildingFactory.GetNewBuilding(type);
        _currentBuilding.Initialize(this);
        if (BuildingWithChilds.IsBuildingWithChilds(_currentBuilding));
    }

    private List<BuildingPointer> SpawnBuiildingPointers(BuildingWithChilds building)
    {
        var buildingTrueSize = building.GetComponent<MeshRenderer>().bounds.size;
        var buildingTrueCenterPosition = building.GetComponent<MeshRenderer>().bounds.center;
        var builingPointers = new List<BuildingPointer>();
        var directions = building.GetGetCorrectBuildPointsDirections(_currentBuilding);
        foreach (var direction in directions)
        {
            builingPointers.Add(_buildingPointersFactory.GetNewBuildPointer(direction,
                direction.ToVector3(), buildingTrueCenterPosition + buildingTrueSize));
        }

        Debug.Log("Building Pointers Is Spawned");
        return builingPointers;
    }

    private Transform TrySelectItem()
    {
        var hitedCollider = GetRaycastHitByMousePosition(_mainCamera).collider;
        if (hitedCollider == null) return null;       
            var collisionObject = hitedCollider.GetComponent<CollisionObject>();
        if (collisionObject != null)
        {
            return collisionObject.Parent;
        }
        return null;
    }

    private static RaycastHit GetRaycastHitByMousePosition(Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit) ? hit : new RaycastHit();
    }
    
    private bool CheckBuildingSettingAvialible()
    {
        return _currentBuilding != null && (_selectedBuilding != null && _selectedBuildingPointer != null)|| _buildingSettedOnGamefield;
    }
    
    private void OnBuildingSelected()
    {
        if (BuildingWithChilds.IsBuildingWithChilds(_selectedBuilding))
        {
            var buildingWithChilds = (BuildingWithChilds)_selectedBuilding;
            _spawnedPointers = SpawnBuiildingPointers(buildingWithChilds);
            Debug.Log("Building Is Selected");

        }
    }

    private void OnPointerSelected(BuildingPointer selectedBuildingPointer, BuildingWithChilds selectedBuilding)
    {
        var correctBuildPoint = ((IBuildingContainer)selectedBuilding).GetCorrectBuildPoints(_currentBuilding).
            FirstOrDefault(buildPoint => buildPoint.Direction == selectedBuildingPointer.Direction);
        
        _gameField.TrySetBuilding(_currentBuilding,(IBuildingContainer)_selectedBuilding, correctBuildPoint, _firstBuildingSetting);
        _firstBuildingSetting = false;
        Debug.Log("Pointer Is Selected");
    }

    private void AcceptSettingBuilding()
    {
        if (_currentBuildingIsReadyToSet == false)
            throw new InvalidOperationException("Current building is not ready to set");
        _setedBuildings.AddBuilding(_currentBuilding);
        Reset();
        Debug.Log("Setting Building Is Accepted");

    }

    private void DeclineSettingBuilding()
    {
        _buildingFactory.Reclaim(_currentBuilding);
        Reset();
        Debug.Log("Setting Building Is Declined");
    }

    private void Reset()
    {
        _currentBuilding = null;
        _selectedBuilding = null;
        _selectedBuildingPointer = null;
        _spawnedPointers.DestroyObjects();
        _spawnedPointers.Clear();
        _buildingSettedOnGamefield = false;
        _firstBuildingSetting = true;
        Debug.Log("Fields Is Reseted");
    }

    public void Recycle()
    {
        Reset();
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

