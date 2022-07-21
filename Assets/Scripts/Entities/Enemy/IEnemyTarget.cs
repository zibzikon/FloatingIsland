using Enums;

public interface IEnemyTarget : ITarget
{
    public EnemyType EnemyType { get; }
}
