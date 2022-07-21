
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
  [SerializeField] private InventoryView _inventoryView;

  public void Initialize(Inventory playerInventory)
  { 
     _inventoryView.Initialize(playerInventory);
  }
}
