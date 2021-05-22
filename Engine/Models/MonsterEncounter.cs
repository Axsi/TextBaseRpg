namespace Engine.Models
{
    public class MonsterEncounter
    {
        public int MonsterId { get; }
        public int ChanceOfEncountering { get; set; }// we leave the setter here because we can actually reset that in our factory method
        
        //constructor
        public MonsterEncounter(int monsterId, int chanceOfEncountering)
        {
            MonsterId = monsterId;
            ChanceOfEncountering = chanceOfEncountering;
        }
        
    }
}