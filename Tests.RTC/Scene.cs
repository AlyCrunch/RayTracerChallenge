using System;
using RTF = RayTracerChallenge.Features;
using RTH = RayTracerChallenge.Helpers;
using Tests.RTC.Helpers;
using Xunit;

namespace Tests.RTC
{
    public class Scene
    {
        [Fact]
        public void CreatingWorld()
        {
            var w = new RTF.World();
            Assert.Empty(w.Objects);
            Assert.Null(w.Light);
        }

        [Fact]
        public void TheDefaultWorld()
        {
            var light = new RTH.Light(
                RTF.PointType.Point(-10, 10, -10),
                RTF.Color.White());

            var s1 = new RTF.Sphere(
                        new RTF.Material(
                            new RTF.Color(0.8, 1, 0.6),
                            0.7,
                            0.2
                        )
                    );
            var s2 = new RTF.Sphere(
                    RTH.Transformations.Scaling(0.5, 0.5, 0.5)
                );

            RTF.World w = RTF.World.Default();

            Assert.Equal(light, w.Light);
            Assert.Contains(s1, w.Objects);
            Assert.Contains(s2, w.Objects);
        }

        [Fact]
        public void IntersectWorldWithRay()
        {
            var w = RTF.World.Default();
            var r = new RTF.Ray(
                RTF.PointType.Point(0, 0, -5),
                RTF.PointType.Vector(0, 0, 1));
            RTF.Intersection[] xs = RTF.Intersection.Intersect(w, r);

            Assert.Equal(4, xs.Length);
            Assert.Equal(4, xs[0].T);
            Assert.Equal(4.5, xs[1].T);
            Assert.Equal(5.5, xs[2].T);
            Assert.Equal(6, xs[3].T);
        }

        [Fact]
        public void PrecomputingStateIntersection()
        {
            var r = new RTF.Ray(
                   RTF.PointType.Point(0, 0, -5),
                   RTF.PointType.Vector(0, 0, 1));
            var shape = new RTF.Sphere();
            var i = new RTF.Intersection(4, shape);
            var comps = RTF.Computation.PrepareComputation(i, r);

            Assert.Equal(i.T, comps.T);
            Assert.Equal(i.Object, comps.Object);
            Assert.Equal(RTF.PointType.Point(0, 0, -1), comps.Point);
            Assert.Equal(RTF.PointType.Vector(0, 0, -1), comps.EyeV);
            Assert.Equal(RTF.PointType.Vector(0, 0, -1), comps.NormalV);
        }

        [Fact]
        public void HitWhenIntersectionOccursOutside()
        {
            var r = new RTF.Ray(
                   RTF.PointType.Point(0, 0, -5),
                   RTF.PointType.Vector(0, 0, 1));
            var shape = new RTF.Sphere();
            var i = new RTF.Intersection(4, shape);
            var comps = RTF.Computation.PrepareComputation(i, r);

            Assert.False(comps.Inside);
        }

        [Fact]
        public void HitWhenIntersectionOccursInside()
        {
            var r = new RTF.Ray(
                   RTF.PointType.Point(0, 0, 0),
                   RTF.PointType.Vector(0, 0, 1));
            var shape = new RTF.Sphere();
            var i = new RTF.Intersection(1, shape);
            var comps = RTF.Computation.PrepareComputation(i, r);


            Assert.Equal(RTF.PointType.Point(0, 0, 1), comps.Point);
            Assert.Equal(RTF.PointType.Vector(0, 0, -1), comps.EyeV);
            Assert.True(comps.Inside);
            Assert.Equal(RTF.PointType.Vector(0, 0, -1), comps.NormalV);
        }

