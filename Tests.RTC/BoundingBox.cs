using Xunit;
using pt = RayTracerChallenge.Features.PointType;
using transform = RayTracerChallenge.Helpers.Transformations;
using shape = RayTracerChallenge.Features.Shapes;
using System;
using Tests.RTC.Helpers;
using RayTracerChallenge.Features;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;

namespace Tests.RTC
{
    public class BoundingBox
    {
        [Fact]
        public void CreatingEmptyBoundingBox()
        {
            var box = new shape.BoundingBox();
            var min = pt.Point(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
            var max = pt.Point(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

            Assert.Equal(min, box.Minimum);
            Assert.Equal(max, box.Maximum);
        }
        [Fact]
        public void CreatingBoundingBoxVolume()
        {
            var min = pt.Point(-1, -2, -3);
            var max = pt.Point(3, 2, 1);
            var box = new shape.BoundingBox(min, max);

            Assert.Equal(min, box.Minimum);
            Assert.Equal(max, box.Maximum);
        }
        [Fact]
        public void AddingPointsToEmptyBoundingBox()
        {
            var box = new shape.BoundingBox();
            var p1 = pt.Point(-5, 2, 0);
            var p2 = pt.Point(7, 0, -3);
            box.Add(p1);
            box.Add(p2);

            Assert.Equal(pt.Point(-5, 0, -3), box.Minimum);
            Assert.Equal(pt.Point(7, 2, 0), box.Maximum);
        }

        [Fact]
        public void SphereBoundingBox()
        {
            var s = new shape.Sphere();
            var box = s.Bounds();
            Assert.Equal(pt.Point(-1, -1, -1), box.Minimum);
            Assert.Equal(pt.Point(1, 1, 1), box.Maximum);
        }
        [Fact]
        public void PlaneBoundingBox()
        {
            var s = new shape.Plane();
            var box = s.Bounds();
            Assert.Equal(pt.Point(double.NegativeInfinity, 0, double.NegativeInfinity), box.Minimum);
            Assert.Equal(pt.Point(double.PositiveInfinity, 0, double.PositiveInfinity), box.Maximum);
        }
        [Fact]
        public void CubeBoundingBox()
        {
            var s = new shape.Cube();
            var box = s.Bounds();
            Assert.Equal(pt.Point(-1, -1, -1), box.Minimum);
            Assert.Equal(pt.Point(1, 1, 1), box.Maximum);
        }
        [Fact]
        public void UnboundedCylinderBoundingBox()
        {
            var s = new shape.Cylinder();
            var box = s.Bounds();
            Assert.Equal(pt.Point(-1, double.NegativeInfinity, -1), box.Minimum);
            Assert.Equal(pt.Point(1, double.PositiveInfinity, 1), box.Maximum);
        }
        [Fact]
        public void BoundedCylinderBoundingBox()
        {
            var s = new shape.Cylinder()
            {
                Minimum = -5,
                Maximum = 3
            };

            var box = s.Bounds();
            Assert.Equal(pt.Point(-1, -5, -1), box.Minimum);
            Assert.Equal(pt.Point(1, 3, 1), box.Maximum);
        }
        [Fact]
        public void UnboundedConeBoundingBox()
        {
            var s = new shape.Cone();
            var box = s.Bounds();
            Assert.Equal(pt.Point(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity), box.Minimum);
            Assert.Equal(pt.Point(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity), box.Maximum);
        }
        [Fact]
        public void BoundedConeBoundingBox()
        {
            var s = new shape.Cone()
            {
                Minimum = -5,
                Maximum = 3
            };

            var box = s.Bounds();
            Assert.Equal(pt.Point(-5, -5, -5), box.Minimum);
            Assert.Equal(pt.Point(5, 3, 5), box.Maximum);
        }
        [Fact]
        public void TestShapeBoundingBox()
        {
            var s = new shape.TestShape();
            var box = s.Bounds();
            Assert.Equal(pt.Point(-1, -1, -1), box.Minimum);
            Assert.Equal(pt.Point(1, 1, 1), box.Maximum);
        }
        [Fact]
        public void AddingBoxToAnother()
        {
            var box1 = new shape.BoundingBox(pt.Point(-5, -2, 0), pt.Point(7, 4, 4));
            var box2 = new shape.BoundingBox(pt.Point(8, -7, -2), pt.Point(14, 2, 8));
            box1.Add(box2);
            Assert.Equal(pt.Point(-5, -7, -2), box1.Minimum);
            Assert.Equal(pt.Point(14, 4, 8), box1.Maximum);
        }
        [Theory]
        [InlineData(5, -2, 0, true)]
        [InlineData(11, 4, 7, true)]
        [InlineData(8, 1, 3, true)]
        [InlineData(3, 0, 3, false)]
        [InlineData(8, -4, 3, false)]
        [InlineData(8, 1, -1, false)]
        [InlineData(13, 1, 3, false)]
        [InlineData(8, 5, 3, false)]
        [InlineData(8, 1, 8, false)]
        public void CheckingIfBoxContainsAPoint(double px, double py, double pz, bool result)
        {
            var point = pt.Point(px, py, pz);
            var box = new shape.BoundingBox(pt.Point(5, -2, 0), pt.Point(11, 4, 7));

            Assert.Equal(result, box.ContainsPoint(point));
        }
        [Theory]
        [InlineData(5, -2, 0, 11, 4, 7, true)]
        [InlineData(6, -1, 1, 10, 3, 6, true)]
        [InlineData(4, -3, -1, 10, 3, 6, false)]
        [InlineData(6, -1, 1, 12, 5, 8, false)]
        public void CheckingIfBoxContainsBox(double minx, double miny, double minz, double maxx, double maxy, double maxz, bool result)
        {
            var min = pt.Point(minx, miny, minz);
            var max = pt.Point(maxx, maxy, maxz);
            var box = new shape.BoundingBox(pt.Point(5, -2, 0), pt.Point(11, 4, 7));
            var box2 = new shape.BoundingBox(min, max);

            Assert.Equal(result, box.ContainsBox(box2));
        }

        [Fact]
        public void TransformingBoudingBox()
        {
            var box = new shape.BoundingBox(pt.Point(-1, -1, -1), pt.Point(1, 1, 1));
            var matrix = transform.RotationX(Math.PI / 4) * transform.RotationY(Math.PI / 4);
            var box2 = box.Transform(matrix);
            CustomAssert.Equal(pt.Point(-1.4142, -1.7071, -1.7071), box2.Minimum, 4);
            CustomAssert.Equal(pt.Point(1.4142, 1.7071, 1.7071), box2.Maximum, 4);
        }
        [Fact]
        public void QueryingShapesBoundingBoxInParentSpace()
        {
            var sphere = new shape.Sphere()
            {
                Transform = transform.Translation(1, -3, 5) * transform.Scaling(0.5, 2, 4)
            };
            var box = sphere.ParentSpaceBounds();
            Assert.Equal(pt.Point(0.5, -5, 1), box.Minimum);
            Assert.Equal(pt.Point(1.5, -1, 9), box.Maximum);
        }
        [Fact]
        public void GroupHasBoundingBoxContainsItsChildren()
        {
            var s = new shape.Sphere()
            {
                Transform = transform.Translation(2, 5, -3) * transform.Scaling(2, 2, 2)
            };
            var c = new shape.Cylinder()
            {
                Minimum = -2,
                Maximum = 2,
                Transform = transform.Translation(-4, -1, 4) * transform.Scaling(0.5, 1, 0.5)
            };
            var shape = new shape.Group();
            shape.Add(s);
            shape.Add(c);

            var box = shape.Bounds();

            Assert.Equal(pt.Point(-4.5, -3, -5), box.Minimum);
            Assert.Equal(pt.Point(4, 7, 4.5), box.Maximum);
        }

        [Theory]
        [InlineData(5, 0.5, 0, -1, 0, 0, true)]
        [InlineData(-5, 0.5, 0, 1, 0, 0, true)]
        [InlineData(0.5, 5, 0, 0, -1, 0, true)]
        [InlineData(0.5, -5, 0, 0, 1, 0, true)]
        [InlineData(0.5, 0, 5, 0, 0, -1, true)]
        [InlineData(0.5, 0, -5, 0, 0, 1, true)]
        [InlineData(0, 0.5, 0, 0, 0, 1, true)]
        [InlineData(-2, 0, 0, 2, 4, 6, false)]
        [InlineData(0, -2, 0, 6, 2, 4, false)]
        [InlineData(0, 0, -2, 4, 6, 2, false)]
        [InlineData(2, 0, 2, 0, 0, -1, false)]
        [InlineData(0, 2, 2, 0, -1, 0, false)]
        [InlineData(2, 2, 0, -1, 0, 0, false)]
        public void IntersectingRayWithBoundingBoxAtOrigin(double px, double py, double pz, double vx, double vy, double vz, bool result)
        {
            var origin = pt.Point(px, py, pz);
            var direction = pt.Vector(vx, vy, vz).Normalize();
            var box = new shape.BoundingBox(pt.Point(-1, -1, -1), pt.Vector(1, 1, 1));
            var r = new Ray(origin, direction);
            Assert.Equal(result, box.Intersects(r));
        }

        [Theory]
        [InlineData(15, 1, 2, -1, 0, 0, true)]
        [InlineData(-5, -1, 4, 1, 0, 0, true)]
        [InlineData(7, 6, 5, 0, -1, 0, true)]
        [InlineData(9, -5, 6, 0, 1, 0, true)]
        [InlineData(8, 2, 12, 0, 0, -1, true)]
        [InlineData(6, 0, -5, 0, 0, 1, true)]
        [InlineData(8, 1, 3.5, 0, 0, 1, true)]
        [InlineData(9, -1, -8, 2, 4, 6, false)]
        [InlineData(8, 3, -4, 6, 2, 4, false)]
        [InlineData(9, -1, -2, 4, 6, 2, false)]
        [InlineData(4, 0, 9, 0, 0, -1, false)]
        [InlineData(8, 6, -1, 0, -1, 0, false)]
        [InlineData(12, 5, 4, -1, 0, 0, false)]
        public void IntersectingRayWithNonCubicBoundingBox(double px, double py, double pz, double vx, double vy, double vz, bool result)
        {
            var origin = pt.Point(px, py, pz);
            var direction = pt.Vector(vx, vy, vz).Normalize();
            var box = new shape.BoundingBox(pt.Point(5, -2, 0), pt.Vector(11, 4, 7));
            var r = new Ray(origin, direction);
            Assert.Equal(result, box.Intersects(r));
        }
        [Fact]
        public void IntersectingRayGroupNotTestChildrenIfBoxMissed()
        {
            var child = new shape.TestShape();
            var shape = new shape.Group();
            shape.Add(child);
            var r = new Ray(pt.Point(0, 0, -5), pt.Vector(0, 1, 0));
            _ = shape.Intersect(r);
            Assert.Null(child.SavedRay);
        }
        [Fact]
        public void IntersectingRayGrouTestChildrenIfBoxHit()
        {
            var child = new shape.TestShape();
            var shape = new shape.Group();
            shape.Add(child);
            var r = new Ray(pt.Point(0, 0, -5), pt.Vector(0, 0, 1));
            _ = shape.Intersect(r);
            Assert.NotNull(child.SavedRay);
        }

        [Fact]
        public void SplittingAPerfectCube()
        {
            var box = new shape.BoundingBox(pt.Point(-1, -4, -5), pt.Point(9, 6, 5));
            (var left, var right) = box.Split();
            Assert.Equal(pt.Point(-1, -4, -5), left.Minimum);
            Assert.Equal(pt.Point(4, 6, 5), left.Maximum);
            Assert.Equal(pt.Point(4, -4, -5), right.Minimum);
            Assert.Equal(pt.Point(9, 6, 5), right.Maximum);
        }
        [Fact]
        public void SplittingXWideBox()
        {
            var box = new shape.BoundingBox(pt.Point(-1, -2, -3), pt.Point(9, 5.5, 3));
            (var left, var right) = box.Split();
            Assert.Equal(pt.Point(-1, -2, -3), left.Minimum);
            Assert.Equal(pt.Point(4, 5.5, 3), left.Maximum);
            Assert.Equal(pt.Point(4, -2, -3), right.Minimum);
            Assert.Equal(pt.Point(9, 5.5, 3), right.Maximum);
        }
        [Fact]
        public void SplittingYWideBox()
        {
            var box = new shape.BoundingBox(pt.Point(-1, -2, -3), pt.Point(5, 8, 3));
            (var left, var right) = box.Split();
            Assert.Equal(pt.Point(-1, -2, -3), left.Minimum);
            Assert.Equal(pt.Point(5, 3, 3), left.Maximum);
            Assert.Equal(pt.Point(-1, 3, -3), right.Minimum);
            Assert.Equal(pt.Point(5, 8, 3), right.Maximum);
        }
        [Fact]
        public void SplittingZWideBox()
        {
            var box = new shape.BoundingBox(pt.Point(-1, -2, -3), pt.Point(5, 3, 7));
            (var left, var right) = box.Split();
            Assert.Equal(pt.Point(-1, -2, -3), left.Minimum);
            Assert.Equal(pt.Point(5, 3, 2), left.Maximum);
            Assert.Equal(pt.Point(-1, -2, 2), right.Minimum);
            Assert.Equal(pt.Point(5, 3, 7), right.Maximum);
        }
        [Fact]
        public void PartitioningGroupChildren()
        {
            var s1 = new shape.Sphere(transform.Translation(-2, 0, 0));
            var s2 = new shape.Sphere(transform.Translation(2, 0, 0));
            var s3 = new shape.Sphere();
            var g = new shape.Group();
            g.Add(new List<shape.Shape>() { s1, s2, s3 });
            (List<shape.Shape> left, List<shape.Shape> right) = g.Partition();

            Assert.Equal(1, g.Count());
            Assert.Single(left);
            Assert.Equal(s1, left[0]);
            Assert.Single(right);
            Assert.Equal(s2, right[0]);
        }
        [Fact]
        public void SubGroupFromListChildren()
        {
            var s1 = new shape.Sphere();
            var s2 = new shape.Sphere();
            var g = new shape.Group();
            var list = new List<shape.Shape>() { s1, s2 };
            g.Subgroup(list);

            Assert.Single(g.Children);
            Assert.IsType<shape.Group>(g.Children[0]);
            Assert.Equal(list, (g.Children[0] as shape.Group).Children);
        }

        [Fact]
        public void SubdividingGroupPartitionsItsChildren()
        {
            var s1 = new shape.Sphere(transform.Translation(-2, -2, 0));
            var s2 = new shape.Sphere(transform.Translation(-2, 2, 0));
            var s3 = new shape.Sphere(transform.Scaling(4, 4, 4));
            var g = new shape.Group(new List<shape.Shape>() { s1, s2, s3 });
            g.Divide(1);

            Assert.Equal(s3, g[0]);
            var subgroup = g[1];
            Assert.IsType<shape.Group>(subgroup);
            
            var sg = subgroup as shape.Group;
            Assert.Equal(2, sg.Count());

            Assert.IsType<shape.Group>(sg[0]);
            Assert.Single((sg[0] as shape.Group).Children);
            Assert.Equal(s1, (sg[0] as shape.Group)[0]);

            Assert.IsType<shape.Group>(sg[1]);
            Assert.Single((sg[1] as shape.Group).Children);
            Assert.Equal(s2, (sg[1] as shape.Group)[0]);
        }
        [Fact]
        public void SubdividingGroupWithTooFewChildren()
        {
            var s1 = new shape.Sphere(transform.Translation(-2, 0, 0));
            var s2 = new shape.Sphere(transform.Translation(2, 1, 0));
            var s3 = new shape.Sphere(transform.Translation(2, -1, 4));
            var subgroup = new shape.Group(new List<shape.Shape>() { s1, s2, s3 });
            var s4 = new shape.Sphere();
            var g = new shape.Group(new List<shape.Shape>() { subgroup, s4 });

            g.Divide(3);

            Assert.Equal(subgroup, g[0]);
            Assert.Equal(s4, g[1]);
            var sg = (g[0] as shape.Group);
            Assert.Equal(2, sg.Count());

            Assert.IsType<shape.Group>(sg[0]);
            Assert.Single((sg[0] as shape.Group).Children);
            Assert.Equal(s1, (sg[0] as shape.Group)[0]);

            Assert.IsType<shape.Group>(sg[1]);
            Assert.Equal(2, (sg[1] as shape.Group).Count());
            Assert.Equal(new List<shape.Shape>() { s2,s3 }, (sg[1] as shape.Group).Children);
        }
    }
}