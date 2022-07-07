using System.Collections.Generic;
using UnityEngine;

public interface IBuildingContainer
{
   public Vector3 WorldPosition { get; }
      
   public void SetBuildPointsPositions();
   
   public void AddChildBuilding(Building building, Direction3 direction);
}
