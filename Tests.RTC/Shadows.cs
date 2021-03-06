﻿using RayTracerChallenge.Features.Shapes;
using System.Collections.Generic;
using Xunit;
using p = RayTracerChallenge.Features.PointType;
using RTF = RayTracerChallenge.Features;
using t = RayTracerChallenge.Helpers.Transformations;

namespace Tests.RTC
{
    public class Shadows
    {
        const double EPSILON = 0.0001;

        [Fact]
        public void LightingSurfaceShadow()
        {
            var m = new RTF.Material();
            var position = p.Point(0, 0, 0);

            var eyeV = p.Vector(0, 0, -1);
            var normalV = p.Vector(0, 0, -1);
            var light = new RTF.Light(p.Point(0, 0, -10), RTF.Color.White);
            var inShadow = true;


            var result = light.Lighting(m, new Sphere(), position, eyeV, normalV, inShadow);
            var expected = new RTF.Color(0.1, 0.1, 0.1);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void NoShadowNothingCollinearPointLight()
        {
            var w = RTF.World.Default();
            var pt = p.Point(0, 10, 0);

            Assert.False(RTF.Light.IsShadowed(w, pt));
        }
        [Fact]
        public void ShadowWhenObjectBetweenPointLight()
        {
            var w = RTF.World.Default();
            var pt = p.Point(10, -10, 10);

            Assert.True(RTF.Light.IsShadowed(w, pt));
        }
        [Fact]
        public void NoShadowWhenObjectBehindLight()
        {
            var w = RTF.World.Default();
            var pt = p.Point(-20, 20, -20);

            Assert.False(RTF.Light.IsShadowed(w, pt));
        }
        [Fact]
        public void NoShadowWhenObjectBehindPoint()
        {
            var w = RTF.World.Default();
            var pt = p.Point(-2, 2, -2);

            Assert.False(RTF.Light.IsShadowed(w, pt));
        }

        [Fact]
        public void ShadeHitIsGivenIntersectionInShadow()
        {
            var w = new RTF.World
            {
                Lights = new List<RTF.Light>() { new RTF.Light(p.Point(0, 0, -10), RTF.Color.White) }
            };

            var s1 = new Sphere();
            w.Objects.Add(s1);

            var s2 = new Sphere(t.Translation(0, 0, 10));
            w.Objects.Add(s2);

            var r = new RTF.Ray(p.Point(0, 0, 5), p.Vector(0, 0, 1));
            var i = new RTF.Intersection(4, s2);

            var comps = RTF.Computation.PrepareComputations(i, r);
            var c = w.ShadeHit(comps);

            var expected = new RTF.Color(0.1, 0.1, 0.1);
            Assert.Equal(expected, c);
        }
        [Fact]
        public void TheHitShouldOffsetThePoint()
        {
            var r = new RTF.Ray(p.Point(0, 0, -5), p.Vector(0, 0, 1));

            var shape = new Sphere(t.Translation(0, 0, 1));
            var i = new RTF.Intersection(5, shape);

            var comps = RTF.Computation.PrepareComputations(i, r);

            Assert.True(comps.OverPoint.Z < -EPSILON / 2);
            Assert.True(comps.Point.Z > comps.OverPoint.Z);
        }

    }
}
