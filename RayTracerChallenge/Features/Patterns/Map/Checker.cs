using System;

namespace RayTracerChallenge.Features.Patterns.Map
{
    public class Checker : Pattern
    {
        public Checker(int width, int height, Color a, Color b)
        {
            A = a;
            B = b;
            Width = width;
            Height = height;
        }

        public override Color At(double u, double v)
        {
            var u2 = Math.Floor(u * Width);
            var v2 = Math.Floor(v * Height);

            return ((u2 + v2) % 2 == 0) ? A : B;
        }
    }
}
