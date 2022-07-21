namespace Units.Behaviours.Atacking
{
    public class ItemThrowerAtackingBehaviour : IAtackable
    {
        private IMovable _movingBehaviour;
        public bool AttackingStarted { get; private set; }
        
        public bool AttackingIsAvailable { get; }
        public void SetAttackingTarget(ITarget target)
        {
            
        }


        public ItemThrowerAtackingBehaviour(IMovable movingBehaviour)
        {
            _movingBehaviour = movingBehaviour;
        }

        public void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}