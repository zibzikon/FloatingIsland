using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Factories.BuildingFactories
{
    [CreateAssetMenu(menuName = @"Factories/Building/BuildingFactory")]
    public class BuildingFactory : ScriptableObject
    {
        [SerializeField] private List<global::Building> _buildings;

        private Dictionary<BuildingType, global::Building> _buildingsDictionary;

        public void Initialize()
        {
            InitializeBuildingDictionary();
        }
        
        private void InitializeBuildingDictionary()
        {
            _buildingsDictionary = new();
            foreach (var building in _buildings)
            {
                _buildingsDictionary.Add(building.BuildingType, building);
            }
        }

        public global::Building GetNewBuilding(BuildingType buildingType)
        {
            return GetNewBuilding(_buildingsDictionary[buildingType]);
        }

        private global::Building GetNewBuilding(Building prefab)
        {
            var instance = Instantiate(prefab);
            return instance;
        }
    }
}

