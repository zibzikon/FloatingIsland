    using System.Collections.Generic;
    using UnityEngine;

    public interface IBuildingsContainer
    {
        public bool CellsIsFreeToSet(Building building, IEnumerable<OccupyingCell> occupyingCells);
    }