using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class Enemy : Node2D, IWeaponHaver, IHurter, IFactionMember
    {
        [Export]
        public float HP;
        [Export]
        public PackedScene DeathPrefab;
        [Export]
        public bool Mirror;

        public List<Hurtbox> Hurtboxes = new List<Hurtbox>();

        public DeltaController TimeController = new DeltaControllerNode();

        int flash = 0;
        float hurtTime = 0;

        public bool Active;

        public Faction Faction => Faction.Enemy;

        public float MirrorMod => Mirror ? -1f : 1f;

        public override void _Ready()
        {
            base._Ready();

            foreach(var hurtbox in GetChildren().OfType<Hurtbox>())
            {
                Hurtboxes.Add(hurtbox);
            }

            var levelSegment = this.FindParent<LevelSegment>();
            if(levelSegment != null)
            {
                levelSegment.KeepAlive(this);
            }
        }

        public void AddWeapon(Weapon weapon)
        {
            //NOOP
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            flash++;

            hurtTime -= (float)delta;

            if (hurtTime > 0)
            {
                if (flash % 2 == 0)
                {
                    Modulate = new Color(3, 2, 2, 1);
                }
                else
                {
                    Modulate = new Color(1, 0.5f, 0.5f, 1);
                }
            }
            else
            {
                Modulate = new Color(1, 1, 1, 1);
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            TimeController.Update(delta);

            CheckDeath();
            CheckActive();
        }

        public void CheckActive(Vector2? pos = null)
        {
            pos ??= Position;

            Vector2 globalPos = this.ToParentGlobal(pos.Value);
            bool isOnScreen = MainCamera.Instance?.IsInView(globalPos) ?? false;
            if(isOnScreen)
            {
                Active = true;
            }
        }

        public void CheckDeath()
        {
            if (HP <= 0)
            {
                if (GodotObject.IsInstanceValid(DeathPrefab))
                {
                    var death = DeathPrefab.Instantiate<Node2D>();
                    death.GlobalPosition = GlobalPosition;
                    Game.Instance.AddChild(death);
                }
                QueueFree();
            }
        }

        public bool TakeDamage(float damage, bool contact = false)
        {
            //GD.Print($"Dealt {damage} damage");

            HP -= damage;
            hurtTime = 0.15f;

            return true;
        }
    }
}
