using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class BulletCircle : Node2D, IFactionMember
    {
        class Ball
        {
            public Node2D Node;

            internal bool IsDeleted()
            {
                return !GodotObject.IsInstanceValid(Node) || Node.IsQueuedForDeletion();
            }
        }

        [Export]
        public PackedScene BallPrefab;
        [Export]
        public int Orbiters;
        [Export]
        public float Radius;
        [Export]
        public float RotationSpeed;
        [Export]
        public bool RotationAlign;
        [Export]
        public float RotationOffset;

        public Faction Faction { get; set; } = Faction.Enemy;

        List<Ball> balls = new List<Ball>();
        float rotation;

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            while(balls.Count < Orbiters)
            {
                AddBall(MakeBall());
            }

            foreach(var ball in balls)
            {
                if(ball.IsDeleted())
                {
                    ball.Node = MakeBall();
                }
            }

            rotation += RotationSpeed * (float)delta;

            for (int i = 0; i < balls.Count; i++)
            {
                var ball = balls[i];

                if(ball.IsDeleted())
                {
                    continue;
                }

                float angle = (float)i / balls.Count * Mathf.Tau;
                angle += rotation;

                ball.Node.Position = Vector2.FromAngle(angle) * Radius;

                if(RotationAlign)
                {
                    ball.Node.Rotation = angle + RotationOffset;
                }
            }
        }

        private Node2D MakeBall()
        {
            Node node = BallPrefab.Instantiate();
            if (node is Node2D node2D)
            {
                node2D.GlobalPosition = GlobalPosition;
                AddChild(node2D);
            }
            if (node is Bullet bullet)
            {
                bullet.Shooter = this;
            }

            return node as Node2D;
        }

        public void AddBall(Node2D ball)
        {
            //ball.TopLevel = true;
            balls.Insert(0, new Ball() { Node = ball });
           
        }
    }
}
