
using System.Collections.Generic;
using Enums;

public interface ICrafter
{
    public IEnumerable<CraftItem> CraftItems { get; }
    public bool TryCraftItem(ItemType itemType, ItemsContainer itemsContainer);
    public bool ValidateCraftingItem(CraftItem craftItem, ItemsContainer itemsContainer);
}
