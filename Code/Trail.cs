using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class Trail : Node2D, IFactionMember
    {
        class PathNode
        {
            public Vector2 Position;
            public Vector2 Velocity;
        }

        class Ball
        {
            public Node2D Node;
            public int Offset;

            internal bool IsDeleted()
            {
                return !GodotObject.IsInstanceValid(Node) || Node.IsQueuedForDeletion();
            }
        }

        [Export]
        public PackedScene OptionBallPrefab;
        [Export]
        public float PathDelay = 0.1f;
        [Export]
        public int PathOffset = 5;
        [Export]
        public float SpawnDelay;
        [Export]
        public int MinBalls = 4;
        [Export]
        public int MaxBalls = 4;

        List<Ball> balls = new List<Ball>();
        List<PathNode> path = new List<PathNode>();

        float pathTime;
        float spawnTime;

        public Faction Faction { get; set; } = Faction.Enemy;

        public override void _Ready()
        {
            base._Ready();

            var factionMember = GetParent().FindParent<IFactionMember>();

            if (factionMember != null)
            {
                Faction = factionMember.Faction;
            }
        }

        private void AddPosition(Vector2 pos)
        {
            path.Insert(0, new PathNode() { Position = pos });
            if (path.Count > PathOffset * balls.Count)
            {
                path.RemoveAt(path.Count - 1);
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (balls.Count < MaxBalls)
            {
                spawnTime += (float)delta;

                if (spawnTime >= SpawnDelay)
                {
                    spawnTime -= SpawnDelay;
                    MakeBall();
                }
            }
            else
            {
                spawnTime = 0;
            }

            pathTime += (float)delta;
            if (pathTime >= PathDelay)
            {
                pathTime -= PathDelay;
                AddPosition(GlobalPosition);
            }

            balls.RemoveAll(x => x.IsDeleted());

            if (path.Count > 0)
            {
                int offset = 0;
                foreach (var ball in balls)
                {
                    offset += PathOffset;
                    ball.Offset += Math.Sign(offset - ball.Offset);
                    ball.Node.Position = path[Math.Min(ball.Offset, path.Count - 1)].Position;
                }
            }
        }

        public void AddBall(Node2D ball)
        {
            ball.TopLevel = true;
            balls.Insert(0, new Ball() { Node = ball, Offset = 0 });
            AddChild(ball);
        }

        private void MakeBall()
        {
            Node node = OptionBallPrefab.Instantiate();
            if (node is Node2D node2D)
            {
                node2D.GlobalPosition = GlobalPosition;
                AddBall(node2D);
            }
            if (node is Bullet bullet)
            {
                bullet.Shooter = this;
            }
        }
    }
}
