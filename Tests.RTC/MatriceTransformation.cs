using RTF = RayTracerChallenge.Features;
using RTH = RayTracerChallenge.Helpers;
using System;
using Xunit;
using Tests.RTC.Helpers;

namespace Tests.RTC
{
    public class MatriceTransformation
    {
        #region Translation
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
        #endregion

        #region Scaling
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
        #endregion

        #region Rotation
        [Fact]
        public void RotationPointXAxis()
        {
            var p = RTF.PointType.Point(0, 1, 0);

            var halfQuarter = RTH.Transformations.RotationX(Math.PI / 4);
            var expected1 = RTF.PointType.Point(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2);
            Assert.Equal(expected1, halfQuarter * p);

            var fullQuarter = RTH.Transformations.RotationX(Math.PI / 2);
            var expected2 = RTF.PointType.Point(0, 0, 1);
            CustomAssert.Equal(expected2, fullQuarter * p, 0);
        }
        [Fact]
        public void InvertedRotationPointXAxis()
        {
            var p = RTF.PointType.Point(0, 1, 0);

            var halfQuarter = RTH.Transformations.RotationX(Math.PI / 4);
            var inverted = halfQuarter.Inverse();

            var expected = RTF.PointType.Point(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
            CustomAssert.Equal(expected, inverted * p, 5);
        }
        [Fact]
        public void RotationPointYAxis()
        {
            var p = RTF.PointType.Point(0, 0, 1);

            var halfQuarter = RTH.Transformations.RotationY(Math.PI / 4);
            var expected1 = RTF.PointType.Point(Math.Sqrt(2) / 2, 0, Math.Sqrt(2) / 2);
            Assert.Equal(expected1, halfQuarter * p);

            var fullQuarter = RTH.Transformations.RotationY(Math.PI / 2);
            var expected2 = RTF.PointType.Point(1, 0, 0);
            CustomAssert.Equal(expected2, fullQuarter * p, 0);
        }
        [Fact]
        public void RotationPointZAxis()
        {
            var p = RTF.PointType.Point(0, 1, 0);

            var halfQuarter = RTH.Transformations.RotationZ(Math.PI / 4);
            var expected1 = RTF.PointType.Point(-Math.Sqrt(2) / 2, Math.Sqrt(2) / 2, 0);
            Assert.Equal(expected1, halfQuarter * p);

            var fullQuarter = RTH.Transformations.RotationZ(Math.PI / 2);
            var expected2 = RTF.PointType.Point(-1, 0, 0);
            CustomAssert.Equal(expected2, fullQuarter * p, 0);
        }
        #endregion

        #region Shearing
        [Fact]
        public void ShearingXtoZ()
        {
            var transform = RTH.Transformations.Shearing(0, 1, 0, 0, 0, 0);
            var p = RTF.PointType.Point(2, 3, 4);
            var expected = RTF.PointType.Point(6, 3, 4);

            Assert.Equal(expected, transform * p);
        }
        [Fact]
        public void ShearingYtoX()
        {
            var transform = RTH.Transformations.Shearing(0, 0, 1, 0, 0, 0);
            var p = RTF.PointType.Point(2, 3, 4);
            var expected = RTF.PointType.Point(2, 5, 4);

            Assert.Equal(expected, transform * p);
        }
        [Fact]
        public void ShearingYtoZ()
        {
            var transform = RTH.Transformations.Shearing(0, 0, 0, 1, 0, 0);
            var p = RTF.PointType.Point(2, 3, 4);
            var expected = RTF.PointType.Point(2, 7, 4);

            Assert.Equal(expected, transform * p);
        }
        [Fact]
        public void ShearingZtoX()
        {
            var transform = RTH.Transformations.Shearing(0, 0, 0, 0, 1, 0);
            var p = RTF.PointType.Point(2, 3, 4);
            var expected = RTF.PointType.Point(2, 3, 6);

            Assert.Equal(expected, transform * p);
        }
        [Fact]
        public void ShearingZtoY()
        {
            var transform = RTH.Transformations.Shearing(0, 0, 0, 0, 0, 1);
            var p = RTF.PointType.Point(2, 3, 4);
            var expected = RTF.PointType.Point(2, 3, 7);

            Assert.Equal(expected, transform * p);
        }
        #endregion

        [Fact]
        public void TransformationSequence()
        {
            var p = RTF.PointType.Point(1, 0, 1);
            var a = RTH.Transformations.RotationX(Math.PI / 2);
            var b = RTH.Transformations.Scaling(5, 5, 5);
            var c = RTH.Transformations.Translation(10, 5, 7);

            var p2 = a * p;
            var expected1 = RTF.PointType.Point(1, -1, 0);
            CustomAssert.Equal(expected1, p2, 0);

            var p3 = b * p2;
            var expected2 = RTF.PointType.Point(5, -5, 0);
            CustomAssert.Equal(expected2, b * p2, 0);

            var p4 = c * p3;
            var expected3 = RTF.PointType.Point(15, 0, 7);
            CustomAssert.Equal(expected3, p4, 0);
        }
        [Fact]
        public void TransformationChained()
        {
            var p = RTF.PointType.Point(1, 0, 1);
            var a = RTH.Transformations.RotationX(Math.PI / 2);
            var b = RTH.Transformations.Scaling(5, 5, 5);
            var c = RTH.Transformations.Translation(10, 5, 7);

            var t = c * b * a;

            var expected = RTF.PointType.Point(15, 0, 7);
            CustomAssert.Equal(expected, t * p, 0);
        }

        [Fact]
        public void TestPIT()
        {
            var twelve = RTF.PointType.Point(0, 0, 1);
            var r = RTH.Transformations.RotationY(3 * (Math.PI / 6));
            var r2 = RTH.Transformations.RotationY(6 * (Math.PI / 6));

            var three = RTF.PointType.Point(1, 0, 0);
            var six = RTF.PointType.Point(0, 0, -1);

            CustomAssert.Equal(three, r * twelve, 0);
            CustomAssert.Equal(six, r2 * twelve, 0);
        }
    }
}
