using System;
using Tests.RTC.Helpers;
using Xunit;
using RayTracerChallenge.Helpers;
using pt = RayTracerChallenge.Features.PointType;
using shape = RayTracerChallenge.Features.Shapes;
using pattern = RayTracerChallenge.Features.Patterns;
using transform = RayTracerChallenge.Helpers.Transformations;
using c = RayTracerChallenge.Features.Color;
using RayTracerChallenge.Features;
using System.Linq;
using System.Diagnostics;

namespace Tests.RTC
{
    public class Triangle
    {
        #region TriangleShape
        [Fact]
        public void ConstructionTriangle()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var t = new shape.Triangle(p1, p2, p3);

            Assert.Equal(p1, t.P1);
            Assert.Equal(p2, t.P2);
            Assert.Equal(p3, t.P3);

            Assert.Equal(pt.Vector(-1, -1, 0), t.Edge1);
            Assert.Equal(pt.Vector(1, -1, 0), t.Edge2);
            Assert.Equal(pt.Vector(0, 0, -1), t.Normal);
        }
        [Fact]
        public void FindingNormalOnTriangle()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var t = new shape.Triangle(p1, p2, p3);

            var n1 = t.TestNormalLocal(pt.Point(0, 0.5, 0));
            var n2 = t.TestNormalLocal(pt.Point(-0.5, 0.75, 0));
            var n3 = t.TestNormalLocal(pt.Point(0.5, 0.25, 0));

