    using UnityEngine;

    public interface IBuildingsContainer
    {
        public Building TryGetBuildingByPosition(Vector3Int position);
    }