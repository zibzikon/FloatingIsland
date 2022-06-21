using Enums;
using UnityEngine;

public interface ITargetContainer
{
    public ITarget GetClosestTargetOnLayer(Vector3 startPosition, TargetType preferredTargetType);
}
