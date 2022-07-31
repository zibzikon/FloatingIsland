using System;
using Enums;

namespace Factories.Item
{
    public class ItemsFactory
    {
        public global::Item Get(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.None => new DefaultItem(),
                ItemType.Wood => new WoodItem(),
                ItemType.WoodenAxe => new WoodenAxe(),
                ItemType.SupportPillarBuilding => new SupportPillarBuildingItem(),
                ItemType.WallBuilding => new WallBuildingItem(),
                _ => throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null)
            };
        }
    }
}