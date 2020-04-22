using System;

namespace RayTracerChallenge.Features.Patterns
{
    public class Gradient : Pattern
    {
        public Gradient(){ Transform = Matrix.GetIdentity(); }
        public Gradient(Color a, Color b)
        {
            Transform = Matrix.GetIdentity();
            A = a;
            B = b;
        }

        public override Color At(PointType point)
        {
            var distance = B - A;
            var fraction = point.X - Math.Floor(point.X);

            return A + distance * fraction;
        }
    }
}