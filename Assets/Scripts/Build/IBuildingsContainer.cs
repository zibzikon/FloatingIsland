    using System.Collections.Generic;
    using UnityEngine;

    public interface IBuildingsContainer
    {
        public bool CellsIsFreeToSet(Building building, List<OccupyingCell> occupyingCells);
    }