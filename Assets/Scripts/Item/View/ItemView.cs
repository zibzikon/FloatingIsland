using System;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour , ISwitchable
{
    [SerializeField] private Image _itemImage;
    private Item _itemModel;

    public void Initialize(Item itemModel, Sprite sprite)
    {
        _itemModel = itemModel;
        _itemImage.sprite = sprite;
    }
    
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
       gameObject.SetActive(false);
    }

    private void UpdateView()
    {
    }
}