        [Fact]
        public void ShadingIntersection()
        {
            var w = RTF.World.Default();
            var r = new RTF.Ray(
                      RTF.PointType.Point(0, 0, -5),
                      RTF.PointType.Vector(0, 0, 1));
            var shape = w.Objects[0];
            var i = new RTF.Intersection(4, shape);

            var comps = RTF.Computation.PrepareComputation(i, r);
            var c = w.ShadeHit(comps);
            var exp = new RTF.Color(0.38066, 0.47583, 0.2855);
            //var exp = new RTF.Color(0.50066, 0.57583, 0.42550);

            CustomAssert.Equal(exp, c, 5);
        }
        [Fact]
        public void ShadingIntersectionFromInside()
        {
            var w = RTF.World.Default();
            w.Light = new RTH.Light(
                      RTF.PointType.Point(0, 0.25, 0),
                      RTF.Color.White());
            var r = new RTF.Ray(
                      RTF.PointType.Point(0, 0, 0),
                      RTF.PointType.Vector(0, 0, 1));
            var shape = w.Objects[1];

            var i = new RTF.Intersection(0.5, shape);
            var comps = RTF.Computation.PrepareComputation(i, r);
            var c = w.ShadeHit(comps);
            var exp = new RTF.Color(0.90498, 0.90498, 0.90498);

            CustomAssert.Equal(exp, c, 5);
        }

        [Fact]
        public void ColorRayMissed()
        {
            var w = RTF.World.Default();
            var r = new RTF.Ray(
                      RTF.PointType.Point(0, 0, -5),
                      RTF.PointType.Vector(0, 1, 0));
            var c = w.ColorAt(r);
            var exp = RTF.Color.Black();

            CustomAssert.Equal(exp, c, 5);
        }

        [Fact]
        public void ColorRayHits()
        {
            var w = RTF.World.Default();
            var r = new RTF.Ray(
                      RTF.PointType.Point(0, 0, -5),
                      RTF.PointType.Vector(0, 0, 1));
            var c = w.ColorAt(r);
            var exp = new RTF.Color(0.38066, 0.47583, 0.2855);

            CustomAssert.Equal(exp, c, 5);
        }

        [Fact]
        public void ColorWithIntersecBehindRay()
        {
            var w = RTF.World.Default();
            RTF.Sphere outer = (RTF.Sphere)w.Objects[0];
            outer.Material.Ambient = 1;
            RTF.Sphere inner = (RTF.Sphere)w.Objects[1];
            inner.Material.Ambient = 1;
            var r = new RTF.Ray(
                      RTF.PointType.Point(0, 0, 0.75),
                      RTF.PointType.Vector(0, 0, -1));
            var c = w.ColorAt(r);
            Assert.Equal(inner.Material.Color, c);
        }

        [Fact]
        public void TransformationMatrixForDefaultOrientation()
        {
            var from = RTF.PointType.Point(0, 0, 0);
            var to = RTF.PointType.Point(0, 0, -1);
            var up = RTF.PointType.Vector(0, 1, 0);
            RTF.Matrix t = RTH.Transformations.ViewTransform(from, to, up);
            var e = RTF.Matrix.GetIdentity();

            Assert.Equal(e, t);
        }
        [Fact]
        public void ViewTransformationMatrixPosZDir()
        {
            var from = RTF.PointType.Point(0, 0, 0);
            var to = RTF.PointType.Point(0, 0, 1);
            var up = RTF.PointType.Vector(0, 1, 0);
            RTF.Matrix t = RTH.Transformations.ViewTransform(from, to, up);
            var e = RTH.Transformations.Scaling(-1, 1, -1);

            Assert.Equal(e, t);
        }
        [Fact]
        public void ViewTransformationMatrixMovesWorld()
        {
            var from = RTF.PointType.Point(0, 0, 8);
            var to = RTF.PointType.Point(0, 0, 0);
            var up = RTF.PointType.Vector(0, 1, 0);
            RTF.Matrix t = RTH.Transformations.ViewTransform(from, to, up);
            var e = RTH.Transformations.Translation(0, 0, -8);

            Assert.Equal(e, t);
        }
        [Fact]
        public void ArbitraryTransformation()
        {
            var from = RTF.PointType.Point(1, 3, 2);
            var to = RTF.PointType.Point(4, -2, 8);
            var up = RTF.PointType.Vector(1, 1, 0);
            RTF.Matrix t = RTH.Transformations.ViewTransform(from, to, up);
            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new double[] { -0.50709, 0.50709, 0.67612, -2.36643 });
            e.SetRow(1, new double[] { 0.76772, 0.60609, 0.12122, -2.82843 });
            e.SetRow(2, new double[] { -0.35857, 0.59761, -0.71714, 0 });
            e.SetRow(3, new double[] { 0, 0, 0, 1 });

