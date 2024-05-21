using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class Weapon : Node
    {
        public IWeaponHaver WeaponHaver;
        public Player Player
        {
            get
            {
                return WeaponHaver as Player;
            }
        }
        public Node2D Body
        {
            get
            {
                return WeaponHaver as Node2D;
            }
        }

        public override void _Ready()
        {
            base._Ready();

            WeaponHaver = this.FindParent<IWeaponHaver>();
            WeaponHaver.AddWeapon(this);
        }
    }
}
