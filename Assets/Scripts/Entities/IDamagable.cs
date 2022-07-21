using System;

public interface IDamagable : IDestroyable
{
    public void Damage(int count);
}
