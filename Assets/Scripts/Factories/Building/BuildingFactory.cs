using System;
using UnityEngine;

namespace Factories.Building
{
    [CreateAssetMenu(menuName = @"Factories/Building/BuildingFactory")]
    public class BuildingFactory : ScriptableObject
    {
        [SerializeField]private global::Building _supportPillarPrefab;
        [SerializeField]private global::Building _wallPrefab;
        [SerializeField]private global::Building _towerPrefab;
        [SerializeField]private global::Building _turretPrefab;
        
        public global::Building GetNewBuilding(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.SupportPillar:
                    return GetNewBuilding(_supportPillarPrefab);
                case BuildingType.Wall:
                    return GetNewBuilding(_wallPrefab);
                case BuildingType.Tower:
                    return GetNewBuilding(_towerPrefab);
                case BuildingType.Turret:
                    return GetNewBuilding(_turretPrefab);
            }
            
            throw new NullReferenceException();
        }

        private global::Building GetNewBuilding(global::Building prefab)
        {
            var instance = Instantiate(prefab);
            return instance;
        }

    
    }
}
