using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class LevelGenerator : Node2D
    {
        [Export]
        PackedScene[] SegmentPrefabs = new PackedScene[0];
        [Export]
        public float ScrollingSpeed = 100;

        List<LevelSegment> Segments = new List<LevelSegment>();
        float currentOffset;
        int currentSegment;

        public override void _PhysicsProcess(double delta)
        {
            Position += new Vector2(0, ScrollingSpeed) * (float)delta;


            var lastSegment = Segments.LastOrDefault();
            var firstSegment = Segments.FirstOrDefault();
            if (lastSegment.IsValid())
            {
                var viewCoords = MainCamera.Instance.GetViewportTransform() * new Vector2(0, lastSegment.Top);
                if(viewCoords.Y > 0)
                {
                    CreateNewSegment();
                }
            }
            else
            {
                CreateNewSegment();
            }

            if(firstSegment.IsValid())
            {
                if (!firstSegment.IsActive && !firstSegment.ShouldKeepAlive())
                {
                    firstSegment.QueueFree();
                }
            }

            Segments.RemoveAll(x => !x.IsValid());
        }

        public void CreateNewSegment()
        {
            if(currentSegment < 0)
            {
                return;
            }

            var prefab = SegmentPrefabs[currentSegment % SegmentPrefabs.Length];

            if(prefab != null)
            {
                var segment = prefab.Instantiate<LevelSegment>();
                segment.Position = new Vector2(0, currentOffset);
                currentOffset -= segment.Length;
                AddChild(segment);
                Segments.Add(segment);

                currentSegment++;
            }
        }
    }
}
