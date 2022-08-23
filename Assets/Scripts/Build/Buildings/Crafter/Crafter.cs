using System;
using System.Collections.Generic;
using System.Linq;
using Enums;

public abstract class Crafter : Building, IInteractable, ICrafter
{
    protected abstract List<CraftItem> AllCraftItems { get; }
    public IEnumerable<CraftItem> CraftItems => AllCraftItems;

    public bool TryCraftItem(ItemType itemType, ItemsContainer itemsContainer)
    {
        var craftItem = AllCraftItems.FirstOrDefault(craftItem => craftItem.Item.ItemType == itemType);
        if (craftItem == null || ValidateCraftingItem(craftItem, itemsContainer) == false) return false;
        
        foreach (var requireItemsToCraft in craftItem.RequireItemsToCraft)
        {
            if (!itemsContainer.TryRemoveItems(requireItemsToCraft.ItemType, requireItemsToCraft.ItemsCount))
            {
                throw new InvalidOperationException();
            }
        }

        var excess = itemsContainer.CreateAndAddItemsToContainerAndReturnNotEstablished(itemType,
                craftItem.Item.ItemsCount);
            return true;
    }
    
    public bool ValidateCraftingItem(CraftItem craftItem, ItemsContainer itemsContainer)
    {
        return craftItem != null
               && craftItem.RequireItemsToCraft.All(requireItemToCraft =>
                   itemsContainer.ContainsItem(requireItemToCraft.ItemType, requireItemToCraft.ItemsCount));
    }
    
    public bool ValidateCraftingItem(ItemType itemType, ItemsContainer itemsContainer)
    {
        var craftItem = AllCraftItems.FirstOrDefault(craftItem => craftItem.Item.ItemType == itemType);

        return ValidateCraftingItem(craftItem, itemsContainer);
    }

    private void Open()
    {
        PlayerUI.Instance.OpenCrafterWindow(this);
    }
    
    public void Interact(IInteractor sender)
    {
        Open();
    }
}
