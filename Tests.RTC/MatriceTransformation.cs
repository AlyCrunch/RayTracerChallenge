using RTF = RayTracerChallenge.Features;
using RTH = RayTracerChallenge.Helpers;
using System;
using Xunit;

namespace Tests.RTC
{
    public class MatriceTransformation
    {
        [Fact]
        public void MultiplyingTranslationMatrix()
        {
            var transform = RTH.Transformations.Translation(5, -3, 2);
            var p = RTF.PointType.Point(-3, 4, 5);
            var expected = RTF.PointType.Point(2, 1, 7);

            Assert.Equal(expected, transform * p);
        }
        [Fact]
        public void MultiplyingInvertedTranslationMatrix()
        {
            var transform = RTH.Transformations.Translation(5, -3, 2);
            var inv = transform.Inverse();
            var p = RTF.PointType.Point(-3, 4, 5);
            var expected = RTF.PointType.Point(-8, 7, 3);

            Assert.Equal(expected, inv * p);
        }
        [Fact]
        public void TranslateVectorNotAffected()
        {
            var transform = RTH.Transformations.Translation(5, -3, 2);
            var v = RTF.PointType.Vector(-3, 4, 5);

            Assert.Equal(v, transform * v);
        }

        [Fact]
        public void ScalingMatrixPoint()
        {
            var transform = RTH.Transformations.Scaling(2, 3, 4);
            var p = RTF.PointType.Point(-4, 6, 8);
            var expected = RTF.PointType.Point(-8, 18, 32);

            Assert.Equal(expected, transform * p);
        }
        [Fact]
        public void ScalingMatrixVector()
        {
            var transform = RTH.Transformations.Scaling(2, 3, 4);
            var p = RTF.PointType.Vector(-4, 6, 8);
            var expected = RTF.PointType.Vector(-8, 18, 32);

            Assert.Equal(expected, transform * p);
        }
        [Fact]
        public void InverseScalingMatrix()
        {
            var transform = RTH.Transformations.Scaling(2, 3, 4);
            var inverse = transform.Inverse();
            var p = RTF.PointType.Vector(-4, 6, 8);

            var expected = RTF.PointType.Vector(-2, 2, 2);

            Assert.Equal(expected, inverse * p);
        }
        [Fact]
        public void ReflexionScalingNegativeValue()
        {
            var transform = RTH.Transformations.Scaling(-1, 1, 1);
            var p = RTF.PointType.Point(2, 3, 4);

            var expected = RTF.PointType.Point(-2, 3, 4);

            Assert.Equal(expected, transform * p);
        }

        [Fact]
        public void RotationPointXAxis()
        {
            var p = RTF.PointType.Point(0, 1, 0);

            var halfQuarter = RTH.Transformations.RotationX(Math.PI / 4);
            var expected1 = RTF.PointType.Point(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2);
            Assert.Equal(expected1, halfQuarter * p);

            var fullQuarter = RTH.Transformations.RotationX(Math.PI / 2);
            var expected2 = RTF.PointType.Point(0, 0, 1);
            Assert.Equal(expected2, fullQuarter * p);
        }
    }
}
