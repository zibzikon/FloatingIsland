using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequireItemToCraftViewGridLayoutGroup : GridLayoutGroup
{
    private List<RequireItemToCraftView> _items = new();
    [SerializeField] private RequireItemToCraftView _requireItemToCraftViewPrefab;
    [SerializeField] private bool a;

    public void Add(RequireItemToCraftView requireItemToCraftView)
    {
        requireItemToCraftView.transform.SetParent(transform);
    }
    
    private void CorrectScale()
    {
        var sizeDelta = rectTransform.sizeDelta;
        var correctedScale = new Vector2(sizeDelta.y, sizeDelta.y);
        
        if (_items.Count > 1)
        {
            correctedScale /= _items.Count;
        }

        cellSize = correctedScale;
    }
    
    public void Remove(RequireItemToCraftView requireItemToCraftView)
    {
        _items.Remove(requireItemToCraftView);
        Destroy(requireItemToCraftView.gameObject);
    }

    public void Clear()
    {
        foreach (var item in _items)
        {
            Destroy(item.gameObject);
        }
        
        _items.Clear();
    }
}
