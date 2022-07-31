
using Factories.Item.View;
using UnityEngine;
using UnityEngine.UI;

public class CrafterItemView : MonoBehaviour
{
    [SerializeField] private ItemsSpriteFactory _itemsSpriteFactory;
    
    [SerializeField] private ItemsViewFactory _itemsViewFactory;
    
    [SerializeField] private Button _carftButton;

    [SerializeField] private Image _itemImage;

    [SerializeField] private RequireItemToCraftViewGridLayoutGroup _requireItemsToCraftContainer;

    [SerializeField] private RequireItemToCraftView _requireItemToCraftViewPrefab;
    
    
    private CraftItem _model;

    private ICrafter _crafter;

    private ItemsContainer _itemsContainer;

    public void Initialize(CraftItem model, ICrafter crafter, ItemsContainer itemsContainer)
    {
        _crafter = crafter;
        _model = model;
        _itemsContainer = itemsContainer;
        SubscribeEvents();
        UpdateView();
    }

    private void SubscribeEvents()
    {
        _carftButton.onClick.AddListener(OnBuyItemButtonPressed);
    }

    private void OnBuyItemButtonPressed()
    {
        _crafter.TryCraftItem(_model.Item.ItemType, _itemsContainer);
    }
    
    private void UpdateView()
    {
        _itemImage.sprite = _itemsSpriteFactory.Get(_model.Item.ItemType);

        _requireItemsToCraftContainer.Clear();
        foreach (var requireItemToCraft in _model.RequireItemsToCraft)
        {
            var requireItemToCraftViewInstance = Instantiate(_requireItemToCraftViewPrefab);
            requireItemToCraftViewInstance.Initialize(requireItemToCraft);
           _requireItemsToCraftContainer.Add(requireItemToCraftViewInstance);
        }
    }
}
