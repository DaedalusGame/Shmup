using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass, Tool]
    public partial class BulletArea : Node2D, IFactionMember
    {
        static Random random = new Random();

        [Export]
        public PackedScene BulletPrefab;
        [Export]
        public Rect2 Area;
        [Export]
        public float ShootDelay;
        [Export]
        public int BulletCount;
        [Export]
        public int MaxEmissions;

        int emissions;
        float shootTime;

        public Faction Faction { get; set; } = Faction.Enemy;

        public override void _PhysicsProcess(double delta)
        {
            if (Engine.IsEditorHint())
                return;

            base._PhysicsProcess(delta);

            if (!MainCamera.Instance.IsInView(GlobalPosition))
            {
                return;
            }

            if (MaxEmissions <= 0 || emissions < MaxEmissions)
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
                shootTime = 0;
            }
        }

        public override void _Process(double delta)
        {
            QueueRedraw();

            base._Process(delta);
        }

        public void Emit()
        {
            emissions++;

            for (int i = 0; i < BulletCount; i++)
            {
                Vector2 pos = new Vector2(random.NextFloat(Area.Position.X, Area.End.X), random.NextFloat(Area.Position.Y, Area.End.Y));
                
                var bullet = this.ShootBullet(BulletPrefab, pos, new Vector2(random.NextFloat(-50,50), random.NextFloat(0, 150)), this);
                bullet.Duration = random.NextFloat(2f, 8f);
            }
        }

        public override void _Draw()
        {
            if (!MainCamera.Instance?.IsInView(GlobalPosition) ?? false)
            {
                return;
            }
            if (Engine.IsEditorHint())
            {
                DrawRect(Area, Colors.Blue, false);
                DrawLine(new Vector2(-8, -8), new Vector2(+8, +8), Colors.Blue);
                DrawLine(new Vector2(+8, -8), new Vector2(-8, +8), Colors.Blue);
            }
        }
    }
}
