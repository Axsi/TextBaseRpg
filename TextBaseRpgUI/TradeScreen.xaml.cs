using System.Text.RegularExpressions;
using System.Windows;
using Engine.Models;
using Engine.ViewModels;

namespace TextBaseRpgUI
{
    //Interaction logic for TradeScreen.xaml
    public partial class TradeScreen : Window
    {
        //property here we where we are casting DataContext as a GameSession object, so the object we are working with
        //in the TradeScreen will be a GameSession
        public GameSession Session => DataContext as GameSession;
        
        public TradeScreen()
        {
            InitializeComponent();
        }
        
        //!!!!!!!!!@@!HONESTLY I'M NOT SURE WHY FOR OnClick_Sell and OnClick_Buy we changed the property type from GameItem to GroupedInventoryItem??????@@!!!!!!!!!!
        //Well GroupedInventoryItem class actually has a property of quantity and a property of GameItem, this way instead of just using GameItem by itself, with
        //GroupedInventoryItem, we have the properties of a GameItem class and also the quantity property as well
        private void OnClick_Sell(object sender, RoutedEventArgs e)
        {
            //here the ((FrameworkElement) sender).DataContext, allows us to figure out what row, the button clicked
            //was on. Each row in the player or trader menu should be a game item.
            //I believe the "sender" param gives us an idea of which button was clicked
            GroupedInventoryItem groupedInventoryItem = ((FrameworkElement) sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
                Session.CurrentPlayer.ReceiveGold(groupedInventoryItem.Item.Price);
                Session.CurrentTrader.AddItemToInventory(groupedInventoryItem.Item);
                Session.CurrentPlayer.RemoveItemFromInventory(groupedInventoryItem.Item);
            }
        }

        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem = ((FrameworkElement) sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
                if (Session.CurrentPlayer.Gold >= groupedInventoryItem.Item.Price)
                {
                    Session.CurrentPlayer.SpendGold(groupedInventoryItem.Item.Price);
                    Session.CurrentTrader.RemoveItemFromInventory(groupedInventoryItem.Item);
                    Session.CurrentPlayer.AddItemToInventory(groupedInventoryItem.Item);
                }
                else
                {
                    MessageBox.Show("You do not have enough gold");
                }
            }
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}