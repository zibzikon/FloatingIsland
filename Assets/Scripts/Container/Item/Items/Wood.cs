using Container.Behaviours;

namespace Container
{
    public class Wood:Item
    {
        protected override ICraftable _craftBehaviour { get; } = new NonCraftableItemBehaviour();
        protected override IPickable _pickBehaviour { get; } = new PickableItemBehaviour();
        protected override IDropable _dropBehaviour { get; } = new DropableItemBehaviour();
        protected override IPlaceble _placeBehaviour { get; } = new NonPlacebleItemBehaviour();
    }
}