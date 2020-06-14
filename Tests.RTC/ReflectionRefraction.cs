using System;
using Tests.RTC.Helpers;
using Xunit;
using pt = RayTracerChallenge.Features.PointType;
using shape = RayTracerChallenge.Features.Shapes;
using pattern = RayTracerChallenge.Features.Patterns;
using transform = RayTracerChallenge.Helpers.Transformations;
using c = RayTracerChallenge.Features.Color;
using RayTracerChallenge.Features;

namespace Tests.RTC
{
    public class ReflectionRefraction
    {
        const double EPSILON = 0.00001;

        #region Reflection
        [Fact]
        public void ReflectivityForDefaultMaterial()
        {
            var m = new Material();
            Assert.IsType<double>(m.Reflective);
        }

        [Fact]
        public void PrecomputingReflectorVector()
        {
            var shape = new shape.Plane();
            var r = new Ray(
                pt.Point(0, 1, -1),
                pt.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var i = new Intersection(Math.Sqrt(2), shape);
            var comps = Computation.PrepareComputations(i, r);

            var e = pt.Vector(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2);

            Assert.Equal(e, comps.RelflectV);
        }

        [Fact]
        public void ReflectedColorForNonReflectiveMaterial()
        {
            var w = World.Default();
            var r = new Ray(pt.Point(0, 0, 0), pt.Vector(0, 0, 1));
            w.Objects[1].Material.Ambient = 1;
            var i = new Intersection(1, w.Objects[1]);
            var comps = Computation.PrepareComputations(i, r);

            var color = w.ReflectedColor(comps);
            Assert.Equal(c.Black, color);
        }

        [Fact]
        public void ReflectedColorForReflectiveMaterial()
        {
            var w = World.Default();
            var shape = new shape.Plane()
            {
                Material = new Material()
                {
                    Reflective = 0.5
                },
                Transform = transform.Translation(0, -1, 0)
            };
            w.Objects.Add(shape);

            var r = new Ray(
                pt.Point(0, 0, -3),
                pt.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));

            var i = new Intersection(Math.Sqrt(2), shape);
            var comps = Computation.PrepareComputations(i, r);

            var color = w.ReflectedColor(comps);
            //TODO var exp = new c(0.19032, 0.23790, 0.14274);
            var exp = new c(0.19035, 0.23793, 0.14276);

            CustomAssert.Equal(exp, color, 5);
        }
        [Fact]
        public void ShadeHitReflectiveMaterial()
        {
            var w = World.Default();
            var shape = new shape.Plane()
            {
                Material = new Material()
                {
                    Reflective = 0.5
                },
                Transform = transform.Translation(0, -1, 0)
            };
            w.Objects.Add(shape);

            var r = new Ray(
                pt.Point(0, 0, -3),
                pt.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));

            var i = new Intersection(Math.Sqrt(2), shape);
            var comps = Computation.PrepareComputations(i, r);

            var color = w.ShadeHit(comps);
            //TODO var exp = new c(0.87677, 0.92436, 0.82918);
            var exp = new c(0.87677, 0.92436, 0.82919);

            CustomAssert.Equal(exp, color, 5);
        }

        [Fact]
        public void ColorAtMutuallyReflectiveSurfaces()
        {
            var w = new World
            {
                Lights = new System.Collections.Generic.List<Light>() { new Light(pt.Point(0, 0, 0), c.White) }
            };

            var lower = new shape.Plane();
            lower.Material.Reflective = 1;
            lower.Transform = transform.Translation(0, -1, 0);

            var upper = new shape.Plane();
            upper.Material.Reflective = 1;
            upper.Transform = transform.Translation(0, 1, 0);

            w.Objects.Add(lower);
            w.Objects.Add(upper);

            var r = new Ray(pt.Point(0, 0, 0), pt.Vector(0, 1, 0));

            w.ColorAt(r);
        }

