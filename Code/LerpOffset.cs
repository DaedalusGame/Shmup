using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    public class Slider
    {
        public float Time;
        public float Duration;
        public float Slide => Mathf.Lerp(0, 1, Time / Duration);

        public Slider(float duration)
        {
            Duration = duration;
        }

        public void Update(float time)
        {
            Time += time;
        }
    }

    public class LerpSlide<T>
    {
        public Slider Slider;
        public Func<float, T> Function;
        public bool Locked;

        public bool Done => Slider.Slide >= 1 && !Locked;

        public LerpSlide(T value, float time = 0) : this(_ => value, time)
        {
        }

        public LerpSlide(Func<float, T> function, float time)
        {
            Function = function;
            Slider = new Slider(time);
        }

        public T Get()
        {
            return Get(Slider.Slide);
        }

        public T Get(float slide)
        {
            return Function(slide);
        }

        public void Update(float deltaTime)
        {
            Slider.Update(deltaTime);
        }
    }
}
