using System;
using Factories.Item.View;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ItemCellView : Button, ISwitchable
{
    [SerializeField] private ItemsViewFactory _itemsViewFactory;
    public RectTransform RectTransform { get; private set;} 
    public ItemCell ItemCellModel { get; private set; }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        onClick.AddListener(OnButtonClick);
    }

    public void Initialize(ItemCell itemCellModel)
    {
        ItemCellModel = itemCellModel;
        
        itemCellModel.ContentChanged += OnItemCellContentChanged;
        
        UpdateView();
    }

    private void OnButtonClick()
    {
        if ( ItemCellModel.Content == null) return;

        ItemCellModel.Content.Select();
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        if(ItemCellModel == null) return;
        ItemCellModel.ContentChanged += OnItemCellContentChanged;
        onClick.AddListener(OnButtonClick);
        UpdateView();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        if(ItemCellModel == null) return;
        ItemCellModel.ContentChanged -= OnItemCellContentChanged;
        onClick.RemoveListener(OnButtonClick);
    }

    private void OnItemCellContentChanged(ItemCell obj)
    {
        UpdateView();
    }

    private void UpdateView()
    { 
        var content = ItemCellModel.Content;
        
        if (content == null) return;
        
        var itemView = _itemsViewFactory.Get(content.ItemType, transform);
       
       itemView.transform.position = transform.position;
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
