using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Engine
{
    //this class was created because our GameSession.cs and Player.cs file were both using setting up the OnPropertyChange
    //so instead we created this base class to have it be inherited by other classes. This base class wil implement the INotifyPropertyChanged interface
    public class BaseNotification : INotifyPropertyChanged
    {
        //below PropertyChangedEventHandler and OnPropertyChanged function are what is needed to implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        //[CallerMemberName] will look to see what called this OnPropertyChanged function
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        { //if anybody is listening to the propertyChanged handler, we see them the below message
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}