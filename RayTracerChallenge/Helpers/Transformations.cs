using RayTracerChallenge.Features;
using RayTracerChallenge.Features.Shapes;
using System;

namespace RayTracerChallenge.Helpers
{
    public class Transformations
    {
        public static Matrix Translation(double x, double y, double z)
        {
            var m = Matrix.GetIdentity(4, 4);
            m[0, 3] = x;
            m[1, 3] = y;
            m[2, 3] = z;
            return m;
        }

        public static Matrix Scaling(double x, double y, double z)
        {
            var m = Matrix.GetIdentity(4, 4);
            m[0, 0] = x;
            m[1, 1] = y;
            m[2, 2] = z;
            return m;
        }

        public static Matrix RotationX(double rotation)
        {
            var m = Matrix.GetIdentity(4, 4);

            m[1, 1] = Math.Cos(rotation);
            m[1, 2] = -Math.Sin(rotation);
            m[2, 1] = Math.Sin(rotation);
            m[2, 2] = Math.Cos(rotation);

            return m;
        }

        public static Matrix RotationY(double rotation)
        {
            var m = Matrix.GetIdentity(4, 4);

            m[0, 0] = Math.Cos(rotation);
            m[0, 2] = Math.Sin(rotation);
            m[2, 0] = -Math.Sin(rotation);
            m[2, 2] = Math.Cos(rotation);

            return m;
        }

        public static Matrix RotationZ(double rotation)
        {
            var m = Matrix.GetIdentity(4, 4);

            m[0, 0] = Math.Cos(rotation);
            m[0, 1] = -Math.Sin(rotation);
            m[1, 0] = Math.Sin(rotation);
            m[1, 1] = Math.Cos(rotation);

            return m;
        }

        public static Matrix Shearing(double x1, double x2, double y1, double y2, double z1, double z2)
        {

            var m = Matrix.GetIdentity(4, 4);

            m[0, 1] = x1;
            m[0, 2] = x2;
            m[1, 0] = y1;
            m[1, 2] = y2;
            m[2, 0] = z1;
            m[2, 1] = z2;

            return m;
        }

        public static Intersection[] Intersect(Sphere s, Ray r)
        {
            return s.Intersect(r);
        }

        public static PointType Position(Ray r, double t)
            => r.Origin + r.Direction * t;

        public static Matrix ViewTransform(PointType from, PointType to, PointType up)
        {
            var forward = (to - from).Normalize();
            var left = PointType.CrossProduct(forward, up.Normalize());
            var trueUp = PointType.CrossProduct(left, forward);

            var orientation = new Matrix(4, 4);
            orientation.SetRow(0, new double[] { left.X, left.Y, left.Z, 0 });
            orientation.SetRow(1, new double[] { trueUp.X, trueUp.Y, trueUp.Z, 0 });
            orientation.SetRow(2, new double[] { -forward.X, -forward.Y, -forward.Z, 0 });
            orientation.SetRow(3, new double[] { 0, 0, 0, 1 });
            var t = Translation(-from.X, -from.Y, -from.Z);
            return orientation * t;
        }
    }
}