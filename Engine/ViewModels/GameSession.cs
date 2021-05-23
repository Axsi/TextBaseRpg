using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.EventArgs;
using Engine.Models;
using Engine.Factories;

namespace Engine.ViewModels
{
    //think of GameSession class as managing our world
    public class GameSession : BaseNotification
    {
        //OnMessageRaised will hold is a reference to a function in the view that should be run whenever this message is raised
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        private Player _currentPlayer;
        private Location _currentLocation;
        private Monster _currentMonster;
        private Trader _currentTrader;
        
        public Location CurrentLocation
        {
            get
            {
                return _currentLocation;
            }

            set
            {
                _currentLocation = value;
                
                // OnPropertyChanged("CurrentLocation");
                OnPropertyChanged(); //initially we had CurrentLocation and all other similiar properties set as string
                                                            //but this makes it hard for the ide to rename things with the rename command as a string is not actually
                                                            //part of the property, even if it is the same text
                
                //when our location changes we set a new currentLocation.. the below property changes will update
                //the ui on if certain movement buttons should still be visible at new location
                OnPropertyChanged(nameof(HasLocationToNorth)); //we still need "nameof" for these HasLocations... because we are not calling them from their
                OnPropertyChanged(nameof(HasLocationToSouth)); //own property, we are calling them from the "CurrentLocation" property
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));

                CompleteQuestsAtLocation();
                
                //when a player moves to a location, the Current Location will change, and "GivePlayerQuestsAtLocation() function will run to give
                //location's available quest
                GivePlayerQuestsAtLocation();
                
                //when location changes we get a new monster at the new location
                GetMonsterAtLocation();

