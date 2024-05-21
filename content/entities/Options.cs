using Godot;
using Shmup.Code;
using System;
using System.Collections.Generic;

public partial class Options : Weapon
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
    public int MaxBalls = 4;

    public Vector2 Velocity;

    float pathTime;
    float spawnTime;

    List<Ball> balls = new List<Ball>();
    List<PathNode> path = new List<PathNode>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void AddPosition(Vector2 pos)
    {
        path.Insert(0,new PathNode() { Position = pos });
        if(path.Count > PathOffset * balls.Count)
        {
            path.RemoveAt(path.Count-1);
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
            AddPosition(Player.GlobalPosition);

            if(Player.IdleTime > 0)
            {
                Velocity += new Vector2(0, 5);

                if(Velocity.Y > 100)
                {
                    Velocity.Y = 100;
                }

                for (int i = 0; i < path.Count; i++)
                {
                    //path[i].Velocity = path[i].Velocity + new Vector2(0, 3);
                    path[i].Position = path[i].Position + Velocity * (float)delta;
                }
            }
            else
            {
                Velocity.Y = 0;
            }
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

    private void MakeBall()
    {
        Node node = OptionBallPrefab.Instantiate();
        if (node is Node2D node2D)
        {
            node2D.TopLevel = true;
            balls.Insert(0, new Ball() { Node = node2D, Offset = 0 });
        }
        if(node is Bullet bullet)
        {
            bullet.Shooter = Player;
        }
        AddChild(node);
    }
}
