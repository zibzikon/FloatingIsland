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
        [SerializeField]private global::Building _woodenCrafterPrefab;
        
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
                case BuildingType.WoodenCrafter:
                    return GetNewBuilding(_woodenCrafterPrefab);
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
