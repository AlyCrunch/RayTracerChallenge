using System;

namespace RayTracerChallenge.Features.Patterns
{
    public class Ring : Pattern
    {
        public Ring(Color a, Color b)
        {
            Transform = Matrix.GetIdentity();
            A = a;
            B = b;
        }
        public Ring() { Transform = Matrix.GetIdentity(); }

        public override Color At(PointType point)
            => (Math.Floor(Math.Sqrt(point.X * point.X + point.Z * point.Z)) % 2 == 0) ? A : B;
    }
}