                CurrentTrader = CurrentLocation.TraderHere;
            }
        }
        public Player CurrentPlayer
        {
            get { return _currentPlayer;}
            set
            {
                if (_currentPlayer != null)
                {
                    //here with "-=" we mean unsubscribe from it
                    _currentPlayer.OnActionPerformed -= OnCurrentPlayerPerformedAction;
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                }

                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    //here with "=+" we are saying on this new _currentPlayer, there OnKilled we are going to subscribe to it
                    
                    //this is a common pattern you use when you have on object subscribing to an event on another object, and that object can change
                    //because otherwise .net garbage collection does not know the object is completely unused, we don't care about the old player object anymore
                    //since it doesn't know it may keep it around saved in memory... which can take up space..
                    _currentPlayer.OnActionPerformed += OnCurrentPlayerPerformedAction;
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                }
            }
            
        }
        public World CurrentWorld { get; } //as the player moves around in game, we will set the current location to where they currently are at

        //current monster property will hold monster at the location so player can battle it
        public Monster CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                if(_currentMonster != null)
                {
                    _currentMonster.OnActionPerformed -= OnCurrentMonsterPerformedAction;
                    _currentMonster.OnKilled -= OnCurrentMonsterKilled;
                }
                
                _currentMonster = value;

                if(_currentMonster != null)
                {
                    _currentMonster.OnActionPerformed += OnCurrentMonsterPerformedAction;
                    _currentMonster.OnKilled += OnCurrentMonsterKilled;
 
                    RaiseMessage("");
                    RaiseMessage($"You see a {CurrentMonster.Name} here!");
                }
                
                // OnPropertyChanged(nameof(CurrentMonster));
                OnPropertyChanged(); //same here we are calling from CurrentMonster property so we don't need nameof when we are using [CallerMemberName]
                OnPropertyChanged(nameof(HasMonster));
            }
        }

        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        public bool HasLocationToNorth => 
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;

        public bool HasLocationToEast => 
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;

        public bool HasLocationToSouth => 
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;

        public bool HasLocationToWest => 
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;

        //this HasMonster function is similiar to the above functions like "HasLocationToWest" but instead of using get, we are using
        //an expression body "=> ..." It is basically the same as saying "return ..." Seems to do the same thing as the get call, but cleaner
        public bool HasMonster => CurrentMonster != null;
        
        //we use HasTrader function here to help us hide or show the trader button
        public bool HasTrader => CurrentTrader != null;
        
        //as an example we are setting up a player object here in the currentPlayer property inside the GameSession class, but
        //we should be doing this in a player creation screen or loading it from disk instead
        
        //when we want to create an object of a class we will call the constructor of that class
        public GameSession()
        {
            CurrentPlayer = new Player("Alex", "Fighter", 0, 10, 10, 10000);

            //if current player doesnt have any weapon in their inventory just add a weapon for them, in this case game item 1001, which is a pointy stick
            if (!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }
            
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2001));

            // WorldFactory factory = new WorldFactory(); //create a world factory object here
            // CurrentWorld = factory.CreateWorld(); //call the createWorld function here
            
            //this line below is making use of the now changed to static class, WorldFactory class. As you can see
            //it is not instantiating a new WorldFactory object
            CurrentWorld = WorldFactory.CreateWorld();
            

            CurrentLocation = CurrentWorld.LocationAt(0, 0);
        }

        //public because the UI project needs to call these functions
        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);   
            }
        }
        
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }
        
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);  
            }
        }
        
        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);   
            }
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.Id == quest.Id && !q.IsCompleted);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        //remove the quest completion items from the player's inventory
                        foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                        {
                            for (int i = 0; i < itemQuantity.Quantity; i++)
                            {
                                CurrentPlayer.RemoveItemFromInventory(
                                    CurrentPlayer.Inventory.First(item => item.ItemTypeId == itemQuantity.ItemId));
                            }
                        }

                        RaiseMessage("");
                        RaiseMessage($"You completed the '{quest.Name}' quest");
                        
                        //Give the player the quest rewards
                        RaiseMessage($"You receive {quest.RewardExperiencePoints} experience points");
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);

                        RaiseMessage($"You receive {quest.RewardGold} gold");
                        CurrentPlayer.ReceiveGold(quest.RewardGold);

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemId);

                            RaiseMessage($"You receive a {rewardItem.Name}");
                            CurrentPlayer.AddItemToInventory(rewardItem);
                        }
                        
                        //Mark the quest as completed
                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }

        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.Id == quest.Id))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    
                    RaiseMessage("");
                    RaiseMessage($"You receive the '{quest.Name}' quest");
                    RaiseMessage(quest.Description);
 
                    RaiseMessage("Return with:");
                    foreach(ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemId).Name}");
                    }
 
                    RaiseMessage("And you will receive:");
                    RaiseMessage($"   {quest.RewardExperiencePoints} experience points");
                    RaiseMessage($"   {quest.RewardGold} gold");
                    foreach(ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemId).Name}");
                    }
                }
            }
        }

        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        public void AttackCurrentMonster()
        {
            //our guard clause to make sure the CurrentWeapon value exists for the rest of the function, if not send below message to the UI
            if (CurrentPlayer.CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon, to attack.");
                return;
            }
            
            CurrentPlayer.UseCurrentWeaponOn(CurrentMonster);
            
            if (!CurrentMonster.IsDead)
            {
                CurrentMonster.UseCurrentWeaponOn(CurrentPlayer);
            }
            else
            {
                //yes killing the monster and setting CurrentMonster property to null will run the setter code of this property
                //which unsubscribes to OnCurrentMonsterKilled, and since value is now null subscribing to OnCurrentMonsterKilled with new monster
                //will not occur
                CurrentMonster = null;
            }
        }
        
        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CurrentConsumable == null)
            {
                RaiseMessage("You must select a consumable to use.");
                return;
            }
            
            CurrentPlayer.UseCurrentConsumable();
        }
        
        private void OnCurrentPlayerPerformedAction(object sender, string result)
        {
            RaiseMessage(result);
        }
        
        //this function handles when a monster performs an action
        private void OnCurrentMonsterPerformedAction(object sender, string result)
        {
            RaiseMessage(result);
        }
        
        //this is the function to call when the player dies
        private void OnCurrentPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"The {CurrentMonster.Name} killed you.");

            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
        }
        
        //this is the function to call when the monster dies, this code was inside AttackCurrentMonster function
        private void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"You defeated the {CurrentMonster.Name}!");
 
            RaiseMessage($"You receive {CurrentMonster.RewardExperiencePoints} experience points.");
            CurrentPlayer.AddExperience(CurrentMonster.RewardExperiencePoints);
 
            RaiseMessage($"You receive {CurrentMonster.Gold} gold.");
            CurrentPlayer.ReceiveGold(CurrentMonster.Gold);
 
            foreach(GameItem gameItem in CurrentMonster.Inventory)
            {
                RaiseMessage($"You receive one {gameItem.Name}.");
                CurrentPlayer.AddItemToInventory(gameItem);
            }
        }

        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage($"You are now level {CurrentPlayer.Level}!");
        }
        
        private void RaiseMessage(string message)
        {
            //if anything is subscribed to the OnMessageRaised property, it will call Invoke and pass in itself and the message
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}