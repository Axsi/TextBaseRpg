using System;
using Engine.Models;

namespace Engine.Actions
{
    //our action interface that will help us setup different types of actions
    public interface IAction
    {
        //for all our actions these are the two things we care about and will need to have: OnActionPerformed, and Execute
        //we can have other things too but we just HAVE to have these two (in our case)
        
        //besides being a contract, an interface can also be a data type, basically if you use it as a data type, whatever you pass into
        //whatever is using interface as a datatype has to have implemented the required methods or attributes in the interface
        event EventHandler<string> OnActionPerformed;
        void Execute(LivingEntity actor, LivingEntity target);
    }
}