using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Features.Patterns
{
    public class Blended : Pattern
    {
        public Pattern Ap { get; set; }
        public Pattern Bp { get; set; }

        public Blended(Pattern a, Pattern b)
        {
            Ap = a;
            Bp = b;
        }

        public override Color At(PointType point)
        {
            var colorA = Ap.At(Ap.Transform.Inverse() * point);
            var colorB = Bp.At(Bp.Transform.Inverse() * point);

            return (colorA + colorB) * 0.5;
        }
    }
}
