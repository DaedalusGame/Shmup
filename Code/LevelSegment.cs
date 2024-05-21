using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    [Tool]
    public partial class LevelSegment : Node2D
    {
        public const int LEVEL_WIDTH = 240;

        [Export]
        public int Length;

        public bool IsActive = true;
        public float Top => GlobalPosition.Y - Length;
        HashSet<Node> KeepAlives = new HashSet<Node>();



        public bool ShouldKeepAlive()
        {
            return KeepAlives.Any(x => x.IsValid());
        }

        public override void _Process(double delta)
        {
            if (Engine.IsEditorHint())
            {
                this.QueueRedraw();
            }

            var viewCoords = MainCamera.Instance.GetViewportTransform() * new Vector2(0, Top);
            if (viewCoords.Y > MainCamera.Instance.GetViewportRect().End.Y)
            {
                IsActive = false;
            }
        }

        public void KeepAlive(Node node)
        {
            KeepAlives.Add(node);
        }

        public override void _Draw()
        {
            //if(Engine.IsEditorHint())
            //{
                DrawString(ThemeDB.FallbackFont, new Vector2(-LEVEL_WIDTH / 2, -Length), Name, HorizontalAlignment.Left, fontSize: 12, modulate: Colors.Green);
                DrawRect(new Rect2(-LEVEL_WIDTH / 2, -Length, LEVEL_WIDTH, Length), Colors.Green, false);
            //}
        }
    }
}
