using System.Linq;

namespace Container.Behaviours
{
    public class CraftableItemBehaviour:ICraftable
    {
        private CellContent[] _itemsToCraft;
        private CellContent[] _clientItems;
        public void Craft()
        {
            
        }

        private bool ValidateCrafting()
        {
            return true;
        }
    }
}