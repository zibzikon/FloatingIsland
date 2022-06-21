using System.Collections.Generic;
using UnityEngine;

public interface IBuildingContainer
{ 
   void SetBuildPointsPositions();
   public IEnumerable<BuildPoint> GetCorrectBuildPoints(Building building);
}
