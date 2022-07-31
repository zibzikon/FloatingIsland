
using UnityEngine;

public interface ISelectable
{
    public Transform Transform { get; }
    public void Select();
    public void Deselect();
}
