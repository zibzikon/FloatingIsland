using Enums;
using Factories.Item.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequireItemToCraftView : MonoBehaviour
{
    [SerializeField] private ItemsSpriteFactory _itemsSpriteFactory;
    
    [SerializeField] private Image _image;

    [SerializeField] private TextMeshProUGUI _itemsCountText;
    private CountableItem _craftItem;

    public void Initialize(CountableItem craftItem)
    {
        _craftItem = craftItem;
        UpdateView();
    }
    
    public void UpdateView()
    {
        _image.sprite = _itemsSpriteFactory.Get(_craftItem.ItemType);
        _itemsCountText.text = _craftItem.ItemsCount.ToString();
    }

}
