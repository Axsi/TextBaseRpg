using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Factories;

namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get; }
        public int YCoordinate { get; }
        public string Name { get; }
        public string Description { get; }
        public string ImageName { get; } //property string that holds the file location of the image
        
        public List<Quest> QuestsAvailableHere { get; } = new List<Quest>(); //initializing this property with an empty list to start with

        //we could add MonsterEncounter objects directly to this list property but we want it to be a bit smarter so the AddMonster function was created
        public List<MonsterEncounter> MonstersHere { get; } = new List<MonsterEncounter>();
        
        public Trader TraderHere { get; set; } //we keep set here, because we instantiate the location then add a trader
        
        //constructor
        public Location(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Name = name;
            Description = description;
            ImageName = imageName;
        }
        
        //This function adds a possible monster at a location
        public void AddMonster(int monsterId, int chanceOfEncountering)
        {
            if (MonstersHere.Exists(m => m.MonsterId == monsterId))
            {
                //This monster has already been added to this location
                //so, overwrite the ChanceOfEncountering with the new number
                MonstersHere.First(m => m.MonsterId == monsterId).ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                //This monster is not already at this location, so add it.
                MonstersHere.Add(new MonsterEncounter(monsterId, chanceOfEncountering));
            }
        }

        
        // If there are not any objects in the MonstersHere list, the function returns null.
        // Otherwise, it adds up all the ChancesOfEncountering, for the MonsterEncounter objects. It gets
        // a random number between 1 and the total ChancesOfEncountering. Then, it loops through the MonsterEncounter
        // objects until the accumulated runningTotal is less then or equal to the current loop's monster's ChanceOfEncountering
        //When it determines the random monster, it instantiates a new Monster object, through the MonsterFactory, and returns that object.
        public Monster GetMonster()
        {
            //checks if there are any monsters at this location
            if (!MonstersHere.Any())
            {
                return null;
            }
            
            //Total the percentage of all monsters at this location
            int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);

            int randomNumber = RandomNumberGenerator.NumberBetween(1, totalChances);

            
            // Loop through the monster list, 
            // adding the monster's percentage chance of appearing to the runningTotal variable.
            // When the random number is lower than the runningTotal,
            // that is the monster to return. (I don't believe this approach would return more then once monster)
            
            //I THINK, this is for determining which monster gets chosen, not so much the chance of encountering a monster
            int runningTotal = 0;

            foreach (MonsterEncounter monsterEncounter in MonstersHere)
            {
                runningTotal += monsterEncounter.ChanceOfEncountering;

                if (randomNumber <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MonsterId);
                }
            }
            
            //if there was a problem return the last monster in the list
            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterId);
        }
    }
}