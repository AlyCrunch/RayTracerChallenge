using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Features.Patterns
{
    public class RadialGradient : Pattern
    {
        public RadialGradient() { Transform = Matrix.GetIdentity(); }
        public RadialGradient(Color a, Color b)
        {
            Transform = Matrix.GetIdentity();
            A = a;
            B = b;
        }
        public override Color At(PointType point)
        {
            var distance = B - A;

            var r = Math.Sqrt(point.X * point.X + point.Z * point.Z);
            var fraction =  r - Math.Floor(r);

            return A + distance * fraction;
        }
    }
}
