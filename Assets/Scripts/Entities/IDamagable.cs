using System;
using Enums;

public interface IDamagable : IDestroyable
{
    public DamagableType DamagableType { get; }
    public void TakeDamage(int count);
}
