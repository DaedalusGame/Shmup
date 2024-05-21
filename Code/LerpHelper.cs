using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    public static class LerpHelper
    {
        public delegate float Delegate(float a, float b, float amt);

        public static Func<float, float> Lerp01(this Delegate func)
        {
            return (amt) => func(0, 1, amt);
        }

        public static Delegate Invert(this Delegate lerp)
        {
            return (a, b, amt) => lerp(b, a, amt);
        }

        public static Delegate ForwardReverse(float start, float end, Delegate lerpStart, Delegate lerpEnd)
        {
            return (a, b, amt) => ForwardReverse(a, b, amt, start, end, lerpStart, lerpEnd);
        }

        public static float ForwardReverse(float a, float b, float amt)
        {
            if (amt < 0.5)
                return Linear(a, b, amt * 2);
            else
                return Linear(b, a, (amt - 0.5f) * 2);
        }

        public static float ForwardReverse(float a, float b, float amt, float start, float end, Delegate lerpStart, Delegate lerpEnd)
        {
            if (amt < start)
                return lerpStart(a, b, Util.ReverseLerp(amt, 0, start));
            else if (amt < end)
                return b;
            else
                return lerpEnd(b, a, Util.ReverseLerp(amt, end, 1));
        }

        public static float Flick(float a, float b, float amt)
        {
            if (amt < 1)
                return a;
            else
                return b;
        }

        public static float Linear(float a, float b, float amt)
        {
            return a * (1 - amt) + b * amt;
        }

        public static float QuadraticIn(float a, float b, float amt)
        {
            return Linear(a, b, amt * amt);
        }

        public static float QuadraticOut(float a, float b, float amt)
        {
            return Linear(a, b, 1 - (amt - 1) * (amt - 1));
        }

        public static float Quadratic(float a, float b, float amt)
        {
            amt *= 2;
            if (amt < 1)
                return Linear(a, b, 0.5f * amt * amt);
            else
                return Linear(a, b, -0.5f * ((amt - 1) * (amt - 3) - 1));
        }

        public static float CubicIn(float a, float b, float amt)
        {
            return Linear(a, b, amt * amt * amt);
        }

        public static float CubicOut(float a, float b, float amt)
        {
            return Linear(a, b, 1 + (amt - 1) * (amt - 1) * (amt - 1));
        }

        public static float Cubic(float a, float b, float amt)
        {
            amt *= 2;
            if (amt < 1)
                return Linear(a, b, 0.5f * amt * amt * amt);
            else
                return Linear(a, b, 0.5f * ((amt - 2) * (amt - 2) * (amt - 2) + 2));
        }

        public static float QuarticIn(float a, float b, float amt)
        {
            return Linear(a, b, amt * amt * amt * amt);
        }

        public static float QuarticOut(float a, float b, float amt)
        {
            return Linear(a, b, 1 - (amt - 1) * (amt - 1) * (amt - 1) * (amt - 1));
        }

        public static float Quartic(float a, float b, float amt)
        {
            amt *= 2;
            if (amt < 1)
                return Linear(a, b, 0.5f * amt * amt * amt * amt);
            else
                return Linear(a, b, -0.5f * ((amt - 2) * (amt - 2) * (amt - 2) * (amt - 2) - 2));
        }

        public static float QuinticIn(float a, float b, float amt)
        {
            return Linear(a, b, amt * amt * amt * amt * amt);
        }

        public static float QuinticOut(float a, float b, float amt)
        {
            return Linear(a, b, 1 + (amt - 1) * (amt - 1) * (amt - 1) * (amt - 1) * (amt - 1));
        }

        public static float Quintic(float a, float b, float amt)
        {
            amt *= 2;
            if (amt < 1)
                return Linear(a, b, 0.5f * amt * amt * amt * amt * amt);
            else
                return Linear(a, b, 0.5f * ((amt - 2) * (amt - 2) * (amt - 2) * (amt - 2) * (amt - 2) + 2));
        }

        public static float SineIn(float a, float b, float amt)
        {
            return Linear(a, b, 1 - Mathf.Cos(amt * Mathf.Pi / 2));
        }

        public static float SineOut(float a, float b, float amt)
        {
            return Linear(a, b, Mathf.Sin(amt * Mathf.Pi / 2));
        }

        public static float Sine(float a, float b, float amt)
        {
            return Linear(a, b, 0.5f * (1 - Mathf.Cos(amt * Mathf.Pi)));
        }

        public static float ExponentialIn(float a, float b, float amt)
        {
            if (amt <= 0)
                return a;
            if (amt >= 1)
                return b;
            return Linear(a, b, Mathf.Pow(1024, amt - 1));
        }

        public static float ExponentialOut(float a, float b, float amt)
        {
            if (amt <= 0)
                return a;
            if (amt >= 1)
                return b;
            return Linear(a, b, 1 - Mathf.Pow(2, -10 * amt));
        }

        public static float Exponential(float a, float b, float amt)
        {
            if (amt <= 0)
                return a;
            if (amt >= 1)
                return b;
            amt *= 2;
            if (amt < 1)
                return Linear(a, b, 0.5f * Mathf.Pow(1024, amt - 1));
            else
                return Linear(a, b, -0.5f * Mathf.Pow(2, -10 * (amt - 1)) + 1);
        }

        public static float CircularIn(float a, float b, float amt)
        {
            return Linear(a, b, 1 - Mathf.Sqrt(1 - amt * amt));
        }

        public static float CircularOut(float a, float b, float amt)
        {
            return Linear(a, b, Mathf.Sqrt(1 - (amt - 1) * (amt - 1)));
        }

        public static float Circular(float a, float b, float amt)
        {
            amt *= 2;
            if (amt < 1)
                return Linear(a, b, -0.5f * (Mathf.Sqrt(1 - amt * amt) - 1));
            else
                return Linear(a, b, 0.5f * (Mathf.Sqrt(1 - (amt - 2) * (amt - 2)) + 1));
        }

        public static float ElasticIn(float a, float b, float amt)
        {
            return ElasticInCustom(a, b, 0.1f, 0.4f, amt);
        }

        public static float ElasticInCustom(float a, float b, float k, float p, float amt)
        {
            if (amt <= 0)
                return a;
            if (amt >= 1)
                return b;

            float s;

            if (k < 1)
            {
                k = 1;
                s = p / 4;
            }
            else
            {
                s = p * Mathf.Asin(1 / k) / (Mathf.Pi * 2);
            }

            return Linear(a, b, -k * Mathf.Pow(2, 10 * (amt - 1)) * Mathf.Sin(((amt - 1) - s) * (2 * Mathf.Pi) / p));
        }

        public static float ElasticOut(float a, float b, float amt)
        {
            return ElasticOutCustom(a, b, 0.1f, 0.4f, amt);
        }

        public static float ElasticOutCustom(float a, float b, float k, float p, float amt)
        {
            if (amt <= 0)
                return a;
            if (amt >= 1)
                return b;

            float s;

            if (k < 1)
            {
                k = 1;
                s = p / 4;
            }
            else
            {
                s = p * Mathf.Asin(1 / k) / (Mathf.Pi * 2);
            }

            return Linear(a, b, k * Mathf.Pow(2, -10 * amt) * Mathf.Sin((amt - s) * (2 * Mathf.Pi) / p) + 1);
        }

        public static float Elastic(float a, float b, float amt)
        {
            return ElasticCustom(a, b, 0.1f, 0.4f, amt);
        }

        public static float ElasticCustom(float a, float b, float k, float p, float amt)
        {
            if (amt <= 0)
                return a;
            if (amt >= 1)
                return b;

            float s;

            if (k < 1)
            {
                k = 1;
                s = p / 4;
            }
            else
            {
                s = p * Mathf.Asin(1 / k) / (Mathf.Pi * 2);
            }

            amt *= 2;
            if (amt < 1)
                return Linear(a, b, -0.5f * k * Mathf.Pow(2, 10 * (amt - 1)) * Mathf.Sin(((amt - 1) - s) * (2 * Mathf.Pi) / p));
            else
                return Linear(a, b, 0.5f * k * Mathf.Pow(2, -10 * (amt - 1)) * Mathf.Sin(((amt - 1) - s) * (2 * Mathf.Pi) / p) + 1);
        }

        public static float BackIn(float a, float b, float amt) => BackInCustom(a, b, 1.7f, amt);

        public static float BackInCustom(float a, float b, float c, float amt)
        {
            if (amt <= 0)
                return a;
            if (amt >= 1)
                return b;

            return (c + 1) * amt * amt * amt;
        }

        public static float BackOut(float a, float b, float amt) => BackOutCustom(a, b, 1.7f, amt);

        public static float BackOutCustom(float a, float b, float c, float amt)
        {
            if (amt <= 0)
                return a;
            if (amt >= 1)
                return b;

            return 1 + (c + 1) * Mathf.Pow(amt - 1, 3) + c * Mathf.Pow(amt - 1, 2);
        }
    }
}
