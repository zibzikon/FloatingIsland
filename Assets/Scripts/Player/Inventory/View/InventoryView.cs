using System;

public class InventoryView : ContainerView
{
   private Inventory _inventoryModel;
   protected override ItemsContainer ItemsContainerModel => _inventoryModel;

   public void Initialize(Inventory inventory)
   {
      _inventoryModel = inventory;
      
      _inventoryModel.Opened += Enable;
      _inventoryModel.Closed += Disable;
      
      Initialize();
   }
   
   private void OnDisable()
   {
      if (_inventoryModel == null) return;

      _inventoryModel.Opened -= Enable;
      _inventoryModel.Closed -= Disable; 
   }

   private void OnEnable()
   {
      if (_inventoryModel == null) return;

      _inventoryModel.Opened += Enable;
      _inventoryModel.Closed += Disable; 
   }
}
