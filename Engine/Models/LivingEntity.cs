using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    //this is our base class for anything that is alive, a monster, a player, an npc
    //LivingEntity will be an abstract class, this means you can never instantiate a LivingEntity object
    //you can instantiate a child class that uses the LivingEntity class
    public abstract class LivingEntity : BaseNotification //Note child classes that use LivingEntity will still have access to BaseNotification class
    {
        private string _name;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _gold;
        private int _level;
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;

        public string Name
        {
            get { return _name; }
            private set //changed properties to private set because of encapsulation?
            {
                _name = value;
                //OnPropertyChanged(nameof(Name));
                OnPropertyChanged(); //changed to no parameters because of [CallerMemberName] in BaseNotification's OnPropertyChange function
            }
        }

        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged();
            }
        }

        public int MaximumHitPoints
        {
            get { return _maximumHitPoints; }
            protected set //this was changed from private to protected, so the player class can modified the max hitpoints when
            // a player gains a level
            {
                _maximumHitPoints = value;
                OnPropertyChanged();
            }
        }

        public int Gold
        {
            get { return _gold; }
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }

        public int Level
        {
            get { return _level; }
            protected set //we made this here protected instead of private, because it is protected that means
            //we can set this value inside the living entity class or from any of the child classes like player, monster, etc
            //since player is the only thing that will have experience change, we need to make it protected so player has access to it
            {
                _level = value;
                OnPropertyChanged();
            }
        }


        //!!!!!!!!@@@ honestly for this method I am a bit confused (lesson 12.2)
        public GameItem CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                //we subscribe and unsubscribe to and from the RaiseActionPerformedEvent, so this way when the player
                //changes their current weapon, then we are going to look at the current weapon's action and subscribe to 
                //its OnActionperformed event
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                //set current value passed into setter to be the _currentWeapon
                _currentWeapon = value;

                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
                
                OnPropertyChanged();
            }
        }

        public GameItem CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentConsumable = value;

                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }
                
                OnPropertyChanged();
            }
        }
        
        //Inventory property is for the one line per item (its weird to have Inventory and GroupedInventory.. will have to make it just one property..)
        public ObservableCollection<GameItem> Inventory { get; }
        
        //GroupedInventory property is for the grouped items on one line
        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; }

        public List<GameItem> Weapons => 
            Inventory.Where(i => i.Category == GameItem.ItemCategory.Weapon).ToList();

        public List<GameItem> Consumables =>
            Inventory.Where(i => i.Category == GameItem.ItemCategory.Consumable).ToList();

        public bool HasConsumable => Consumables.Any();

        public bool IsDead => CurrentHitPoints <= 0;

        public event EventHandler<string> OnActionPerformed;
        public event EventHandler OnKilled;
        
        //constructor
        //protected means it can only be seen by the child classes that make use of LivingEntity c lass
        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int gold, int level = 1)
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Gold = gold;
            Level = level;
            
            Inventory = new ObservableCollection<GameItem>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
        }

        public void UseCurrentWeaponOn(LivingEntity target)
        {
            //"this" is just the living entity that is doing the attacking
            CurrentWeapon.PerformAction(this, target);
        }

        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(CurrentConsumable);
        }

        public void TakeDamage(int hitPointsOfDamage)
        {
            CurrentHitPoints -= hitPointsOfDamage;

            if (IsDead)
            {
                CurrentHitPoints = 0;
                RaiseOnKilledEvent();
            }
        }

        public void Heal(int hitPointsToHeal)
        {
            CurrentHitPoints += hitPointsToHeal;

            if (CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
        }

        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }
        
        public void SpendGold(int amountOfGold)
        {
            if (amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException(
                    $"{Name} only has {Gold} gold, and cannot spend {amountOfGold} gold");
            }

            Gold -= amountOfGold;
        }
        
        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);

            if (item.IsUnique)
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 1)); //so I guess if the item is unique it will always add a new row of quantity 1
            }
            else
            {
                if (!GroupedInventory.Any(gi => gi.Item.ItemTypeId == item.ItemTypeId))
                {
                    GroupedInventory.Add(new GroupedInventoryItem(item, 0));
                }

                GroupedInventory.First(gi => gi.Item.ItemTypeId == item.ItemTypeId).Quantity++;
            }
            
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        public void RemoveItemFromInventory(GameItem item)
        {
            //Remember, you got two types of Inventory properties now.. Inventory and GroupedInventory
            Inventory.Remove(item); //Inventory should just be for the one line items, whereas GroupedInventory is for multiple items in one line

            //so i guess this is for if the item is in groupedInventory
            GroupedInventoryItem groupedInventoryItemToRemove = item.IsUnique //ah if its unique, return the item where gi.Item == item, if it is not unique, return the item whos itemTypeId matches
                ? GroupedInventory.FirstOrDefault(gi => gi.Item == item)
                : GroupedInventory.FirstOrDefault(gi => gi.Item.ItemTypeId == item.ItemTypeId);

            if (groupedInventoryItemToRemove != null)
            {
                if (groupedInventoryItemToRemove.Quantity == 1)
                {
                    GroupedInventory.Remove(groupedInventoryItemToRemove);
                }
                else
                {
                    groupedInventoryItemToRemove.Quantity--;
                }
            }
            
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        private void RaiseOnKilledEvent()
        {
            //looks at OnKilled event to see if there are any subscribers to it (in this case will be the GameSession object)
            //it will do whatever it has to do. With the case of monster killed we will get the exp etc, if player is killed
            //he will be sent home
            OnKilled?.Invoke(this, new System.EventArgs());
        }

        //how this command notification is going to work is, the ui is going to watch for ActionsPerformed on the LivingEntity/player object
        //the player object is going to look for actions performed by the weapon or by healing potion, etc
        //so when the weapon or healing item raises an action message, the player is going to catch it, and its going to use the OnActionPerformed property and the
        //associated RaiseActionPerformedEvent method to notify the UI, cause the UI is going to be subscribed to the LivingEntity/player's onActionPerformed
        private void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}