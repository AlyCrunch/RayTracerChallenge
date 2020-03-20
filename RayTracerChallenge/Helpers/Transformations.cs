using RayTracerChallenge.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Helpers
{
    public class Transformations
    {
        public static Matrix Translation(decimal x, decimal y, decimal z)
        {
            var m = Matrix.GetIdentity(4, 4);
            m[0, 3] = x;
            m[1, 3] = y;
            m[2, 3] = z;
            return m;
        }

        public static Matrix Scaling(decimal x, decimal y, decimal z)
        {
            var m = Matrix.GetIdentity(4, 4);
            m[0, 0] = x;
            m[1, 1] = y;
            m[2, 2] = z;
            return m;
        }

        public static Matrix RotationX(decimal rotation)
        {
            var r = decimal.ToDouble(rotation);
            var m = Matrix.GetIdentity(4, 4);

            m[1, 1] = (decimal)Math.Cos(r);
            m[1, 2] = -(decimal)Math.Sin(r);
            m[2, 1] = (decimal)Math.Sin(r);
            m[2, 2] = (decimal)Math.Cos(r);

            return m;
        }
        public static Matrix RotationX(double rotation)
        {
            return RotationX((decimal)rotation);
        }
    }
}