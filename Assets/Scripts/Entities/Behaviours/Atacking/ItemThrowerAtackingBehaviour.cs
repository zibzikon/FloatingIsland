namespace Units.Behaviours.Atacking
{
    public class ItemThrowerAtackingBehaviour : IAtackable
    {
        private IMovable _movingBehaviour;
        public bool AttackingStarted { get; private set; }

        public ItemThrowerAtackingBehaviour(IMovable movingBehaviour)
        {
            _movingBehaviour = movingBehaviour;
        }
        
        public void Atack(ITarget target)
        {
            AttackingStarted = true;
        }
    }
}