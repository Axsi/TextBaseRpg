// using System;
// using System.Collections.Generic;
// using System.ComponentModel;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
//
// namespace Engine.Models
// {
//     //Weapon class inherits GameItem class as weapon class will make use of some properties of the GameItem class
//     public class Weapon : GameItem
//     {
//         public int MinimumDamage { get; }
//         public int MaximumDamage { get; }
//         
//         //constructor for weapon class. what the below constructor is doing is, when the Weapon object is created it gets passed
//         //an itemTypeId, a name, and a price. The ":" part is saying take the three param that Weapon class object is given and pass those 
//         //to the base class, in this case GameItem
//         public Weapon(int itemTypeId, string name, int price, int minDamage, int maxDamage) : base(itemTypeId, name, price, true)
//         {
//             //as you can see the inheritted constructor params will get passed the needed values through ":base(.....)" 
//             //whereas the constructor params that exist within this class are setup as usual
//             MinimumDamage = minDamage;
//             MaximumDamage = maxDamage;
//         }
//
//         //weapon class is a subclass of gameItem which already has a clone function
//         //so for the weapon subclass, we have to put "new" in the function. This is basically saying, when we call Clone in weapon class
//         //we want to override the existing Clone function in the parent class GameItem
//         public new Weapon Clone()
//         {
//             return new Weapon(ItemTypeId, Name, Price, MinimumDamage, MaximumDamage);
//         }
//     }
// }