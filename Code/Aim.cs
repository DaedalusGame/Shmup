using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class Aim : Node2D
    {
        [Export(PropertyHint.Range, "0,1,")]
        public float AimEase;
        [Export(PropertyHint.Range, "-180,180,")]
        public float AimOffset;

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            Node2D parent = GetParent<Node2D>();

            if (Game.Instance.Player.IsValid())
            {
                var deltaVec = Game.Instance.Player.GlobalPosition - GlobalPosition;

                parent.GlobalRotation = Mathf.DegToRad(AimOffset) + Mathf.LerpAngle(parent.GlobalRotation, deltaVec.Angle(), AimEase);
            }
        }
    }
}
