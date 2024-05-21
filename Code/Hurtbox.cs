using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    [GlobalClass]
    public partial class Hurtbox : Area2D
    {
        [Export]
        public DamageMask DamageMask { get; set; }

        [Signal]
        public delegate void OnHurtEventHandler(Hitbox other);

        public IHurter Hurter;
        public List<Hitbox> Contacts = new List<Hitbox>();

        public override void _Ready()
        {
            base._Ready();

            Monitoring = true;
            Monitorable = false;
            AreaEntered += Hurtbox_AreaEntered;
            AreaExited += Hurtbox_AreaExited;

            Hurter = this.FindParent<IHurter>();
        }

        private void Hurtbox_AreaExited(Area2D area)
        {
            if (area is Hitbox hitbox)
            {
                Contacts.Remove(hitbox);
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            foreach (var contact in Contacts)
            {
                if (contact.MultiHit)
                {
                    contact.Hit(this);
                }
            }
        }

        private void Hurtbox_AreaEntered(Area2D area)
        {
            if(area is Hitbox hitbox)
            {
                if(DamageMask != null && !DamageMask.IsWeak(hitbox.DamageType))
                {
                    return;
                }

                Contacts.Add(hitbox);

                hitbox.Hit(this);
                EmitSignal(SignalName.OnHurt, hitbox);
            }
        }
    }
}
