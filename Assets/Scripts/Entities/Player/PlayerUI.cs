
using System;
using System.Runtime.CompilerServices;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour, IUpdatable
{
    private Player _playerModel;
    [SerializeField] private CrafterUI _crafterUI;
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private SelectedObjectUI _selectedObjectUI;

    private static PlayerUI _playerUIInstance;
    public static PlayerUI Instance => _playerUIInstance;
    
    public void Initialize(Player playerModel)
    {
        _playerUIInstance = this; 
        _playerModel = playerModel;
        _inventoryView.Initialize(_playerModel.Inventory);
        SubscribeEvents();
        _selectedObjectUI.Initialize(_playerModel);
      
     }
     private void OnEnable()
     {
         SubscribeEvents();
     }

     private void OnDisable()
     { 
         UnSubscribeEvents();
     }
     
     private void SubscribeEvents()
     {
         if(_playerModel == null) return;
         _playerModel.SelectableObjectSelected += OnSelectableObjectSelected;
         _playerModel.SelectableObjectDeselected += OnSelectableObjectDeselected;
     }
     
     private void UnSubscribeEvents()
     {
         if(_playerModel == null) return;
         _playerModel.SelectableObjectSelected -= OnSelectableObjectSelected;
         _playerModel.SelectableObjectDeselected -= OnSelectableObjectDeselected;
     }
     
     public void OnUpdate()
     {
         if (Keyboard.current.eKey.wasPressedThisFrame)
         {
             if (_playerModel.TryOpenInventory() == false)
             {
                 _playerModel.CloseInventory();
             }
         }

         if (Keyboard.current.escapeKey.wasPressedThisFrame)
         {
             CloseAllWindows();
         }

         if (Mouse.current.leftButton.wasPressedThisFrame && GeneralGameSettings.RayCastIsBlocked == false) 
         {
             if (_playerModel.TrySelectSelectableObject() == false)
             {
                 _playerModel.TryMoveToHitGameFieldPoint();
             }
         }
     }

     private void OnSelectableObjectSelected(ISelectable selectable)
     {
         _selectedObjectUI.SetSelectedObject(selectable);
     }
     
     private void OnSelectableObjectDeselected()
     {
         _selectedObjectUI.RemoveSelectedObject();
     }
     
     public void OpenCrafterWindow(ICrafter crafter)
     {
         _crafterUI.Enable();
         GeneralGameSettings.RayCastIsBlocked = true;
         _crafterUI.Initialize(_playerModel.Inventory, crafter);
     }
    
     
     public void CloseAllWindows()
     {
         CloseCrafterWindow();
         _playerModel.CloseInventory();
         GeneralGameSettings.RayCastIsBlocked = false;
     }
     
     private void CloseCrafterWindow()
     {
         _crafterUI.Disable();
     }
}
