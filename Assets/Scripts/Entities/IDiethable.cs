using System;
using UnityEngine.Events;

public interface IDiethable
{
    public event Action<object> Died;

    public void Die();

}