            CustomAssert.Equal(e, t, 5);
        }

        [Fact]
        public void ConstructionCamera()
        {
            var hsize = 160;
            var vsize = 120;
            var field_of_view = Math.PI / 2;

            RTF.Camera c = new RTF.Camera(hsize, vsize, field_of_view);

            Assert.Equal(hsize, c.HorizontalSize);
            Assert.Equal(vsize, c.VerticalSize);
            Assert.Equal(field_of_view, c.FieldOfView);
            Assert.Equal(RTF.Matrix.GetIdentity(), c.Transform);
        }
        [Fact]
        public void PixelSizeHorizontalCanvas()
        {
            RTF.Camera c = new RTF.Camera(200, 125, Math.PI / 2);

            Assert.Equal(0.01, c.PixelSize, 2);
        }
        [Fact]
        public void PixelSizeVerticalCanvas()
        {
            RTF.Camera c = new RTF.Camera(125, 200, Math.PI / 2);

            Assert.Equal(0.01, c.PixelSize, 2);
        }

        [Fact]
        public void ConstructingRayThroughCenterCanvas()
        {
            var c = new RTF.Camera(201, 101, Math.PI / 2);
            var r = c.RayForPixel(100, 50);

            Assert.Equal(RTF.PointType.Point(0, 0, 0), r.Origin);
            CustomAssert.Equal(RTF.PointType.Vector(0, 0, -1), r.Direction, 5);
        }
        [Fact]
        public void ConstructingRayThroughCornerCanvas()
        {
            var c = new RTF.Camera(201, 101, Math.PI / 2);
            var r = c.RayForPixel(0, 0);

            Assert.Equal(RTF.PointType.Point(0, 0, 0), r.Origin);
            CustomAssert.Equal(
                RTF.PointType.Vector(0.66519, 0.33259, -0.66851),
                r.Direction,
                5);
        }
        [Fact]
        public void ConstructingRayWhenCameraTransformed()
        {
            var c = new RTF.Camera(201, 101, Math.PI / 2)
            {
                Transform = RTH.Transformations.RotationY(Math.PI / 4)
                * RTH.Transformations.Translation(0, -2, 5)
            };
            var r = c.RayForPixel(100, 50);

            Assert.Equal(RTF.PointType.Point(0, 2, -5), r.Origin);
            CustomAssert.Equal(
                RTF.PointType.Vector(Math.Sqrt(2) / 2, 0, -Math.Sqrt(2) / 2),
                r.Direction,
                5);
        }

        [Fact]
        public void RenderingWorldWithCamera()
        {
            var w = RTF.World.Default();
            var c = new RTF.Camera(11, 11, Math.PI / 2);

            var from = RTF.PointType.Point(0, 0, -5);
            var to = RTF.PointType.Point(0, 0, 0);
            var up = RTF.PointType.Vector(0, 1, 0);

            c.Transform = RTH.Transformations.ViewTransform(from, to, up);

            RTF.Canvas image = RTF.Canvas.Render(c, w);

            var exp = new RTF.Color(0.38066, 0.47583, 0.2855);

            CustomAssert.Equal(exp, image.PixelAt(5, 5), 5);

        }
    }
}