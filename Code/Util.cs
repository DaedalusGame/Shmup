using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    public static class Util
    {
        public static float ReverseLerp(float n, float lower, float upper)
        {
            return (n - lower) / (upper - lower);
        }

        public static double ReverseLerp(double n, double lower, double upper)
        {
            return (n - lower) / (upper - lower);
        }

        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return min + NextFloat(random) * (max - min);
        }

        public static Vector2 NextVector2(this Random random, Rect2 rect)
        {
            return new Vector2(random.NextFloat(rect.Position.X, rect.End.X), random.NextFloat(rect.Position.Y, rect.End.Y));
        }

        public static bool IsValid(this Node node)
        {
            return node != null && GodotObject.IsInstanceValid(node) && !node.IsQueuedForDeletion();
        }

        public static Color WithAlpha(this Color color, float alpha)
        {
            color.A = alpha;
            return color;
        }

        public static T FindParent<T>(this Node node)
        {
            for(Node current = node; current != null; current = current.GetParent())
            {
                if(current is T t)
                {
                    return t;
                }
            }

            return default(T);
        }

        public static T FindNode<T>(this Node node)
        {
            return node.GetChildren().OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> FindNodes<T>(this Node node)
        {
            return node.GetChildren().OfType<T>();
        }

        public static Vector2 ToParentLocal(this Node2D node, Vector2 globalPos)
        {
            var parent = node.GetParent<Node2D>();
            return parent?.ToLocal(globalPos) ?? globalPos;
        }

        public static Vector2 ToParentGlobal(this Node2D node, Vector2 localPos)
        {
            var parent = node.GetParent<Node2D>();
            return parent?.ToGlobal(localPos) ?? localPos;
        }
    }
}
