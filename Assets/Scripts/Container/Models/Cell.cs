namespace Container
{
    public class Cell
    {
        public Cell(CellContent cellContent)
        {
            CellContent = cellContent;
        }
        public CellContent CellContent { get; set; }
    }
}