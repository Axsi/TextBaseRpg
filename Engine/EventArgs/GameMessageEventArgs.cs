namespace Engine.EventArgs
{
    //our custom EventArgs class
    public class GameMessageEventArgs : System.EventArgs
    {
        public string Message { get; set; }

        //constructor
        public GameMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}