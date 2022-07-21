using System;
using UnityEngine.Events;

public interface IDestroyable
{
    public event Action<object> Destroyed;

    public bool IsDestroyed {get;}

    public void Destroy();

}
