using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class DamageMask : Resource
    {
        [Export]
        public Godot.Collections.Array<DamageType> ResistTypes;
        [Export]
        public bool ResistIsWeakness;

        public bool IsWeak(DamageType damageType)
        {
            return ResistTypes.Contains(damageType);
        }
    }
}
