using System;
using System.Linq;

namespace RayTracerChallenge.Features
{
    public class PointType
    {
        public PointType(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public bool IsVector
        {
            get => (W == 0);
        }
        public bool IsPoint
        {
            get => (W == 1);
        }

        #region Operators
        public static PointType operator +(PointType a, PointType b)
            => new PointType(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        public static PointType operator -(PointType a, PointType b) 
            => new PointType(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        public static PointType operator -(PointType a)
            => new PointType(-a.X, -a.Y, -a.Z, -a.W);   
        public static PointType operator *(PointType a, double b)
            => new PointType(a.X * b, a.Y * b, a.Z * b, a.W * b);
        public static PointType operator /(PointType a, double b)
            => new PointType(a.X / b, a.Y / b, a.Z / b, a.W / b);
        #endregion


        public static PointType Point(double x, double y, double z)
            => new PointType(x, y, z, 1);
        public static PointType Vector(double x, double y, double z)
            => new PointType(x, y, z, 0);

        public double Magnetude()
        {
            if (!IsVector) throw new Exception("You can only calculate the magnetude of a vector.");

            return Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public PointType Normalize()
        {
            var m = Magnetude();
            if (m == 0) return new PointType(0, 0, 0, 0);

            return new PointType(
                X / m,
                Y / m,
                Z / m,
                W / m);
        }

        public static double DotProduct(PointType a, PointType b)
            => DotProductTuple(a.ToTuple(), b.ToTuple());
        public static PointType CrossProduct(PointType a, PointType b)
        {
            return Vector(a.Y * b.Z - a.Z * b.Y,
                          a.Z * b.X - a.X * b.Z,
                          a.X * b.Y - a.Y * b.X);
        }

        #region Tuples Methods
        public Tuple<double, double, double, double> ToTuple()
        {
            return new Tuple<double, double, double, double>(X, Y, Z, W);
        }
        public static Tuple<double, double, double, double> PointToTuple(double x, double y, double z)
        {
            return new Tuple<double, double, double, double>(x, y, z, 1);
        }
        public static Tuple<double, double, double, double> VectorToTuple(double x, double y, double z)
        {
            return new Tuple<double, double, double, double>(x, y, z, 0);
        }

        public static Tuple<double, double, double, double> AddTuple(Tuple<double, double, double, double> a1, Tuple<double, double, double, double> a2)
        {
            return new Tuple<double, double, double, double>(
                a1.Item1 + a2.Item1,
                a1.Item2 + a2.Item2,
                a1.Item3 + a2.Item3,
                a1.Item4 + a2.Item4);
        }
        public static Tuple<double, double, double, double> SubstractTuple(Tuple<double, double, double, double> a1, Tuple<double, double, double, double> a2)
        {
            return new Tuple<double, double, double, double>(
                a1.Item1 - a2.Item1,
                a1.Item2 - a2.Item2,
                a1.Item3 - a2.Item3,
                a1.Item4 - a2.Item4);
        }
        public static Tuple<double, double, double, double> NegateTuple(Tuple<double, double, double, double> a1)
        {
            return new Tuple<double, double, double, double>(
                -a1.Item1,
                -a1.Item2,
                -a1.Item3,
                -a1.Item4);
        }
        public static Tuple<double, double, double, double> MultiplyTuple(Tuple<double, double, double, double> a1, double a2)
        {
            return new Tuple<double, double, double, double>(
                a1.Item1 * a2,
                a1.Item2 * a2,
                a1.Item3 * a2,
                a1.Item4 * a2);
        }
        public static Tuple<double, double, double, double> DivideTuple(Tuple<double, double, double, double> a1, double a2)
        {
            return new Tuple<double, double, double, double>(
                a1.Item1 / a2,
                a1.Item2 / a2,
                a1.Item3 / a2,
                a1.Item4 / a2);
        }
        public static double DotProductTuple(Tuple<double, double, double, double> a, Tuple<double, double, double, double> b)
        {
            return a.Item1 * b.Item1 + a.Item2 * b.Item2 + a.Item3 * b.Item3 + a.Item4 * b.Item4;
        }
        #endregion

        private string Type()
        {
            if (IsVector) return "Vector";
            if (IsPoint) return "Point";
            return "nothing";
        }
        public string ToCoordinates() => $"[{X},{Y},{Z}]";
        public override string ToString() => $"[{X}, {Y}, {Z}, {W}][{Type()}]";
        public string ToString(string format)
            => $"[{X.ToString(format)}, {Y.ToString(format)}, {Z.ToString(format)}, {W.ToString(format)}][{Type()}]";
        public override bool Equals(object obj)
        {
            var coords = obj as PointType;
            return (X.CompareTo(coords.X) == 0) &&
                   (Y.CompareTo(coords.Y) == 0) &&
                   (Z.CompareTo(coords.Z) == 0) &&
                   (W.CompareTo(coords.W) == 0);
        }
        public bool Equals(PointType pt, int p)
        {
            return Math.Round(X, p) == Math.Round(pt.X, p) &&
                   Math.Round(Y, p) == Math.Round(pt.Y, p) &&
                   Math.Round(Z, p) == Math.Round(pt.Z, p) &&
                   Math.Round(W, p) == Math.Round(pt.W, p);
        }
        public override int GetHashCode() => base.GetHashCode();
    }
}