using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class Bullet : Node2D, IHitter, IFactionMember
    {
        public Node Shooter;
        public Vector2 Speed;
        public DeltaControllerNode TimeController = new DeltaControllerNode();

        [Export]
        public float Damage = 1;
        [Export]
        public float Duration = -1;
        [Export]
        public bool ImpactKill = false;
        [Export]
        public bool OutOfBoundsKill = true;
        [Export]
        public float OutOfBoundsGrow = 0;

        [Export]
        public float RotationSpeed = 0;
        [Export(PropertyHint.Range, "0,1,")]
        public float RotationOffset = 0;
        [Export]
        public bool AlignRotation = false;

        float time;

        public Faction Faction => (Shooter as IFactionMember)?.Faction ?? Faction.Neutral;

        public override void _Ready()
        {
            base._Ready();

            
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            if (AlignRotation && Speed != Vector2.Zero)
            {
                this.Rotation = Speed.Angle() + RotationOffset * Mathf.Tau;
            }
            else
            {
                this.Rotation += RotationSpeed * Mathf.Tau * (float)delta;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            var position = Position;
            var deltaReal = TimeController.Update(delta);

            position += Speed * deltaReal;

            if(OutOfBoundsKill && !(MainCamera.Instance?.IsInView(GlobalPosition, OutOfBoundsGrow) ?? false))
            {
                QueueFree();
            }

            time += deltaReal;
            if(Duration > 0 && time >= Duration)
            {
                QueueFree();
            }

            Position = position;
        }

        public virtual bool Hit(IHurter hurter)
        {
            bool result = false;

            if (Damage > 0)
            {
                result = hurter.TakeDamage(Damage);
            }

            if(ImpactKill)
            {
                QueueFree();
            }

            return result;
        }
    }
}
