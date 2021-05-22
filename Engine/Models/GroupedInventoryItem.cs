namespace Engine.Models
{
    //we need to use the BaseNotification class here because we need to tell the UI when an item quantity has decreased or increased
    public class GroupedInventoryItem : BaseNotification
    {
        private GameItem _item;
        private int _quantity;

        public GameItem Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        //constructor
        public GroupedInventoryItem(GameItem item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}