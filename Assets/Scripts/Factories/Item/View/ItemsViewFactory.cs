using System;
using Enums;
using UnityEngine;

namespace Factories.Item.View
{
    [CreateAssetMenu(fileName = "ItemsViewFactory", menuName = "Factories/Item/View/ItemsViewFactory", order = 0)]
    public class ItemsViewFactory : ScriptableObject
    {

        [SerializeField] private ItemView _wall;
        
        public ItemView Get(ItemType itemType, Transform parentTransform)
        {
            return itemType switch
            {
                ItemType.WalBuilding => InstantiateItemView(_wall, parentTransform),

                ItemType.GeneralPillarBuilding => InstantiateItemView(_wall, parentTransform),
                
                _ => throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null)
            };
        }

        private ItemView InstantiateItemView(ItemView itemViewPrefab, Transform parentTransform)
        {
            var instance =Instantiate( itemViewPrefab , parentTransform);
            
            return instance;
        }
    }
}