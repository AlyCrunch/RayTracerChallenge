using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Features.Patterns
{
    public class Nested : Pattern
    {
        public Pattern Main { get; set; }
        public Pattern Ap { get; set; }
        public Pattern Bp { get; set; }

        public Nested(Pattern main, Pattern a, Pattern b)
        {
            Main = main;
            Ap = a;
            Ap.Transform = Ap.Transform * Helpers.Transformations.Scaling(0.5, 1, 0.5);

            Bp = b;
            Bp.Transform = Bp.Transform * Helpers.Transformations.Scaling(0.5, 1, 0.5);
        }

        public override Color At(PointType point)
        {
            var colorMain = Main.At(point);

            if(colorMain == Main.A)
                return Ap.At(Ap.Transform.Inverse() * point);
            if(colorMain == Main.B)
                return Bp.At(Bp.Transform.Inverse() * point);

            var altMain = Color.White - colorMain;

            var colorA = colorMain * Ap.At(Ap.Transform.Inverse() * point);
            var colorB = altMain * Bp.At(Bp.Transform.Inverse() * point);
            
            return colorA + colorB;
        }
    }
}
