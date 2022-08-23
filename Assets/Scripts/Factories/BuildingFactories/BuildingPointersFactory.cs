using System.Collections.Generic;
using UnityEngine;

namespace Factories.BuildingFactories
{
    [CreateAssetMenu(menuName = @"Factories/Building/BuildingPointersFactory")]
    public class BuildingPointersFactory: ScriptableObject
    {
        [SerializeField] 
        private BuildPointer _buildPointerPrefab;

        private Dictionary<Direction3, Quaternion> _directionToRtation = new()
        {
            [Direction3.Right] = new Quaternion(),
            [Direction3.Left] = new Quaternion(),
            [Direction3.Foward] = new Quaternion(),
            [Direction3.Back] = new Quaternion(),
            [Direction3.Up] = new Quaternion(),
            [Direction3.Down] = new Quaternion()
        };

        public BuildPointer GetNewBuildPointer(Direction3 direction, Vector3 offset, Vector3 spawnPosition)
        {
            var instance = Instantiate(_buildPointerPrefab, spawnPosition + offset, _directionToRtation[direction]).GetComponent<BuildPointer>();
            instance.Initialize( direction);
            return instance;
        }
        public void DestroyBuildingPointer( BuildPointer buildPointer)
        {
            Destroy(buildPointer.gameObject);
        }
    }
}