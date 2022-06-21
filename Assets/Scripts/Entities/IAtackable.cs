
public interface IAtackable
{
    public bool AtackingStarted { get; }
    public void Atack(ITarget target);
}
