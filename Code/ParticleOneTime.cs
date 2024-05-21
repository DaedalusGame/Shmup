using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    public partial class ParticleOneTime : CpuParticles2D
    {
        [Export]
        public bool Detach; 
        float lifetime;
        float duration;

        public override void _Ready()
        {
            base._Ready();

            Emitting = true;
            duration = (float)((Lifetime * 2) / SpeedScale);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            if (Detach)
            {
                Detach = false;
                this.Reparent(Game.Instance);
            }

            lifetime += (float)delta;
            if(!Emitting && lifetime > duration)
            {
                QueueFree();
            }
        }
    }
}
