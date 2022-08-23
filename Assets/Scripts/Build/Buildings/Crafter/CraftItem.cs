
using System;
using System.Collections.Generic;
using System.Linq;
using Enums;

public class CraftItem
{
    public readonly CountableItem Item;
    public readonly List<CountableItem> RequireItemsToCraft;

    public CraftItem(CountableItem countableItem, List<CountableItem> requireItemsToCraft)
    {
        Item = countableItem;
        RequireItemsToCraft = requireItemsToCraft;
        CorrectRequireItemsToCraft();
    }

    private void CorrectRequireItemsToCraft()
    {
        if (RequireItemsToCraft.Any(requireItemToCraft =>
                RequireItemsToCraft.Any(item => !item.Equals(requireItemToCraft) && item.ItemType
                == requireItemToCraft.ItemType)))
        {
            throw new Exception("RequireItemsToCraft cannot contains the same items by type " +
                                "(you are invalid and debil)");
        }
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