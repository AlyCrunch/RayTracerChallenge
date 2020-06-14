using RTF = RayTracerChallenge.Features;
using pt = RayTracerChallenge.Features.PointType;
using patterns = RayTracerChallenge.Features.Patterns;
using Xunit;
using System.Collections.Generic;

namespace Tests.RTC
{
    public class TextureMapping
    {
        [Theory]
        [InlineData(0, 0, true)]
        [InlineData(0.5, 0, false)]
        [InlineData(0, 0.5, false)]
        [InlineData(0.5, 0.5, true)]
        [InlineData(1, 1, true)]
        public void CheckerPattern2D(double u, double v, bool expected)
        {
            var black = RTF.Color.Black;
            var checker = new patterns.UV.Checker(2, 2, black, RTF.Color.White);
            var color = checker.At(u, v);

            Assert.Equal(color == black, expected);
        }

        [Theory]
        [InlineData(0, 0, -1, 0.0, 0.5)]
        [InlineData(1, 0, 0, 0.25, 0.5)]
        [InlineData(0, 0, 1, 0.5, 0.5)]
        [InlineData(-1, 0, 0, 0.75, 0.5)]
        [InlineData(0, 1, 0, 0.5, 1.0)]
        [InlineData(0, -1, 0, 0.5, 0.0)]
        [InlineData(0.70710678118, 0.70710678118, 0, 0.25, 0.75)]
        public void SphericalMapping3DPoint(double x, double y, double z, double u, double v)
        {
            var point = pt.Point(x, y, z);
            (var newU, var newV) = patterns.UV.Pattern.SphericalMap(point);

            Assert.Equal(u, newU);
            Assert.Equal(v, newV);
        }

        [Theory]
        [InlineData(0.4315, 0.4670, 0.7719, false)]
        [InlineData(-0.9654, 0.2552, -0.0534, true)]
        [InlineData(0.1039, 0.7090, 0.6975, false)]
        [InlineData(-0.4986, -0.7856, -0.3663, true)]
        [InlineData(-0.0317, -0.9395, 0.3411, true)]
        [InlineData(0.4809, -0.7721, 0.4154, true)]
        [InlineData(0.0285, -0.9612, -0.2745, true)]
        [InlineData(-0.5734, -0.2162, -0.7903, false)]
        [InlineData(0.7688, -0.1470, 0.6223, true)]
        [InlineData(-0.7652, 0.2175, 0.6060, true)]
        public void UsingTextureMapWithSperical(double x, double y, double z, bool expected)
        {
            var point = pt.Point(x, y, z);
            var black = RTF.Color.Black;
            var checker = new patterns.UV.Checker(16, 8, black, RTF.Color.White);
            var pattern = new patterns.TextureMap(patterns.UV.Pattern.SphericalMap, checker);

            var color = pattern.At(point);

            Assert.Equal(color == black, expected);
        }

        [Theory]
        [InlineData(0.25, 0, 0.5, 0.25, 0.5)]
        [InlineData(0.25, 0, -0.25, 0.25, 0.75)]
        [InlineData(0.25, 0.5, -0.25, 0.25, 0.75)]
        [InlineData(1.25, 0, 0.5, 0.25, 0.5)]
        [InlineData(0.25, 0, -1.75, 0.25, 0.25)]
        [InlineData(1, 0, -1, 0.0, 0.0)]
        [InlineData(0, 0, 0, 0.0, 0.0)]
        public void UsingPlanarMapping3Dpoint(double x, double y, double z, double uExp, double vExp)
        {
            var p = pt.Point(x, y, z);
            (var u, var v) = patterns.UV.Pattern.PlanarMap(p);

            Assert.Equal(uExp, u);
            Assert.Equal(vExp, v);
        }

        [Theory]
        [InlineData(0, 0, -1, 0.0, 0.0)]
        [InlineData(0, 0.5, -1, 0.0, 0.5)]
        [InlineData(0, 1, -1, 0.0, 0.0)]
        [InlineData(0.70711, 0.5, -0.70711, 0.125, 0.5)]
        [InlineData(1, 0.5, 0, 0.25, 0.5)]
        [InlineData(0.70711, 0.5, 0.70711, 0.375, 0.5)]
        [InlineData(0, -0.25, 1, 0.5, 0.75)]
        [InlineData(-0.70711, 0.5, 0.70711, 0.625, 0.5)]
        [InlineData(-1, 1.25, 0, 0.75, 0.25)]
        [InlineData(-0.70711, 0.5, -0.70711, 0.875, 0.5)]
        public void UsingCylindrical3DPoint(double x, double y, double z, double uExp, double vExp)
        {
            var p = pt.Point(x, y, z);
            (var u, var v) = patterns.UV.Pattern.CylindricalMap(p);

            Assert.Equal(uExp, u, 3);
            Assert.Equal(vExp, v, 3);
        }

        [Theory]
        [InlineData(0.5, 0.5, "main")]
        [InlineData(0.1, 0.9, "ul")]
        [InlineData(0.9, 0.9, "ur")]
        [InlineData(0.1, 0.1, "bl")]
        [InlineData(0.9, 0.1, "br")]
        public void LayoutAlignCheckPattern(double u, double v, string expected)
        {
            Dictionary<string, RTF.Color> colors = new Dictionary<string, RTF.Color>
            {
                { "main", RTF.Color.White },
                { "ul", RTF.Color.Red },
                { "ur", RTF.Color.Yellow },
                { "bl", RTF.Color.Lime },
                { "br", RTF.Color.Cyan }
            };

            var pattern = new patterns.UV.AlignCheck
                (colors["main"], 
                colors["ul"], colors["ur"], 
                colors["bl"], colors["br"]);
            var c = pattern.At(u, v);

            Assert.Equal(colors[expected], c);
        }
    }
}
