
public interface IAtackable
{
    public bool AttackingStarted { get; }
    
    public bool AttackingIsAvailable { get; }
    
    public void SetAttackingTarget(ITarget target);
}
