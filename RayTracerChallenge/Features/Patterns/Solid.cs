using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Features.Patterns
{
    public class Solid : Pattern
    {
        public Color Color { get; set; }

        public Solid(Color a)
        {
            Transform = Matrix.GetIdentity();
            A = a;
        }
        public Solid() { Transform = Matrix.GetIdentity(); }

        public override Color At(PointType point)
            => A;
    }
}
