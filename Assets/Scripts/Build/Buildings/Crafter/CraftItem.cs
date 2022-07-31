
using System.Collections.Generic;
using Enums;

public class CraftItem
{
    public readonly CountableItem Item;
    public readonly List<CountableItem> RequireItemsToCraft;

    public CraftItem(CountableItem countableItem, List<CountableItem> requireItemsToCraft)
    {
        Item = countableItem;
        RequireItemsToCraft = requireItemsToCraft;
    }
}

public struct CountableItem
{
    public readonly ItemType ItemType;
    public readonly int ItemsCount;

    public CountableItem(ItemType itemType, int itemsCount)
    {
        ItemType = itemType;
        ItemsCount = itemsCount;
    }
}