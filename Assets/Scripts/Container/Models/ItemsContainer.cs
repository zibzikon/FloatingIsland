using System;

namespace Container
{
    public class ItemsContainer
    {
        public event Action ContainerChanged;
        
        private Cell[] _cells;
        public Cell[] Cells => _cells;

        public int Size => _cells.Length;
        

        public Cell SelectCell(int index)
        {
            if (index >= Size)
            {
                throw new IndexOutOfRangeException();
            }
            return Cells[index];
        }

        public void SetCellContent(Cell cell, CellContent cellContent)
        {
            cell.CellContent.SetContent(cellContent.Item, cellContent.ItemType);
            ContainerChanged?.Invoke();
        }
        
        public void ResetCellContent(Cell cell)
        {
            cell.CellContent.SetContent(null, ItemType.Empty);
            ContainerChanged?.Invoke();
        }
        
        public void Initialize(int size)
        {
            Generate(size);
        }

        private void Generate(int size)
        {
        }
    }
}