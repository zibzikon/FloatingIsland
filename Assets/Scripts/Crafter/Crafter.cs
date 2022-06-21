using Enums;
using Factories.Crafter;
using Interact;

namespace Crafter
{
    public abstract class Crafter : InteractableBuilding, ICrafter
    {
        private readonly CrafterBehaviourFactory _crafterBehaviourFactory = new CrafterBehaviourFactory();
        public ItemType[] CraftableItems { get; private set; }
        public ICrafterBehaviour CrafterBehaviour { get; private set;}
        public abstract Tier Tier { get; }

        public void Initialize()
        {
            CrafterBehaviour = _crafterBehaviourFactory.Get(Tier);
            CraftableItems = CrafterBehaviour.CraftableItems;
        }
    }
}