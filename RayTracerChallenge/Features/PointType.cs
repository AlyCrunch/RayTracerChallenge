using RayTracerChallenge.Helpers;
using System.Linq;
using System;

namespace RayTracerChallenge.Features
{
    public class PointType<T> where T : IComparable<T>
    {
        public PointType(T x, T y, T z, T w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;

            Init();
        }
        private void Init()
        {
            zero = TConverter.ChangeType<T>(0);
            one = TConverter.ChangeType<T>(1);
        }

        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }
        public T W { get; set; }

        private T zero;
        private T one;

        public bool IsVector
        {
            get => (W.CompareTo(zero) == 0) ? true : false;
        }
        public bool IsPoint
        {
            get => (W.CompareTo(one) == 0) ? true : false;
        }

        #region Operators
        public static PointType<T> operator +(PointType<T> a, PointType<T> b)
        {
            return new PointType<T>(Add(a.X, b.X), Add(a.Y, b.Y), Add(a.Z, b.Z), Add(a.W, b.W));
        }
        public static PointType<T> operator -(PointType<T> a, PointType<T> b)
        {
            return new PointType<T>(Subs(a.X, b.X), Subs(a.Y, b.Y), Subs(a.Z, b.Z), Subs(a.W, b.W));
        }
        public static PointType<T> operator -(PointType<T> a)
        {
            return new PointType<T>(Neg(a.X), Neg(a.Y), Neg(a.Z), Neg(a.W));
        }
        public static PointType<T> operator *(PointType<T> a, T b)
        {
            return new PointType<T>(Mult(a.X, b), Mult(a.Y, b), Mult(a.Z, b), Mult(a.W, b));
        }
        public static PointType<T> operator /(PointType<T> a, T b)
        {
            return new PointType<T>(Div(a.X, b), Div(a.Y, b), Div(a.Z, b), Div(a.W, b));
        }
        #endregion

        #region Basic Operations
        private static T Add(params T[] n)
        {
            dynamic sum = 0;
            foreach (T value in n)
            {
                sum += value;
            }
            return sum;
        }

        private static T Subs(params T[] n)
        {
            dynamic subs = n[0];
            foreach (T value in n.Skip(1))
            {
                subs -= value;
            }
            return subs;
        }

        private static T Neg(T n1)
        {
            dynamic a = n1;
            return -a;
        }

        private static T Mult(params T[] n)
        {
            dynamic mult = n[0];
            foreach (T value in n.Skip(1))
            {
                mult *= value;
            }
            return mult;
        }

        private static T Div(T n1, T n2)
        {
            dynamic a = n1;
            dynamic b = n2;
            return a / b;
        }

        #endregion

        public static PointType<T> Point(T x, T y, T z)
        {
            var _one = TConverter.ChangeType<T>(1);
            return new PointType<T>(x, y, z, _one);
        }
        public static PointType<T> Vector(T x, T y, T z)
        {
            var _zero = TConverter.ChangeType<T>(0);
            return new PointType<T>(x, y, z, _zero);
        }

        public double Magnetude()
        {
            if (!IsVector) throw new Exception("You can only calculate the magnetude of a vector.");

            dynamic x = X;
            dynamic y = Y;
            dynamic z = Z;
            dynamic w = W;

            return Math.Sqrt(x * x + y * y + z * z + w * w);
        }

        public PointType<T> Normalizing()
        {
            var m = Magnetude();

            dynamic x = X;
            dynamic y = Y;
            dynamic z = Z;
            dynamic w = W;

            var magn = TConverter.ChangeType<T>(m);

            return new PointType<T>(
                x / magn,
                y / magn,
                z / magn,
                w / magn);
        }
        
        public static T DotProduct(PointType<T> a, PointType<T> b)
        {
            return DotProductTuple(a.ToTuple(), b.ToTuple());
        }
        public static PointType<T> CrossProduct(PointType<T> a, PointType<T> b)
        {
            return Vector(Subs(Mult(a.Y, b.Z), Mult(a.Z, b.Y)),
                Subs(Mult(a.Z, b.X), Mult(a.X, b.Z)),
                Subs(Mult(a.X, b.Y), Mult(a.Y, b.X)));
        }

        #region Tuples Methods
        public Tuple<T, T, T, T> ToTuple()
        {
            return new Tuple<T, T, T, T>(X, Y, Z, W);
        }
        public static Tuple<T, T, T, T> PointToTuple(T x, T y, T z)
        {
            var _one = TConverter.ChangeType<T>(1);
            return new Tuple<T, T, T, T>(x, y, z, _one);
        }
        public static Tuple<T, T, T, T> VectorToTuple(T x, T y, T z)
        {
            var _zero = TConverter.ChangeType<T>(0);
            return new Tuple<T, T, T, T>(x, y, z, _zero);
        }

        public static Tuple<T, T, T, T> AddTuple(Tuple<T, T, T, T> a1, Tuple<T, T, T, T> a2)
        {
            return new Tuple<T, T, T, T>(
                Add(a1.Item1, a2.Item1),
                Add(a1.Item2, a2.Item2),
                Add(a1.Item3, a2.Item3),
                Add(a1.Item4, a2.Item4));
        }
        public static Tuple<T, T, T, T> SubstractTuple(Tuple<T, T, T, T> a1, Tuple<T, T, T, T> a2)
        {
            return new Tuple<T, T, T, T>(
                Subs(a1.Item1, a2.Item1),
                Subs(a1.Item2, a2.Item2),
                Subs(a1.Item3, a2.Item3),
                Subs(a1.Item4, a2.Item4));
        }
        public static Tuple<T, T, T, T> NegateTuple(Tuple<T, T, T, T> a1)
        {
            return new Tuple<T, T, T, T>(
                Neg(a1.Item1),
                Neg(a1.Item2),
                Neg(a1.Item3),
                Neg(a1.Item4));
        }
        public static Tuple<T, T, T, T> MultiplyTuple(Tuple<T, T, T, T> a1, T a2)
        {
            return new Tuple<T, T, T, T>(
                Mult(a1.Item1, a2),
                Mult(a1.Item2, a2),
                Mult(a1.Item3, a2),
                Mult(a1.Item4, a2));
        }
        public static Tuple<T, T, T, T> DivideTuple(Tuple<T, T, T, T> a1, T a2)
        {
            return new Tuple<T, T, T, T>(
                Div(a1.Item1, a2),
                Div(a1.Item2, a2),
                Div(a1.Item3, a2),
                Div(a1.Item4, a2));
        }
        public static T DotProductTuple(Tuple<T, T, T, T> a, Tuple<T, T, T, T> b)
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
            return $"[{X.ToString()},{Y.ToString()},{Z.ToString()},{W.ToString()}][{Type()}]";
        }
        public override bool Equals(object obj)
        {
            var coords = obj as PointType<T>;
            return (X.CompareTo(coords.X) == 0) &&
                   (Y.CompareTo(coords.Y) == 0) &&
                   (Z.CompareTo(coords.Z) == 0) &&
                   (W.CompareTo(coords.W) == 0);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}