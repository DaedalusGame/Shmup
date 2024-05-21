using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class BulletEmitter : Node2D, IFactionMember
    {
        [Export]
        public PackedScene BulletPrefab;
        [Export]
        public float ShootDelay;
        [Export]
        public int Subdivisions;
        [Export]
        public float AngleCenter;
        [Export]
        public float AngleOffset;
        [Export]
        public float AngleRotation;
        [Export]
        public float Speed;
        [Export]
        public int MaxEmissions;
        [Export]
        public bool OneShot = false;

        public int Emissions;
        float shootTime;
        float angleRotationCurrent;

        public Faction Faction { get; set; } = Faction.Enemy;

        public override void _Ready()
        {
            base._Ready();

            var factionMember = GetParent().FindParent<IFactionMember>();

            if(factionMember != null)
            {
                Faction = factionMember.Faction;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            if (!MainCamera.Instance.IsInView(GlobalPosition))
            {
                return;
            }

            if (MaxEmissions <= 0 || Emissions < MaxEmissions)
            {
                shootTime += (float)delta;

                if (shootTime >= ShootDelay)
                {
                    shootTime -= ShootDelay;
                    Emit();
                }
            }
            else
            {
                if(OneShot)
                {
                    QueueFree();
                }
                shootTime = 0;
            }
        }

        public void Emit()
        {
            Emissions++;

            for (int i = 0; i < Subdivisions; i++)
            {
                float divSlide = (float)i / (Subdivisions - 1);
                float angle = AngleCenter + AngleOffset * i + (-AngleOffset * Subdivisions) / 2;

                this.ShootBullet(BulletPrefab, GlobalPosition, Vector2.FromAngle(Mathf.DegToRad(angle) + Rotation + angleRotationCurrent) * Speed);
            }

            angleRotationCurrent += AngleRotation * (Mathf.Tau / Subdivisions);
        }

        public void Reset()
        {
            Emissions = 0;
            shootTime = 0;
        }

        public bool IsDone => Emissions >= MaxEmissions;

    }
}
