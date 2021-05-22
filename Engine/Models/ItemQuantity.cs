namespace Engine.Models
{
    //this class helps us manage the items and their quantities
    public class ItemQuantity
    {
        public int ItemId { get; }
        public int Quantity { get; }
        
        //constructor
        public ItemQuantity(int itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}