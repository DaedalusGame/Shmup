using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class Hitbox : Area2D
    {
        [Export]
        public DamageType DamageType { get; set; }
        [Export]
        public float MinTime = 0;
        [Export]
        public float MaxTime = 9999;
        [Export]
        public bool MultiHit = false;

        float time;
        public IHitter Hitter;

        [Signal]
        public delegate void OnHitEventHandler(Hurtbox other);

        public override void _Ready()
        {
            base._Ready();

            Monitorable = true;
            Monitoring = false;

            if(MinTime > 0)
            {
                Monitorable = false;
            }

            Hitter = this.FindParent<IHitter>();
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            time += (float)delta;
            Monitorable = time >= MinTime && time <= MaxTime;
        }

        public void Hit(Hurtbox hurtbox)
        {
            EmitSignal(SignalName.OnHit, hurtbox);
            var hurter = hurtbox.Hurter;
            var hitter = Hitter;
            if(hurter != null && hitter != null)
            {
                if(hurter.Faction == hitter.Faction)
                {
                    return;
                }

                Hitter.Hit(hurtbox.Hurter);
            }
        }
    }
}
