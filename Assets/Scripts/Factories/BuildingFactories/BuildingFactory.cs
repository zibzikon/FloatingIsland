using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Factories.BuildingFactories
{
    public class BuildingFactory 
    {

        public Building GetNewBuilding(BuildingType buildingType, IBuildingsContainer buildingsContainer)
        {
            switch (buildingType)
            {
                case BuildingType.SupportPillar:
                    return new SupportPillarBuilding(buildingsContainer);
                case BuildingType.Wall:
                    return new WallBuilding(buildingsContainer);
                case BuildingType.Tower:
                    return new Turret(buildingsContainer);
                case BuildingType.Turret:
                    return new TowerBuilding(buildingsContainer);
                case BuildingType.WoodenCrafter:
                    return new WoodenCrafter(buildingsContainer);
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildingType), buildingType, null);
            }
        }
    }
}

