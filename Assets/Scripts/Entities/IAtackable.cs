
public interface IAtackable : IUpdatable
{
    public bool AttackingStarted { get; }
    
    public bool AttackingIsAvailable { get; }
    
    public void SetAttackingTarget(ITarget target);
}
