using System;
using Enums;

public interface IDamagable : IDestroyable
{
    public int Health { get; }
    public DamagableType DamagableType { get; }
    public void TakeDamage(int count);
}
