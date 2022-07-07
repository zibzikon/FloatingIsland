
public interface IAtackable
{
    public bool AttackingStarted { get; }
    public void Atack(ITarget target);
}
