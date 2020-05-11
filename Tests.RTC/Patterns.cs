using System;
using RTF = RayTracerChallenge.Features;
using pt = RayTracerChallenge.Features.PointType;
using tf = RayTracerChallenge.Helpers.Transformations;
using shapes = RayTracerChallenge.Features.Shapes;
using RayTracerChallenge.Features.Patterns;
using Tests.RTC.Helpers;
using Xunit;

namespace Tests.RTC
{
    public class Patterns
    {
        [Fact]
        public void CreatingPattern()
        {
            var pattern = Pattern.GetStripePattern
                (RTF.Color.White, RTF.Color.Black);
            Assert.Equal(RTF.Color.White, pattern.A);
            Assert.Equal(RTF.Color.Black, pattern.B);
        }

        [Fact]
        public void StripePatternConstantInY()
        {
            var pattern = Pattern.GetStripePattern
                (RTF.Color.White, RTF.Color.Black);

            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 0)));
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 1, 0)));
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 2, 0)));
        }
        [Fact]
        public void StripePatternConstantInZ()
        {
            var pattern = Pattern.GetStripePattern
                (RTF.Color.White, RTF.Color.Black);

            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 0)));
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 1)));
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 2)));
        }
        [Fact]
        public void StripePatternAlternateInX()
        {
            var pattern = Pattern.GetStripePattern
                (RTF.Color.White, RTF.Color.Black);

            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 0)));
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0.9, 0, 0)));
            Assert.Equal(RTF.Color.Black, pattern.At(pt.Point(1, 0, 0)));
            Assert.Equal(RTF.Color.Black, pattern.At(pt.Point(-0.1, 0, 0)));
            Assert.Equal(RTF.Color.Black, pattern.At(pt.Point(-1, 0, 0)));
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(-1.1, 0, 0)));
        }

        [Fact]
        public void LightingWithPatternApplied()
        {
            var m = new RTF.Material
            {
                Pattern = Pattern.GetStripePattern(RTF.Color.White, RTF.Color.Black),
                Ambient = 1,
                Diffuse = 0,
                Specular = 0
            };
            var eyev = pt.Vector(0, 0, -1);
            var normalv = pt.Vector(0, 0, -1);
            var light = new RTF.Light(pt.Point(0, 0, -10), RTF.Color.White);

            var c1 = light.Lighting(m, new shapes.Sphere(), pt.Point(0.9, 0, 0), eyev, normalv, false);
            var c2 = light.Lighting(m, new shapes.Sphere(), pt.Point(1.1, 0, 0), eyev, normalv, false);

            Assert.Equal(RTF.Color.White, c1);
            Assert.Equal(RTF.Color.Black, c2);
        }

        [Fact]
        public void StripesWithObjectTransformation()
        {
            var obj = new shapes.Sphere(tf.Scaling(2, 2, 2));
            var pattern = Pattern.GetStripePattern(RTF.Color.White, RTF.Color.Black);

            RTF.Color c = pattern.AtObject(obj, pt.Point(1.5, 0, 0));
            Assert.Equal(RTF.Color.White, c);
        }
        [Fact]
        public void StripesWithPatternTransformation()
        {
            var obj = new shapes.Sphere();
            var pattern = Pattern.GetStripePattern(RTF.Color.White, RTF.Color.Black);
            pattern.Transform = tf.Scaling(2, 2, 2);

            RTF.Color c = pattern.AtObject(obj, pt.Point(1.5, 0, 0));
            Assert.Equal(RTF.Color.White, c);
        }
        [Fact]
        public void StripesWithBothTransformation()
        {
            var obj = new shapes.Sphere(tf.Scaling(2, 2, 2));
            var pattern = Pattern.GetStripePattern(RTF.Color.White, RTF.Color.Black);
            pattern.Transform = tf.Translation(0.5, 0, 0);

            RTF.Color c = pattern.AtObject(obj, pt.Point(2.5, 0, 0));
            Assert.Equal(RTF.Color.White, c);
        }
        [Fact]
        public void TheDefaultPatternTransformation()
        {
            var pattern = new TestPattern();
            Assert.Equal(RTF.Matrix.GetIdentity(), pattern.Transform);
        }
        [Fact]
        public void AssigningTransformation()
        {
            var pattern = new TestPattern
            {
                Transform = tf.Translation(1, 2, 3)
            };
            Assert.Equal(tf.Translation(1, 2, 3), pattern.Transform);
        }

        [Fact]
        public void PatternWithTransformationObject()
        {
            var s = new shapes.Sphere(tf.Scaling(2, 2, 2));
            var pattern = new TestPattern();

            var c = pattern.AtObject(s, pt.Point(2, 3, 4));
            Assert.Equal(new RTF.Color(1, 1.5, 2), c);
        }
        [Fact]
        public void PatternWithTransformationPattern()
        {
            var s = new shapes.Sphere();
            var pattern = new TestPattern() { Transform = tf.Scaling(2, 2, 2) };

            var c = pattern.AtObject(s, pt.Point(2, 3, 4));
            Assert.Equal(new RTF.Color(1, 1.5, 2), c);
        }
        [Fact]
        public void PatternWithTransformationObjectAndPattern()
        {
            var s = new shapes.Sphere(tf.Scaling(2, 2, 2));
            var pattern = new TestPattern()
            { Transform = tf.Translation(0.5, 1, 1.5) };

            var c = pattern.AtObject(s, pt.Point(2.5, 3, 3.5));
            Assert.Equal(new RTF.Color(0.75, 0.5, 0.25), c);
        }

        [Fact]
        public void GradientLinearlyInterpolatesColors()
        {
            var pattern = new Gradient(RTF.Color.White, RTF.Color.Black);
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 0)));
            Assert.Equal(new RTF.Color(0.75, 0.75, 0.75), pattern.At(pt.Point(0.25, 0, 0)));
            Assert.Equal(new RTF.Color(0.5, 0.5, 0.5), pattern.At(pt.Point(0.5, 0, 0)));
            Assert.Equal(new RTF.Color(0.25, 0.25, 0.25), pattern.At(pt.Point(0.75, 0, 0)));
        }
        [Fact]
        public void RingExtendXandZ()
        {
            var pattern = new Ring(RTF.Color.White, RTF.Color.Black);
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 0)));
            Assert.Equal(RTF.Color.Black, pattern.At(pt.Point(1, 0, 0)));
            Assert.Equal(RTF.Color.Black, pattern.At(pt.Point(0, 0, 1)));
            Assert.Equal(RTF.Color.Black, pattern.At(pt.Point(0.708, 0, 0.708)));
        }

        [Fact]
        public void CheckerRepeatX()
        {
            var pattern = new Checker(RTF.Color.White, RTF.Color.Black);

            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 0)));
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0.99, 0, 0)));
            Assert.Equal(RTF.Color.Black, pattern.At(pt.Point(1.01, 0, 0)));
        }
        [Fact]
        public void CheckerRepeatY()
        {
            var pattern = new Checker(RTF.Color.White, RTF.Color.Black);

            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 0)));
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0.99, 0)));
            Assert.Equal(RTF.Color.Black, pattern.At(pt.Point(0, 1.01, 0)));
        }
        [Fact]
        public void CheckerRepeatZ()
        {
            var pattern = new Checker(RTF.Color.White, RTF.Color.Black);

            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 0)));
            Assert.Equal(RTF.Color.White, pattern.At(pt.Point(0, 0, 0.99)));
            Assert.Equal(RTF.Color.Black, pattern.At(pt.Point(0, 0, 1.01)));
        }

    }
}