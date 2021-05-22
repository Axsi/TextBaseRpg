using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Engine.Actions;
using Engine.Models;


namespace Engine.Factories
{
    //static because we arent going to instantiate it, we are just going to use the functions in it.
    //a static class does not have a constructor because we never create an instance of a static class
    public static class ItemFactory
    {
        //readonly here makes it so _standardGameItems can only be set here or in a constructor. This kind of protects us from
        //accidently setting the value somewhere else
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        //first time anytime anybody uses anything in this ItemFactory class, the below "static ItemFactory()" function will runn
        static ItemFactory()
        {
            //player weapons
            BuildWeapon(1001, "Pointy Stick", 1, 1, 2);
            BuildWeapon(1002, "Rusty Sword", 5, 1, 3);
            
            //monster weapons, only used by monsters for when they attack
            BuildWeapon(1501, "Snake fangs", 0, 0, 2);
            BuildWeapon(1502, "Raw claws", 0, 0, 2);
            BuildWeapon(1503, "Spider fangs", 0, 0, 3);
 
            //Healing items
            BuildHealingItem(2001, "Granola bar", 5, 2);
            
            //Loot
            BuildMiscellaneousItem(9001, "Snake fang", 1);
            BuildMiscellaneousItem(9002, "Snakeskin", 2);
            BuildMiscellaneousItem(9003, "Rat tail", 1);
            BuildMiscellaneousItem(9004, "Rat fur", 2);
            BuildMiscellaneousItem(9005, "Spider fang", 1);
            BuildMiscellaneousItem(9006, "Spider silk", 2);
        }

        public static GameItem CreateGameItem(int itemTypeId)
        {
            //what this below line does is, it uses LINQ to find the ItemTypeId that matches with the itemTypeid we passed into the CreateGameItem function
            //FirstOrDefault: if we don't find an item that matches, we use the default which is just null
            return _standardGameItems.FirstOrDefault(item => item.ItemTypeId == itemTypeId)?.Clone();
        }
        
        private static void BuildMiscellaneousItem(int id, string name, int price)
        {
            _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, id, name, price));
        }
 
        private static void BuildWeapon(int id, string name, int price, 
            int minimumDamage, int maximumDamage)
        {
            //create a weapon GameItem object
            GameItem weapon = new GameItem(GameItem.ItemCategory.Weapon, id, name, price, true);

            //set the weapon GameItem object's Action property with a AttackWithWeapon object
            //this is where we are composing the GameItem object's behaviour through passing in command objects
            weapon.Action = new AttackWithWeapon(weapon, minimumDamage, maximumDamage);
            
            _standardGameItems.Add(weapon);
        }

        private static void BuildHealingItem(int id, string name, int price, int hitPointsToHeal)
        {
            GameItem item = new GameItem(GameItem.ItemCategory.Consumable, id, name, price);
            item.Action = new Heal(item, hitPointsToHeal);
            _standardGameItems.Add(item);
        }
    }
}