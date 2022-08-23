using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private CountableItem _item;

    private int _itemCount;

    public void Initialize(CountableItem item)
    {
        _item = item;
        _itemCount = item.ItemsCount;
    }

    public void Collect(ItemsContainer itemsContainer)
    {
        var excess = itemsContainer.CreateAndAddItemsToContainerAndReturnNotEstablished(_item.ItemType, _itemCount);
        _itemCount = excess;
        
        if (_itemCount <= 0)
            Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
        _item = new CountableItem();
    }
}
