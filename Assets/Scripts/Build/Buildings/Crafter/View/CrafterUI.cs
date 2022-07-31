using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class CrafterUI : MonoBehaviour, ISwitchable
{
    [SerializeField] private CraftItemsContainer _craftItemsContainer;
    [SerializeField] private CrafterItemView _crafterItemViewPrefab;
    
    private ItemsContainer _itemsContainer;
    
    private ICrafter _model;

    public void Initialize(ItemsContainer itemsContainer, ICrafter crafter)
    {
        _itemsContainer = itemsContainer;
        _model = crafter;
        UpdateView();
    }

    public void UpdateView()
    {
        _craftItemsContainer.Clear();
        
        foreach (var craftItem in _model.CraftItems)
        {
            var crafterItemViewInstance = Instantiate(_crafterItemViewPrefab);
            crafterItemViewInstance.Initialize(craftItem, _model, _itemsContainer);
            
            _craftItemsContainer.Add(crafterItemViewInstance);
        }
    }
    
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
