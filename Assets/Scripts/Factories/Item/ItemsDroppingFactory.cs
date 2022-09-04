using System;
using Enums;
using Extentions;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsDroppingFactory", menuName = "Factories/Item/ItemsDroppingFactory", order = 0)]

public class ItemsDroppingFactory : ScriptableObject
{
    [SerializeField] private ItemDrop _woodenAxeDrop;
    [SerializeField] private ItemDrop _woodDrop;
    [SerializeField] private ItemDrop _supportPillarBuildingDrop;
    [SerializeField] private ItemDrop _wallBuildingDrop;

    public void Get(CountableItem item, Vector3 position)
    {
        switch (item.ItemType)
        {
            case ItemType.SupportPillarBuilding:
                SpawnItemDrop(_supportPillarBuildingDrop, item, position);
                break;
            case ItemType.WallBuilding:
                SpawnItemDrop(_wallBuildingDrop, item, position);
                break;
            case ItemType.Wood :
                SpawnItemDrop(_woodDrop, item, position);
                break;
            case ItemType.WoodenAxe:
                SpawnItemDrop(_woodenAxeDrop, item, position);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(item), item, null);
        }
    }

    private void SpawnItemDrop(ItemDrop itemDrop, CountableItem item, Vector3 position)
    {
        var dropInstance = Instantiate(itemDrop, position, Quaternion.Euler
            (VectorExtensions.GetRandomVector(new Vector3(0,0,0), new Vector3(0,360,0))));
        
        dropInstance.Initialize(item);
    }

 
}
