using System.ComponentModel;

namespace Engine.Models
{
    public class QuestStatus : BaseNotification
    {
        private bool _isCompleted;
        public Quest PlayerQuest { get; }

        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
            set
            {
                _isCompleted = value;
                OnPropertyChanged(); //OnPropertyChanged with no value passed to it, because of the new [CallerMemberName] added in BaseNotification class
                                     //the OnPropertyChanged function will take the name of the property that called it, I guess? in this case "IsCompleted"
                                     
                                     //I guess this allows us to avoid doing something like "OnPropertyChanged(nameof(IsCompleted))" in order to adjust the UI
            }
        }

        //constructor
        public QuestStatus(Quest quest)
        {
            //setting the PlayerQuest should only happen here
            PlayerQuest = quest;
            IsCompleted = false; //we don't pass in an isCompleted param here, because when we start a new quest it is always going to be false
        }
    }
}