using RayTracerChallenge.Features;
using System;
using Xunit;

namespace Tests.RTC
{
    public class Points
    {
        [Fact]
        public void TupleWithOneIsPoint()
        {
            var a = new PointType<double>(4.3, -4.2, 3.1, 1.0);
            Assert.Equal(4.3, a.X);
            Assert.Equal(-4.2, a.Y);
            Assert.Equal(3.1, a.Z);
            Assert.Equal(1.0, a.W);
            Assert.True(a.IsPoint);
            Assert.False(a.IsVector);
        }

        [Fact]
        public void TupleWithZeroIsVector()
        {
            var a = new PointType<double>(4.3, -4.2, 3.1, 0.0);
            Assert.Equal(4.3, a.X);
            Assert.Equal(-4.2, a.Y);
            Assert.Equal(3.1, a.Z);
            Assert.Equal(0.0, a.W);
            Assert.False(a.IsPoint);
            Assert.True(a.IsVector);
        }

        [Fact]
        public void TestPointFunction()
        {
            var p = PointType<int>.PointToTuple(4, -4, 3);
            var e = new Tuple<int, int, int, int>(4, -4, 3, 1);

            Assert.Equal(e, p);
        }

        [Fact]
        public void TestVectorFunction()
        {
            var p = PointType<int>.VectorToTuple(4, -4, 3);
            var e = new Tuple<int, int, int, int>(4, -4, 3, 0);

            Assert.Equal(e, p);
        }

        [Fact]
        public void AddingTwoTuples()
        {
            var a1 = new Tuple<int, int, int, int>(3, -2, 5, 1);
            var a2 = new Tuple<int, int, int, int>(-2, 3, 1, 0);
            var expected = new Tuple<int, int, int, int>(1, 1, 6, 1);

            Assert.Equal(expected, PointType<int>.AddTuple(a1, a2));
        }

        [Fact]
        public void SubstractTwoPoints()
        {
            var p1 = PointType<int>.Point(3, 2, 1);
            var p2 = PointType<int>.Point(5, 6, 7);
            var v = PointType<int>.Vector(-2, -4, -6);
            var sub = p1 - p2;

            Assert.Equal(v, sub);
            Assert.True(sub.IsVector);
        }

        [Fact]
        public void SubstractVectorFromPoint()
        {
            var p1 = PointType<int>.Point(3, 2, 1);
            var v2 = PointType<int>.Vector(5, 6, 7);
            var p = PointType<int>.Point(-2, -4, -6);
            var sub = p1 - v2;

            Assert.Equal(p, sub);
            Assert.True(sub.IsPoint);
        }

        [Fact]
        public void SubstractTwoVectors()
        {
            var v1 = PointType<int>.Vector(3, 2, 1);
            var v2 = PointType<int>.Vector(5, 6, 7);
            var v = PointType<int>.Vector(-2, -4, -6);
            var sub = v1 - v2;

            Assert.Equal(v, sub);
            Assert.True(sub.IsVector);
        }

        [Fact]
        public void SubstractVectorFromZeroVector()
        {
            var v1 = PointType<int>.Vector(0, 0, 0);
            var v2 = PointType<int>.Vector(1, -2, 3);
            var v = PointType<int>.Vector(-1, 2, -3);
            var sub = v1 - v2;

            Assert.Equal(v, sub);
            Assert.True(sub.IsVector);
        }

        [Fact]
        public void NegateTuple()
        {
            var v1 = new Tuple<int, int, int, int>(1, -2, 3, -4);
            var expected = new Tuple<int, int, int, int>(-1, 2, -3, 4);
            var neg = PointType<int>.NegateTuple(v1);

            Assert.Equal(expected, neg);
        }

        [Fact]
        public void MultiplyTupleByScalar()
        {
            var v1 = new Tuple<double, double, double, double>(1, -2, 3, -4);
            var expected = new Tuple<double, double, double, double>(3.5, -7, 10.5, -14);
            var neg = PointType<double>.MultiplyTuple(v1, 3.5);

            Assert.Equal(expected, neg);
        }

        [Fact]
        public void MultiplyTupleByFraction()
        {
            var v1 = new Tuple<double, double, double, double>(1, -2, 3, -4);
            var expected = new Tuple<double, double, double, double>(0.5, -1, 1.5, -2);
            var neg = PointType<double>.MultiplyTuple(v1, 0.5);

            Assert.Equal(expected, neg);
        }

        [Fact]
        public void DivideTupleByScalar()
        {
            var v1 = new Tuple<double, double, double, double>(1, -2, 3, -4);
            var expected = new Tuple<double, double, double, double>(0.5, -1, 1.5, -2);
            var neg = PointType<double>.DivideTuple(v1, 2);

            Assert.Equal(expected, neg);
        }

        [Fact]
        public void MagnitudeX1()
        {
            var v1 = PointType<int>.Vector(1, 0, 0);
            var expected = 1;

            Assert.Equal(expected, v1.Magnetude());
        }
        [Fact]
        public void MagnitudeY1()
        {
            var v1 = PointType<int>.Vector(0, 1, 0);
            var expected = 1;

            Assert.Equal(expected, v1.Magnetude());
        }
        [Fact]
        public void MagnitudeZ1()
        {
            var v1 = PointType<int>.Vector(0, 0, 1);
            var expected = 1;

            Assert.Equal(expected, v1.Magnetude());
        }
        [Fact]
        public void Magnitude123()
        {
            var v1 = PointType<int>.Vector(1, 2, 3);
            var expected = Math.Sqrt(14);

            Assert.Equal(expected, v1.Magnetude());
        }
        [Fact]
        public void MagnitudeNeg123()
        {
            var v1 = PointType<int>.Vector(-1, -2, -3);
            var expected = Math.Sqrt(14);

            Assert.Equal(expected, v1.Magnetude());
        }

        [Fact]
        public void NormalizingSimple()
        {
            var v1 = PointType<double>.Vector(4, 0, 0);
            var expected = PointType<double>.Vector(1, 0, 0);

            Assert.Equal(expected, v1.Normalizing());
        }
        [Fact]
        public void NormalizingComplex()
        {
            var v1 = PointType<double>.Vector(1, 2, 3);
            var expected = PointType<double>.Vector(1 / Math.Sqrt(14), 2 / Math.Sqrt(14), 3 / Math.Sqrt(14));

            Assert.Equal(expected, v1.Normalizing());
        }

        [Fact]
        public void DotProduct()
        {
            var v1 = PointType<int>.Vector(1, 2, 3);
            var v2 = PointType<int>.Vector(2, 3, 4);
            var expected = 20;

            Assert.Equal(expected, PointType<int>.DotProduct(v1, v2));
        }
        [Fact]
        public void CrossProduct()
        {
            var v1 = PointType<int>.Vector(1, 2, 3);
            var v2 = PointType<int>.Vector(2, 3, 4);
            var expected1 = PointType<int>.Vector(-1, 2, -1);
            var expected2 = PointType<int>.Vector(1, -2, 1);

            Assert.Equal(expected1, PointType<int>.CrossProduct(v1, v2));
            Assert.Equal(expected2, PointType<int>.CrossProduct(v2, v1));
        }
    }
}