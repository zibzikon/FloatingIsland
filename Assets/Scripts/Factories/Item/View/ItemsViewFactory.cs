using System;
using Enums;
using UnityEngine;

namespace Factories.Item.View
{
    [CreateAssetMenu(fileName = "ItemsViewFactory", menuName = "Factories/Item/View/ItemsViewFactory", order = 0)]
    public class ItemsViewFactory : ScriptableObject
    {
        [SerializeField] private ItemsSpriteFactory _itemsSpriteFactory;
        
        [SerializeField] private ItemView _iitemView;
        
        public ItemView Get(global::Item item, Transform parentTransform)
        {
           return InstantiateItemView(_itemsSpriteFactory.Get(item.ItemType), item, parentTransform);
        }

        private ItemView InstantiateItemView(Sprite itemSprite, global::Item item, Transform parentTransform)
        {
            var instance =Instantiate( _iitemView , parentTransform);
            instance.Initialize(item, itemSprite);
            
            return instance;
        }
    }
}