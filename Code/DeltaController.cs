using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    public class DeltaController
    {
        public float StopTime;
        public float SlowTime;

        public virtual float GetDeltaTime(double delta)
        {
            if(StopTime > 0)
            {
                return 0;
            }

            if(SlowTime > 0)
            {
                delta *= 0.25f;
            }

            return (float)delta;
        }

        public float Update(double delta)
        {
            float deltaTrue = GetDeltaTime(delta);
            StopTime = Mathf.Max(0, StopTime - (float)delta);
            SlowTime = Mathf.Max(0, SlowTime - (float)delta);
            return deltaTrue;
        }
    }

    public class DeltaControllerNode : DeltaController
    {
        public override float GetDeltaTime(double delta)
        {
            return Mathf.Min(base.GetDeltaTime(delta), Game.Instance.TimeController.GetDeltaTime(delta));
        }
    }
}