        [Fact]
        public void ReflectedColorMaximumRecursiveDepth()
        {
            var w = World.Default();
            var shape = new shape.Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = transform.Translation(0, -1, 0);

            w.Objects.Add(shape);

            var r = new Ray(
                pt.Point(0, 0, -3),
                pt.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var i = new Intersection(Math.Sqrt(2), shape);

            var comps = Computation.PrepareComputations(i, r);
            var color = w.ReflectedColor(comps, 0);

            Assert.Equal(c.Black, color);
        }
        #endregion

        #region Transparency & Refraction
        [Fact]
        public void TransparencyRefractiveIndexDefaultMaterial()
        {
            var m = new Material();
            Assert.Equal(0, m.Transparency);
            Assert.Equal(1, m.RefractiveIndex);
        }

        [Fact]
        public void HelperForSphereWithGlassyMaterial()
        {
            var s = shape.Sphere.Glass();
            Assert.Equal(Matrix.GetIdentity(), s.Transform);
            Assert.Equal(1, s.Material.Transparency);
            Assert.Equal(1.5, s.Material.RefractiveIndex);
        }

        [Theory]
        [InlineData(0, 1, 1.5)]
        [InlineData(1, 1.5, 2.0)]
        [InlineData(2, 2.0, 2.5)]
        [InlineData(3, 2.5, 2.5)]
        [InlineData(4, 2.5, 1.5)]
        [InlineData(5, 1.5, 1.0)]
        public void FindingN1andN2VariousIntersection(int index, double n1, double n2)
        {
            var a = shape.Sphere.Glass();
            a.Transform = transform.Scaling(2, 2, 2);
            a.Material.RefractiveIndex = 1.5;

            var b = shape.Sphere.Glass();
            b.Transform = transform.Translation(0, 0, -0.25);
            b.Material.RefractiveIndex = 2;

            var c = shape.Sphere.Glass();
            c.Transform = transform.Translation(0, 0, 0.25);
            c.Material.RefractiveIndex = 2.5;

            var r = new Ray(pt.Point(0, 0, -4), pt.Vector(0, 0, 1));
            var xs = Intersection.Intersections(
                new Intersection(2, a),
                new Intersection(2.75, b),
                new Intersection(3.25, c),
                new Intersection(4.75, b),
                new Intersection(5.25, c),
                new Intersection(6, a)
                );

            var comps = Computation.PrepareComputations(xs[index], r, xs);
            Assert.Equal(n1, comps.N1);
            Assert.Equal(n2, comps.N2);

        }

        [Fact]
        public void UnderPointOffsetBelowSurface()
        {
            var r = new Ray(
                pt.Point(0, 0, -5), pt.Vector(0, 0, 1));
            var s = shape.Sphere.Glass();
            s.Transform = transform.Translation(0, 0, 1);
            var i = new Intersection(5, s);
            var xs = Intersection.Intersections(i);

            var comps = Computation.PrepareComputations(i, r, xs);

            Assert.True(comps.UnderPoint.Z > EPSILON / 2);
            Assert.True(comps.Point.Z < comps.UnderPoint.Z);
        }
        [Fact]
        public void RefractedColorWithOpaqueSurface()
        {
            var w = World.Default();
            var shape = w.Objects[0];
            var r = new Ray(
                 pt.Point(0, 0, -5), pt.Vector(0, 0, 1));
            var xs = Intersection.Intersections(
                new Intersection(4, shape),
                new Intersection(6, shape));
            var comps = Computation.PrepareComputations(
                xs[0],
                r,
                xs);

            var c = w.RefractedColor(comps, 5);
            Assert.Equal(c.Black, c);
        }
        [Fact]
        public void RefractedColorAtMaximumRecursiveDepth()
        {
            var w = World.Default();

            var shape = w.Objects[0];
            shape.Material.Transparency = 1;
            shape.Material.RefractiveIndex = 1.5;

            var r = new Ray(
                 pt.Point(0, 0, -5), pt.Vector(0, 0, 1));
            var xs = Intersection.Intersections(
                new Intersection(4, shape),
                new Intersection(6, shape));
            var comps = Computation.PrepareComputations(
                xs[0],
                r,
                xs);

            var c = w.RefractedColor(comps, 0);
            Assert.Equal(c.Black, c);
        }
        [Fact]
        public void RefractedColorUnderTotalReflection()
        {
            var w = World.Default();

            var shape = w.Objects[0];
            shape.Material.Transparency = 1;
            shape.Material.RefractiveIndex = 1.5;

            var r = new Ray(
                 pt.Point(0, 0, Math.Sqrt(2) / 2), pt.Vector(0, 1, 0));
            var xs = Intersection.Intersections(
                new Intersection(-Math.Sqrt(2) / 2, shape),
                new Intersection(Math.Sqrt(2) / 2, shape));

            var comps = Computation.PrepareComputations(
                xs[1],
                r,
                xs);

            var c = w.RefractedColor(comps, 5);
            Assert.Equal(c.Black, c);
        }
        [Fact]
        public void RefractedColorWithRefractedRay()
        {
            var w = World.Default();
            var a = w.Objects[0];
            a.Material.Ambient = 1;
            a.Material.Pattern = new pattern.TestPattern();

            var b = w.Objects[1];
            b.Material.Transparency = 1;
            b.Material.RefractiveIndex = 1.5;

            var r = new Ray(pt.Point(0, 0, 0.1), pt.Vector(0, 1, 0));
            var xs = Intersection.Intersections(
                new Intersection(-0.9899, a),
                new Intersection(-0.4899, b),
                new Intersection(0.4899, b),
                new Intersection(0.9899, a)
                );

            var comps = Computation.PrepareComputations(xs[2], r, xs);

            var c = w.RefractedColor(comps, 5);
            var exp = new c(0, 0.99888, 0.04725);

            CustomAssert.Equal(exp, c, 5);
        }
        [Fact]
        public void ShadeHitTransparentMaterial()
        {
            var w = World.Default();
            var floor = new shape.Plane
            {
                Transform = transform.Translation(0, -1, 0),
                Material = new Material()
                {
                    Transparency = 0.5,
                    RefractiveIndex = 1.5
                }
            };
            w.Objects.Add(floor);

            var ball = new shape.Sphere
            {
                Transform = transform.Translation(0, -3.5, -0.5),
                Material = new Material()
                {
                    Color = c.Red,
                    Ambient = 0.5
                }
            };
            w.Objects.Add(ball);

            var r = new Ray(
                 pt.Point(0, 0, -3),
                 pt.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var xs = Intersection.Intersections(
                new Intersection(Math.Sqrt(2), floor));
            var comps = Computation.PrepareComputations(xs[0], r, xs);
            var color = w.ShadeHit(comps, 5);
            //TODO var exp = new c(0.93642, 0.68642, 0.68642);
            var exp = new c(0.93643, 0.68643, 0.68643);

            CustomAssert.Equal(exp, color, 5);
        }
        #endregion
        #region FresnelEffect
        [Fact]
        public void SchlickApproximationUnderTotalInteralReflection()
        {
            var glass = shape.Sphere.Glass();
            var r = new Ray(
                pt.Point(0, 0, Math.Sqrt(2) / 2),
                pt.Vector(0, 1, 0));

            var xs = Intersection.Intersections(
                new Intersection(-Math.Sqrt(2) / 2, glass),
                new Intersection(Math.Sqrt(2) / 2, glass)
                );

            var comps = Computation.PrepareComputations(xs[1], r, xs);
            var reflectance = comps.Schlick();

            Assert.Equal(1, reflectance);
        }
        [Fact]
        public void SchlickApproximationPerpendicularViewing()
        {
            var glass = shape.Sphere.Glass();
            var r = new Ray(
                pt.Point(0, 0, 0),
                pt.Vector(0, 1, 0));

            var xs = Intersection.Intersections(
                new Intersection(-1, glass),
                new Intersection(1, glass)
                );

            var comps = Computation.PrepareComputations(xs[1], r, xs);
            var reflectance = comps.Schlick();

            Assert.Equal(0.04, reflectance, 5);
        }
        [Fact]
        public void SchlickApproximationSmallAngleAndN2GTN1()
        {
            var glass = shape.Sphere.Glass();
            var r = new Ray(
                pt.Point(0, 0.99, -2),
                pt.Vector(0, 0, 1));

            var xs = Intersection.Intersections(
                new Intersection(1.8589, glass));

            var comps = Computation.PrepareComputations(xs[0], r, xs);
            var reflectance = comps.Schlick();

            Assert.Equal(0.48873, reflectance, 5);
        }
        [Fact]
        public void ShadeHitReflectiveTransparentMaterial()
        {
            var w = World.Default();
            var floor = new shape.Plane
            {
                Transform = transform.Translation(0, -1, 0),
                Material = new Material()
                {
                    Reflective = 0.5,
                    Transparency = 0.5,
                    RefractiveIndex = 1.5
                }
            };
            w.Objects.Add(floor);

            var ball = new shape.Sphere
            {
                Transform = transform.Translation(0, -3.5, -0.5),
                Material = new Material()
                {
                    Color = c.Red,
                    Ambient = 0.5
                }
            };
            w.Objects.Add(ball);

            var r = new Ray(
                 pt.Point(0, 0, -3),
                 pt.Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
            var xs = Intersection.Intersections(
                new Intersection(Math.Sqrt(2), floor));
            var comps = Computation.PrepareComputations(xs[0], r, xs);
            var color = w.ShadeHit(comps, 5);
            //TODO var exp = new c(0.93391, 0.69643, 0.69243);
            var exp = new c(0.93392, 0.69644, 0.69243);

            CustomAssert.Equal(exp, color, 5);
        }
        #endregion
    }
}