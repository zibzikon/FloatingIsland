using System.Collections.Generic;
using UnityEngine.UI;

public class CraftItemsContainer : ScrollRect
{
    private readonly List<CrafterItemView> _itemViews = new();

    public void Add(CrafterItemView crafterItemView)
    {
        _itemViews.Add(crafterItemView);
        crafterItemView.transform.SetParent(content.transform);
    }
    
    public void Remove(CrafterItemView crafterItemView)
    {
        _itemViews.Remove(crafterItemView);
        Destroy(crafterItemView);
    }

    public void Clear()
    {
        foreach (var itemView in _itemViews)
        {
            Destroy(itemView.gameObject);
        }
        
        _itemViews.Clear();
    }
}
