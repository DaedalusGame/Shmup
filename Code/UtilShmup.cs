using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    public static class UtilShmup
    {
        public static Bullet ShootBullet(this Node shooter, PackedScene bulletPrefab, Vector2 pos, Vector2 speed, Node2D parent = null)
        {
            bool global = false;

            if(parent == null)
            {
                parent = Game.Instance;
                global = true;
            }

            Node node = bulletPrefab.Instantiate();
            if (node is Bullet bullet)
            {
                if (global)
                {
                    bullet.GlobalPosition = pos;
                }
                else
                {
                    bullet.Position = pos;
                }
                bullet.Shooter = shooter;
                bullet.Speed = speed;

                parent?.AddChild(node);
                return bullet;
            }

            return null;
        }
    }
}
