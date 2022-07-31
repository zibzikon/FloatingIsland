using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private CountableItem _item;

    public int ItemCount;

    public void Initialize(CountableItem item)
    {
        _item = item;
    }

    public void Collect(ItemsContainer itemsContainer)
    {
      var excess = itemsContainer.CreateAndAddItemsToContainerAndReturnNotEstablished(_item.ItemType, _item.ItemsCount);
      if (excess <= 0) Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
        _item = new CountableItem();
    }
}
