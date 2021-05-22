using Engine.ViewModels;
using NUnit.Framework;

namespace TestEngine.ViewModels
{
    [TestFixture]
    public class TestGameSession
    {
        [Test]
        public void TestCreateGameSession()
        {
            //Setup
            GameSession gameSession = new GameSession();
            
            //Assertions
            Assert.IsNotNull(gameSession.CurrentPlayer);
            Assert.AreEqual("Town square", gameSession.CurrentLocation.Name);
        }

        [Test]
        public void TestPlayerMovesHomeAndIsCompletelyHealedOnKilled()
        {
            //Setup
            GameSession gameSession = new GameSession();
            gameSession.MoveEast();
            gameSession.MoveEast(); //move right twice to get to spider forest aka a place with monsters
            gameSession.CurrentPlayer.TakeDamage(999);

            //Assertions
            Assert.AreEqual("Home", gameSession.CurrentLocation.Name);
            //this checks if the player is fully healed by checking current level's max hitpoints
            Assert.AreEqual(gameSession.CurrentPlayer.Level * 10, gameSession.CurrentPlayer.CurrentHitPoints);
        }
    }
}