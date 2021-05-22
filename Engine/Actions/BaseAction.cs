using System;
using Engine.Models;

namespace Engine.Actions
{
    //because we are never going to instantiate a BaseAction class on its own, we are only going to instantiate its
    //children, we made this an public abstract class. The abstract means we can't instantiate this, just its children
    public abstract class BaseAction
    {
        //because _itemInUse is protected, the class's children can use it
        protected readonly GameItem _itemInUse;

        public event EventHandler<string> OnActionPerformed;

        //protected constructor, the children can call the constructor for the BaseAction, because it is a
        //protected constructor, the children can use it
        protected BaseAction(GameItem itemInUse)
        {
            _itemInUse = itemInUse;
        }

        //Looks to see if anyone ie subscribed to the OnActionPerformed, if so it raises the event and passes the message to the subscribed object
        //protected means it can be called by anything inside this class, or its child classes
        protected void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }

    }
}