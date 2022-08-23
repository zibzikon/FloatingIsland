using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factories.BuildingFactories
{
    [CreateAssetMenu(fileName = "BuildingTilesFactory", menuName = "Factories/Building/BuildingTilesFactory", order = 0)]
    public class BuildingTilesFactory : ScriptableObject
    {
        [SerializeField] private List<BuildingTile> _buildingTiles;

        private Dictionary<BuildingType, BuildingTile> _buildingTilesDictionary;

        public void Initialize()
        {
            InitializeBuildingTilesDictionary();
        }
        
        private void InitializeBuildingTilesDictionary()
        {
            _buildingTilesDictionary = new();
            foreach (var buildingTile in _buildingTiles)
            {
                _buildingTilesDictionary.Add(buildingTile.BuildingType, buildingTile);
            }
        }

        public BuildingTile Get(BuildingType buildingType)
        {
            if (_buildingTilesDictionary == null)
                throw new NullReferenceException($"the object {_buildingTilesDictionary} is null." +
                                                 $" Please initialize BuildingTilesFactory before trying to access it");
            
            return Spawn(_buildingTilesDictionary[buildingType]);
        }

        private BuildingTile Spawn(BuildingTile buildingTilePrefab)
        {
            return Instantiate(buildingTilePrefab);
        }
    }
}