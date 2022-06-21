using System.Collections.Generic;
using UnityEngine;

namespace Factories.Building
{
    [CreateAssetMenu(fileName = "ConstructionsFactory", menuName = "Factories/Building/ConstructionFactory")]
    public class ConstructionsFactory : ScriptableObject
    {
        [SerializeField] private List<Construction> _constructions;
        public Construction TryGetConstruction(global::Building building, IBuildingsContainer buildingsContainer)
        {
            foreach (var construction in _constructions)
            {
                if (construction.ValidateBuilding(building.PositionOnGameField, buildingsContainer))
                {
                    return construction;
                }    
            }

            return null;
        }
    }
}