using System;

namespace RayTracerChallenge.Features.Patterns
{
    public class Checker : Pattern
    {
        public Checker() { Transform = Matrix.GetIdentity(); }
        public Checker(Color a, Color b)
        {
            Transform = Matrix.GetIdentity();
            A = a;
            B = b;
        }

        public override Color At(PointType point)
            => ((Math.Floor(point.X) + Math.Floor(point.Y) + Math.Floor(point.Z)) % 2 == 0) ? A : B;        
    }
}