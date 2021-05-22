using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        //these private variables will store the values of the object property. They are the "backing variables"
        private int _experiencePoints;
        private string _characterClass;

        public string CharacterClass
        {
            get { return _characterClass; }
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }

        public int ExperiencePoints
        {
            get { return _experiencePoints; } //so whenever we get ExperiencePoints we are going to be grabbing it from the _experiencePoints variable
            private set //made this private to encapsulate
            {
                _experiencePoints = value;
                OnPropertyChanged(); //"ExperiencePoints is just the name of the object property in the player class
                SetLevelAndMaximumHitPoints();
            }
        }

        //using an ObservableCollection automatically handles all notifications, so we don't need to use OnPropertyChanged like we did in the Gold property
        //anytime we add or remove something to the inventory. It is automatically going to update in the UI
        //public ObservableCollection<GameItem> Inventory { get; set; }

        //we decided not to use getter and setter here. The above observableCollection<GameItem> may raise a notification when Inventory changes
        //however this List<GameItem> Weapons does not, so we have to setup a propertyChanged event to notify the UI when the weapons list changes
        // public List<GameItem> Weapons => 
        //     Inventory.Where(i => i is Weapon).ToList();
        
        //when entering a new location we will check if there is any available quest and if the player has completed the available
        //quest at this location yet, if not it will be added to this Quest collection. We will do all that within GameSession.cs class
        public ObservableCollection<QuestStatus> Quests { get; }

        public event EventHandler OnLeveledUp;

        //constructor
        public Player(string name, string characterClass, int experiencePoints, int maximumHitPoints, int currentHitPoints, int gold) :
            base(name, maximumHitPoints, currentHitPoints, gold)
        {
            CharacterClass = characterClass;
            ExperiencePoints = experiencePoints;
            Quests = new ObservableCollection<QuestStatus>();
        }

        public bool HasAllTheseItems(List<ItemQuantity> items) //for quest?
        {
            foreach (ItemQuantity item in items)
            {
                if (Inventory.Count(i => i.ItemTypeId == item.ItemId) < item.Quantity)
                {
                    return false;
                }
            }

            return true;
        }

        public void AddExperience(int experiencePoints)
        {
            ExperiencePoints += experiencePoints;
        }

        private void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = Level;
            Level = (ExperiencePoints / 100) + 1;

            if (Level != originalLevel)
            {
                MaximumHitPoints = Level * 10;
                
                OnLeveledUp?.Invoke(this, System.EventArgs.Empty); //I think OnLeveledUp event is a event property that subscribes to the OnCurrentPlayerLeveledUp function as shown in GameSession.cs
                                                                          //_currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp
            }
        }
    }
}