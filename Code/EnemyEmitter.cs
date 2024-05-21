using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    public enum LevelEdge
    {
        None,
        Top,
        Left,
        Right,
        Bottom,
        Relative,
    }

    [GlobalClass]
    public partial class EnemyEmitter : Node
    {
        static Random random = new Random();

        [Export]
        public PackedScene EnemyPrefab;
        [Export]
        public float StartDelay;
        [Export]
        public float EmitDelay;
        [Export]
        public float MaxEmissions;
        [Export]
        public LevelEdge Edge;
        [Export]
        public Rect2 Area;

        float emitTime;
        int emissions;

        LevelSegment levelSegment;

        public override void _Ready()
        {
            base._Ready();

            levelSegment = this.FindParent<LevelSegment>();
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            if (MaxEmissions <= 0 || emissions < MaxEmissions)
            {
                emitTime += (float)delta;
                if (emitTime >= EmitDelay)
                {
                    Emit();
                    emitTime -= EmitDelay;
                    emissions++;
                }
            }

            if(levelSegment.IsValid() && !levelSegment.IsActive)
            {
                QueueFree();
            }
        }

        private Vector2 GetEmissionPoint()
        {
            var viewportTransform = MainCamera.Instance.GetViewportTransform().AffineInverse();
            var viewportRect = MainCamera.Instance.GetViewportRect();

            var center = viewportRect.GetCenter();

            switch (Edge)
            {
                case LevelEdge.Top:
                    return viewportTransform * random.NextVector2(new Rect2(center.X + Area.Position.X, viewportRect.Position.Y, Area.Size.X, 0));
                case LevelEdge.Left:
                    return viewportTransform * random.NextVector2(new Rect2(viewportRect.Position.X, Area.Position.Y, 0, Area.Size.Y));
                case LevelEdge.Right:
                    return viewportTransform * random.NextVector2(new Rect2(viewportRect.End.X, Area.Position.Y, 0, Area.Size.Y));
                case LevelEdge.Bottom:
                    return viewportTransform * random.NextVector2(new Rect2(center.X + Area.Position.X, viewportRect.End.Y, Area.Size.X, 0));
                default:
                    return random.NextVector2(Area);
            }
        }

        public void Emit()
        {
            var point = GetEmissionPoint();

            var enemy = EnemyPrefab.Instantiate<Node2D>();
            AddSibling(enemy);
            if (Edge == LevelEdge.Relative)
            {
                enemy.Position = point;
            }
            else
            {
                enemy.GlobalPosition = point;
            }
        }
    }
}
