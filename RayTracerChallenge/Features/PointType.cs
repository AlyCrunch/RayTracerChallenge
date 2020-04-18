using System.Linq;
using System;

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
            get => (W == 0) ? true : false;
        }
        public bool IsPoint
        {
            get => (W == 1) ? true : false;
        }

        #region Operators
        public static PointType operator +(PointType a, PointType b)
        {
            return new PointType(Add(a.X, b.X), Add(a.Y, b.Y), Add(a.Z, b.Z), Add(a.W, b.W));
        }
        public static PointType operator -(PointType a, PointType b)
        {
            return new PointType(Subs(a.X, b.X), Subs(a.Y, b.Y), Subs(a.Z, b.Z), Subs(a.W, b.W));
        }
        public static PointType operator -(PointType a)
        {
            return new PointType(Neg(a.X), Neg(a.Y), Neg(a.Z), Neg(a.W));
        }
        public static PointType operator *(PointType a, double b)
        {
            return new PointType(Mult(a.X, b), Mult(a.Y, b), Mult(a.Z, b), Mult(a.W, b));
        }
        public static PointType operator /(PointType a, double b)
        {
            return new PointType(Div(a.X, b), Div(a.Y, b), Div(a.Z, b), Div(a.W, b));
        }
        #endregion

        #region Basic Operations
        private static double Add(params double[] n)
        {
            dynamic sum = 0;
            foreach (double value in n)
            {
                sum += value;
            }
            return sum;
        }

        private static double Subs(params double[] n)
        {
            dynamic subs = n[0];
            foreach (double value in n.Skip(1))
            {
                subs -= value;
            }
            return subs;
        }

        private static double Neg(double n1)
        {
            dynamic a = n1;
            return -a;
        }

        private static double Mult(params double[] n)
        {
            dynamic mult = n[0];
            foreach (double value in n.Skip(1))
            {
                mult *= value;
            }
            return mult;
        }

        private static double Div(double n1, double n2)
        {
            dynamic a = n1;
            dynamic b = n2;
            return a / b;
        }

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

            return new PointType(
                X / m,
                Y / m,
                Z / m,
                W / m);
        }

        public static double DotProduct(PointType a, PointType b)
        {
            return DotProductTuple(a.ToTuple(), b.ToTuple());
        }
        public static PointType CrossProduct(PointType a, PointType b)
        {
            return Vector(Subs(Mult(a.Y, b.Z), Mult(a.Z, b.Y)),
                Subs(Mult(a.Z, b.X), Mult(a.X, b.Z)),
                Subs(Mult(a.X, b.Y), Mult(a.Y, b.X)));
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
                Add(a1.Item1, a2.Item1),
                Add(a1.Item2, a2.Item2),
                Add(a1.Item3, a2.Item3),
                Add(a1.Item4, a2.Item4));
        }
        public static Tuple<double, double, double, double> SubstractTuple(Tuple<double, double, double, double> a1, Tuple<double, double, double, double> a2)
        {
            return new Tuple<double, double, double, double>(
                Subs(a1.Item1, a2.Item1),
                Subs(a1.Item2, a2.Item2),
                Subs(a1.Item3, a2.Item3),
                Subs(a1.Item4, a2.Item4));
        }
        public static Tuple<double, double, double, double> NegateTuple(Tuple<double, double, double, double> a1)
        {
            return new Tuple<double, double, double, double>(
                Neg(a1.Item1),
                Neg(a1.Item2),
                Neg(a1.Item3),
                Neg(a1.Item4));
        }
        public static Tuple<double, double, double, double> MultiplyTuple(Tuple<double, double, double, double> a1, double a2)
        {
            return new Tuple<double, double, double, double>(
                Mult(a1.Item1, a2),
                Mult(a1.Item2, a2),
                Mult(a1.Item3, a2),
                Mult(a1.Item4, a2));
        }
        public static Tuple<double, double, double, double> DivideTuple(Tuple<double, double, double, double> a1, double a2)
        {
            return new Tuple<double, double, double, double>(
                Div(a1.Item1, a2),
                Div(a1.Item2, a2),
                Div(a1.Item3, a2),
                Div(a1.Item4, a2));
        }
        public static double DotProductTuple(Tuple<double, double, double, double> a, Tuple<double, double, double, double> b)
        {
            return Add(Mult(a.Item1, b.Item1),
                       Mult(a.Item2, b.Item2),
                       Mult(a.Item3, b.Item3),
                       Mult(a.Item4, b.Item4));
        }
        #endregion

        private string Type()
        {
            if (IsVector) return "Vector";
            if (IsPoint) return "Point";
            return "nothing";
        }
        public string ToCoordinates() => $"[{X},{Y},{Z}]";
        public override string ToString()
        {
            return $"[{X}, {Y}, {Z}, {W}][{Type()}]";
        }
        public string ToString(string format)
        {
            return $"[{X.ToString(format)}, {Y.ToString(format)}, {Z.ToString(format)}, {W.ToString(format)}][{Type()}]";
        }
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
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}