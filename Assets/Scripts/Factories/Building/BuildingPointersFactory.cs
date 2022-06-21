using System.Collections.Generic;
using UnityEngine;

namespace Factories.Building
{
    [CreateAssetMenu(menuName = @"Factories/Building/BuildingPointersFactory")]
    public class BuildingPointersFactory: ScriptableObject
    {
        [SerializeField] 
        private BuildingPointer _buildPointerPrefab;

        private Dictionary<Direction3, Quaternion> _directionToRtation = new()
        {
            [Direction3.Right] = new Quaternion(),
            [Direction3.Left] = new Quaternion(),
            [Direction3.Foward] = new Quaternion(),
            [Direction3.Back] = new Quaternion(),
            [Direction3.Up] = new Quaternion(),
            [Direction3.Down] = new Quaternion()
        };

        public BuildingPointer GetNewBuildPointer(Direction3 direction, Vector3 offset, Vector3 spawnPosition)
        {
            var instance = Instantiate(_buildPointerPrefab, spawnPosition + offset, _directionToRtation[direction]).GetComponent<BuildingPointer>();
            instance.Initialize( direction);
            return instance;
        }
        public void DestroyBuildingPointer( BuildingPointer buildingPointer)
        {
            Destroy(buildingPointer.gameObject);
        }
    }
}