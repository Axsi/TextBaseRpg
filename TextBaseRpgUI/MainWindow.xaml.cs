using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Engine.EventArgs;
using Engine.ViewModels;

namespace TextBaseRpgUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly GameSession _gameSession = new GameSession(); //We use a private variable here, because no other class is going to use this class,
                                          //so we don't set it as a property in this class to expose it
                                          
        //constructor I believe
        public MainWindow()
        {
            InitializeComponent(); //this sets up everything for the screen

            // _gameSession = new GameSession();

            _gameSession.OnMessageRaised += OnGameMessageRaised;
            
            DataContext = _gameSession; //this is what our xaml file is going to use for its values. DataContext is a built in property for the xaml window
        }

        //OnCLickMove functions need to change our current location (adjust the state of the game)
        private void OnClickMoveNorth(object sender, RoutedEventArgs e) //click event always sends these two parameters, we don't need to use them but we have to accept them
        {
            _gameSession.MoveNorth();
        }
        
        private void OnClickMoveWest(object sender, RoutedEventArgs e) 
        {
            _gameSession.MoveWest();
        }
        
        private void OnClickMoveSouth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveSouth();
        }
        
        private void OnClickMoveEast(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveEast();
        }

        //the function that gets called when clicking the attack monster button
        private void OnClick_AttackMonster(object sender, RoutedEventArgs e)
        {
            _gameSession.AttackCurrentMonster();
        }

        private void OnClick_UseCurrentConsumable(object sender, RoutedEventArgs e)
        {
            _gameSession.UseCurrentConsumable();
        }

        private void OnClick_AttemptEscape(object sender, RoutedEventArgs e)
        {
            _gameSession.EscapeCurrentMonsterEncounter();
        }
        
        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            //I believe GameMessages is the name setup for the RichTextBox in the GameMessage section of the MainWindow.xaml
            GameMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            GameMessages.ScrollToEnd();
        }

        private void OnClick_DisplayTradeScreen(object sender, RoutedEventArgs e)
        {
            TradeScreen tradeScreen = new TradeScreen();
            tradeScreen.Owner = this;
            tradeScreen.DataContext = _gameSession;
            tradeScreen.ShowDialog();
        }
    }
}