            Assert.Equal(n1, t.Normal);
            Assert.Equal(n2, t.Normal);
            Assert.Equal(n3, t.Normal);
        }
        [Fact]
        public void IntersectingRayParallelToTriangle()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var t = new shape.Triangle(p1, p2, p3);
            var r = new Ray(pt.Point(0, -1, -2), pt.Vector(0, 1, 0));
            var xs = t.Intersect(r);
            Assert.Empty(xs);
        }
        [Fact]
        public void RayMissesP1P3Edge()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var t = new shape.Triangle(p1, p2, p3);
            var r = new Ray(pt.Point(1, 1, -2), pt.Vector(0, 0, 1));
            var xs = t.Intersect(r);
            Assert.Empty(xs);
        }
        [Fact]
        public void RayMissesP1P2Edge()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var t = new shape.Triangle(p1, p2, p3);
            var r = new Ray(pt.Point(-1, 1, -2), pt.Vector(0, 0, 1));
            var xs = t.Intersect(r);
            Assert.Empty(xs);
        }
        [Fact]
        public void RayMissesP2P3Edge()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var t = new shape.Triangle(p1, p2, p3);
            var r = new Ray(pt.Point(0, -1, -2), pt.Vector(0, 0, 1));
            var xs = t.Intersect(r);
            Assert.Empty(xs);
        }
        [Fact]
        public void RayStrikesTriangle()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var t = new shape.Triangle(p1, p2, p3);
            var r = new Ray(pt.Point(0, 0.5, -2), pt.Vector(0, 0, 1));
            var xs = t.Intersect(r);
            Assert.Single(xs);
            Assert.Equal(2, xs[0].T);
        }
        #endregion

        #region Wavefront objects
        [Fact]
        public void IgnoringUnrecognizedLines()
        {
            var gibberish = new string[]
            {
                "There was a young lady named bright",
                "Whose speed was much faster than light.",
                "She set out one day",
                "In a relative way",
                "And returned the previous night."
            };
            var parser = new FileParser();
            parser.Parse(gibberish);
            Assert.Equal(5, parser.IgnoredLines);
        }
        [Fact]
        public void VertexRecords()
        {
            var file = new string[]
            {
                "v -1 1 0",
                "v -1.0000 0.5000 0.0000",
                "v 1 0 0",
                "v 1 1 0"
            };
            var parser = new FileParser();
            parser.Parse(file);
            Assert.Equal(pt.Point(-1, 1, 0), parser.Vertices[0]);
            Assert.Equal(pt.Point(-1, 0.5, 0), parser.Vertices[1]);
            Assert.Equal(pt.Point(1, 0, 0), parser.Vertices[2]);
            Assert.Equal(pt.Point(1, 1, 0), parser.Vertices[3]);
        }
        [Fact]
        public void ParsingTriangleFaces()
        {
            var file = new string[]
            {
                "v -1 1 0",
                "v -1 0 0",
                "v 1 0 0",
                "v 1 1 0",
                "f 1 2 3",
                "f 1 3 4"
            };
            var parser = new FileParser();
            parser.Parse(file);
            var g = parser.DefaultGroup;
            var t1 = g[0] as shape.Triangle;
            var t2 = g[1] as shape.Triangle;
            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Vertices[0], t2.P1);
            Assert.Equal(parser.Vertices[2], t2.P2);
            Assert.Equal(parser.Vertices[3], t2.P3);
        }
        [Fact]
        public void TriangulatingPolygones()
        {
            var file = new string[]
            {
                "v -1 1 0",
                "v -1 0 0",
                "v 1 0 0",
                "v 1 1 0",
                "v 0 2 0",
                "f 1 2 3 4 5"
            };
            var parser = new FileParser();
            parser.Parse(file);
            var g = parser.DefaultGroup;
            var t1 = g[0] as shape.Triangle;
            var t2 = g[1] as shape.Triangle;
            var t3 = g[2] as shape.Triangle;
            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Vertices[0], t2.P1);
            Assert.Equal(parser.Vertices[2], t2.P2);
            Assert.Equal(parser.Vertices[3], t2.P3);
            Assert.Equal(parser.Vertices[0], t3.P1);
            Assert.Equal(parser.Vertices[3], t3.P2);
            Assert.Equal(parser.Vertices[4], t3.P3);
        }
        [Fact]
        public void TrianglesInGroups()
        {
            var file = new string[]
            {
                "v -1 1 0",
                "v -1 0 0",
                "v 1 0 0",
                "v 1 1 0",
                "g FirstGroup",
                "f 1 2 3",
                "g SecondGroup",
                "f 1 3 4"
            };
            var parser = new FileParser();
            parser.Parse(file);
            var g1 = parser.Groups["FirstGroup"];
            var g2 = parser.Groups["SecondGroup"];
            var t1 = g1[0] as shape.Triangle;
            var t2 = g2[0] as shape.Triangle;
            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Vertices[0], t2.P1);
            Assert.Equal(parser.Vertices[2], t2.P2);
            Assert.Equal(parser.Vertices[3], t2.P3);
        }
        [Fact]
        public void ConvertingOBJFileToGroup()
        {
            var file = new string[]
            {
                "v -1 1 0",
                "v -1 0 0",
                "v 1 0 0",
                "v 1 1 0",
                "g FirstGroup",
                "f 1 2 3",
                "g SecondGroup",
                "f 1 3 4"
            };

            var parser = new FileParser();
            parser.Parse(file);
            var g1 = parser.Groups["FirstGroup"];
            var g2 = parser.Groups["SecondGroup"];
            var g = parser.ObjToGroup();
            Assert.True(g.Contains(g1));
            Assert.True(g.Contains(g2));
        }
        #endregion
        #region Smooth Triangles
        [Fact]
        public void ConstructionSmoothTriangle()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var v1 = pt.Vector(0, 1, 0);
            var v2 = pt.Vector(-1, 0, 0);
            var v3 = pt.Vector(1, 0, 0);
            var tri = shape.Triangle.Smooth(p1, p2, p3, v1, v2, v3);
            Assert.Equal(p1, tri.P1);
            Assert.Equal(p2, tri.P2);
            Assert.Equal(p3, tri.P3);
            Assert.Equal(v1, tri.N1);
            Assert.Equal(v2, tri.N2);
            Assert.Equal(v3, tri.N3);
        }
        [Fact]
        public void IntersectionCanEncapsulateUV()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var s = new shape.Triangle(p1, p2, p3);
            var i = Intersection.WithUV(3.5, s, 0.2, 0.4);
            Assert.Equal(0.2, i.U);
            Assert.Equal(0.4, i.V);
        }
        [Fact]
        public void IntersectionWithSmoothTriangleStoreUV()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var v1 = pt.Vector(0, 1, 0);
            var v2 = pt.Vector(-1, 0, 0);
            var v3 = pt.Vector(1, 0, 0);
            var tri = shape.Triangle.Smooth(p1, p2, p3, v1, v2, v3);

            var r = new Ray(pt.Point(-0.2, 0.3, -2), pt.Vector(0, 0, 1));
            var xs = tri.Intersect(r);
            Assert.Equal(0.45, xs[0].U, 5);
            Assert.Equal(0.25, xs[0].V, 5);
        }
        [Fact]
        public void SmoothTriangleUsesUVNormal()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var v1 = pt.Vector(0, 1, 0);
            var v2 = pt.Vector(-1, 0, 0);
            var v3 = pt.Vector(1, 0, 0);

            var tri = shape.Triangle.Smooth(p1, p2, p3, v1, v2, v3);
            var i = Intersection.WithUV(1, tri, 0.45, 0.25);
            var n = tri.NormalAt(pt.Point(0, 0, 0), i);

            CustomAssert.Equal(pt.Vector(-0.5547, 0.83205, 0), n, 5);
        }
        [Fact]
        public void PreparingTheNormalSmoothTriangle()
        {
            var p1 = pt.Point(0, 1, 0);
            var p2 = pt.Point(-1, 0, 0);
            var p3 = pt.Point(1, 0, 0);
            var v1 = pt.Vector(0, 1, 0);
            var v2 = pt.Vector(-1, 0, 0);
            var v3 = pt.Vector(1, 0, 0);

            var tri = shape.Triangle.Smooth(p1, p2, p3, v1, v2, v3);
            var i = Intersection.WithUV(1, tri, 0.45, 0.25);
            var r = new Ray(pt.Point(-0.2, 0.3, -2), pt.Vector(0, 0, 1));
            var xs = Intersection.Intersections(i);
            var comps = Computation.PrepareComputations(i, r, xs);

            CustomAssert.Equal(pt.Vector(-0.5547, 0.83205, 0), comps.NormalV, 5);
        }

        [Fact]
        public void VertextNormalRecords()
        {
            var file = new string[]
            {
                "vn 0 0 1",
                "vn 0.707 0 -0.707",
                "vn 1 2 3"
            };
            var parser = new FileParser();
            parser.Parse(file);
            Assert.Equal(pt.Vector(0, 0, 1), parser.Normals[0]);
            Assert.Equal(pt.Vector(0.707,0,-0.707), parser.Normals[1]);
            Assert.Equal(pt.Vector(1,2,3), parser.Normals[2]);
        }
        [Fact]
        public void FacesWithNormals()
        {
            var file = new string[]
            {
                "v 0 1 0",
                "v -1 0 0",
                "v 1 0 0",

                "vn -1 0 0",
                "vn 1 0 0",
                "vn 0 1 0",
                "f 1//3 2//1 3//2",
                "f 1/0/3 2/102/1 3/14/2"
            };
            var parser = new FileParser();
            parser.Parse(file);
            var g = parser.DefaultGroup;
            var t1 = g[0] as shape.Triangle;
            var t2 = g[1] as shape.Triangle;
            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Normals[2], t1.N1);
            Assert.Equal(parser.Normals[0], t1.N2);
            Assert.Equal(parser.Normals[1], t1.N3);
            Assert.Equal(t1, t2);
        }
        #endregion    
    }
}
