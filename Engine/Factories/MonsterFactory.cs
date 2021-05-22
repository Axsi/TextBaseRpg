using System;
using Engine.Models;

namespace Engine.Factories
{
    //in ItemFactory class we called the clone function to instantiate a new item object, we are going to do a different approach for the MonsterFactory
    //Just to see a different way to do things, in this case we use a switch statement.
    public static class MonsterFactory
    {
        public static Monster GetMonster(int monsterId)
        {
            switch (monsterId)
            {
                case 1:
                    Monster snake = new Monster("Snake", "Snake.png", 4, 4, 5, 1);

                    //we setup the monster's CurrentWeapon property, with their respective monster weapon
                    snake.CurrentWeapon = ItemFactory.CreateGameItem(1501);
                    
                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);

                    return snake;
                    // break; //maybe i just need return and dont need break in c#?
                
                case 2:
                    Monster rat = new Monster("Rat", "Rat.png", 5, 5,  5, 1);

                    rat.CurrentWeapon = ItemFactory.CreateGameItem(1502);
                    
                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 75);
 
                    return rat;

                case 3:
                    Monster giantSpider = 
                        new Monster("Giant Spider", "GiantSpider.png", 10, 10,10, 3);

                    giantSpider.CurrentWeapon = ItemFactory.CreateGameItem(1503);
                    
                    AddLootItem(giantSpider, 9005, 25);
                    AddLootItem(giantSpider, 9006, 75);
 
                    return giantSpider;
 
                default:
                    //throwing an error
                    throw new ArgumentException(string.Format("MonsterType '{0}' does not exist", monsterId));
            }
        }

        private static void AddLootItem(Monster monster, int itemId, int percentage)
        {
            if (RandomNumberGenerator.NumberBetween(1, 100) <= percentage)
            {
                // monster.Inventory.Add(new ItemQuantity(itemId, 1));
                monster.AddItemToInventory(ItemFactory.CreateGameItem(itemId));
            }
        }
    }
}