using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Actions;

namespace Engine.Models
{
    public class GameItem
    {
        public enum ItemCategory //enum is kind of like a constant but we have defined options as to what value it can be
        {
            Miscellaneous,
            Weapon,
            Consumable
        }
        
        //properties of the GameItem object, we got rid of the sets because it should only be set in the constructor
        public ItemCategory Category { get; }
        public int ItemTypeId { get; }
        public string Name { get; }
        public int Price { get; }
        public bool IsUnique { get; }

        public IAction Action { get; set; } //IAction type here, so now we can hold any type of IAction in here

        //constructor for GameItem object
        //every Weapon will be unique but the throwaway items will not be unique
        public GameItem(ItemCategory category, int itemTypeId, string name, int price, bool isUnique = false, IAction action = null)
                                                                                      // setting isUnique to default false here just means if we pass the 
        {                                                                             //GameItem constructor only the first three params and not isUnique, it
            ItemTypeId = itemTypeId;                                                  //defaults to false, we can only default params that are at the end of the
            Name = name;                                                              //param list
            Price = price;
            IsUnique = isUnique;
            Category = category;
            Action = action;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            //Action object
            Action?.Execute(actor, target); //this is a null conditional operator
        }

        //function to create a new object that has the exact same property values as a different object
        public GameItem Clone()
        {
            //so basically if we have a GameItem object, and we call the Clone function we just pass this object's existing properties to be used in
            //the GameItem constructor
            return new GameItem(Category, ItemTypeId, Name, Price, IsUnique, Action);
        }
    }
}