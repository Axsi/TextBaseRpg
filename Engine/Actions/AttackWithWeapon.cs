using System;
using Engine.Models;

namespace Engine.Actions
{
    //WAIT I THOUGHT ONLY CAN IMPLEMENT ONE, WHY IS THERE BaseAction AND IAction???!!!!@@@!!!???
    //public class AttackWithWeapon : BaseAction, IAction -> this means AttackWithWeapon is inheriting from BaseAction, and AttackWithWeapon implements IAction
    //you can only ever inherit from one class, but you can implement from as many interfaces as you want
    public class AttackWithWeapon : BaseAction, IAction //this class implements IAction interface
    {
        //if in this example AttackWithWeapon object is of type AttackWithWeapon, it will have access to all properties and methods
        //setup within AttackWithWeapon. IF AttackWithWeapon object is of type IAction, this object will only have access to the properties and methods
        //listed as required in the IAction interface!!!@
        // private readonly GameItem _weapon;
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;

        //public event EventHandler<string> OnActionPerformed; //this is going to be the common way we send a message to the UI for it to be displayed

        //constructor
        public AttackWithWeapon(GameItem itemInUse, int minimumDamage, int maximumDamage) : base(itemInUse)//params taken from GameItem class? 
        //note that in the constructor of classes that make extend a parent class, these child classes need to call the parent class's constructor as well or in other words the "base"
        {
            if (itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a weapon");
            }

            if (_minimumDamage < 0)
            {
                throw new ArgumentException("minimumDamage must be 0 or larger");
            }

            if (_maximumDamage < _minimumDamage)
            {
                throw new ArgumentException("maximumDamage must be >= minimumDamage");
            }

            // _weapon = itemInUse;
            _minimumDamage = minimumDamage;
            _maximumDamage = maximumDamage;
        }

        //every action class is going to have an Execute method, this is what they call to do the action
        //here in the params we pass in "actor" who is doing the action, and "target" who is receiving the action
        public void Execute(LivingEntity actor, LivingEntity target)
        {
            int damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage);

            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "player" : $"the {target.Name.ToLower()}";

            if (damage == 0)
            {
                ReportResult($"{actorName} missed the {targetName}.");
            }
            else
            {
                ReportResult($"{actorName} hit the {targetName} for {damage} point{(damage > 1 ? "s" : "")}.");
                target.TakeDamage(damage);
            }
        }
    }
}