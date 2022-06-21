using System;

namespace Container
{
    public class CellContent
    {
        public bool IsFilled => _item != null && _itemType != ItemType.Empty;

        private Item _item;
        public Item Item => _item;

        private ItemType _itemType;
        public ItemType ItemType => _itemType;

        private int _count;
        public int Count => _count;

        public CellContent(Item item = null, ItemType itemType = ItemType.Empty, int count = 0)
        {
            SetContent(item, itemType);
        }

        public void SetContent(Item item, ItemType itemType)
        {
            _item = item;
            _itemType = itemType;
        }

        public void SetContent(CellContent cellContent)
        {
            _item = cellContent.Item;
            _itemType = cellContent.ItemType;
        }

        public CellContent GetItems(int count)
        {
            if (Count <count)
            {
                throw new IndexOutOfRangeException();
            }
            _count -= count;
            return new CellContent(this._item, this._itemType, count);
        }
}
}