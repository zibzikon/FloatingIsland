using System;
using Enums;
using UnityEngine;

namespace Factories.Item.View
{
    [CreateAssetMenu(fileName = "ItemsSpriteFactory", menuName = "Factories/Item/View/ItemsSpriteFactory", order = 0)]
    public class ItemsSpriteFactory : ScriptableObject
    {

        [SerializeField] private Sprite _wallSprite;
        
        [SerializeField] private Sprite _supportPillarBuildingSprite;
        
        [SerializeField] private Sprite _woodSprite;
        
        public Sprite Get(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.None => null,
                
                ItemType.WallBuilding => _wallSprite,

                ItemType.SupportPillarBuilding => _supportPillarBuildingSprite,
                
                ItemType.Wood => _woodSprite,
                
                _ => throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null)
            };
        }
    }
}