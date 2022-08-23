using System;
using Enums;
using Factories.Item.View;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ItemCellView : MonoBehaviour, ISwitchable
{
    public UnityEvent OnClick { get; } = new ();
    
    [SerializeField] private TextMeshProUGUI _countText;

    [SerializeField] private ItemView _itemView;

    [SerializeField] private Button _button;
    
    private ItemsSpriteFactory _itemsSpriteFactory;
    public RectTransform RectTransform { get; private set;} 
    public ItemCell ItemCellModel { get; private set; }

    public void Initialize(ItemCell itemCellModel, ItemsSpriteFactory itemsSpriteFactory)
    {
        RectTransform = GetComponent<RectTransform>();
        _button.onClick.AddListener(()=> OnClick.Invoke());
        OnClick.AddListener(OnButtonClick);
        
        ItemCellModel = itemCellModel;

        _itemsSpriteFactory = itemsSpriteFactory;
        
        itemCellModel.ContentChanged += OnItemCellContentChanged;
        
        UpdateView();
    }

    private void OnButtonClick()
    {
        if ( ItemCellModel.Item == null) return;

        ItemCellModel.Item.Select();
    }
    
    protected  void OnEnable()
    {
        if(ItemCellModel == null) return;
        ItemCellModel.ContentChanged += OnItemCellContentChanged;
        OnClick.AddListener(OnButtonClick);
        UpdateView();
    }

    protected  void OnDisable()
    {
        if(ItemCellModel == null) return;
        ItemCellModel.ContentChanged -= OnItemCellContentChanged;
        OnClick.RemoveListener(OnButtonClick);
    }

    private void OnItemCellContentChanged(ItemCell obj)
    {
        UpdateView();
    }

    private void UpdateView()
    { 
        var content = ItemCellModel.Item;

        if (content.ItemType == ItemType.None)
        {
            _itemView.Disable();
            _countText.enabled = false;
            return;
        }
        
        _itemView.Enable();
        
        _countText.enabled = true;

        _itemView.Initialize(ItemCellModel.Item, _itemsSpriteFactory.Get(content.ItemType));
        
        _countText.text = ItemCellModel.ItemsCount.ToString();
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
