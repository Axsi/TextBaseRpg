using System.Collections.Generic;
using System.Linq;
using Engine.Models;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();

        //this function will be run the first time someone uses anything inside the QuestFactory class
        static QuestFactory()
        {
            //Declare the items needed to complete the quest, and its reward items
            List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> rewardItems = new List<ItemQuantity>();
            
            itemsToComplete.Add(new ItemQuantity(9001, 5));
            rewardItems.Add(new ItemQuantity(1002, 1));
            
            //Create the quest
            _quests.Add(new Quest(
                1, 
                "Clear the herb garden", 
                "Defeat the snakes in the Herbalist's garden",
                itemsToComplete,
                25,
                10,
                rewardItems ));
        }

        //we can retrieve quest by using this GetQuestById function
        internal static Quest GetQuestById(int id)
        {
            return _quests.FirstOrDefault(quest => quest.Id == id);
        }
    }
}