using RTF = RayTracerChallenge.Features;
using pt = RayTracerChallenge.Features.PointType;
using patterns = RayTracerChallenge.Features.Patterns;
using Xunit;
using System.Collections.Generic;
using System;
using System.Linq;
using Tests.RTC.Helpers;

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
            var checker = new patterns.Map.Checker(2, 2, black, RTF.Color.White);
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
        public void SphericalMapping3DPoint(double x, double y, double z, double uExp, double vExp)
        {
            var point = pt.Point(x, y, z);
            (var u, var v) = patterns.Map.Pattern.SphericalMap(point);

            Assert.Equal(uExp, u);
            Assert.Equal(vExp, v);
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
            var checker = new patterns.Map.Checker(16, 8, black, RTF.Color.White);
            var pattern = new patterns.TextureMap(patterns.Map.Pattern.SphericalMap, checker);

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
            (var u, var v) = patterns.Map.Pattern.PlanarMap(p);

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
            (var u, var v) = patterns.Map.Pattern.CylindricalMap(p);

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

            var pattern = new patterns.Map.AlignCheck
                (colors["main"],
                colors["ul"], colors["ur"],
                colors["bl"], colors["br"]);
            var c = pattern.At(u, v);

            Assert.Equal(colors[expected], c);
        }

        [Theory]
        [InlineData(-1, 0.5, -0.25, "left")]
        [InlineData(1.1, -0.75, 0.8, "right")]
        [InlineData(0.1, 0.6, 0.9, "front")]
        [InlineData(-0.7, 0, -2, "back")]
        [InlineData(0.5, 1, 0.9, "up")]
        [InlineData(-0.2, -1.3, 1.1, "down")]
        public void IdentifyingFaceOfACubeFromPoint(double x, double y, double z, string face)
        {
            var p = pt.Point(x, y, z);
            var map = patterns.Map.Cube.FaceFromPoint(p);

            var eFace = Enum.Parse(typeof(patterns.Map.Faces), FirstCharToUpper(face));

            Assert.Equal(eFace, map);

            static string FirstCharToUpper(string input)
            {
                return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        [Theory]
        [InlineData(-0.5, 0.5, 1, 0.25, 0.75)]
        [InlineData(0.5, -0.5, 1, 0.75, 0.25)]
        public void UVMappingFrontFaceCube(double x, double y, double z, double uExp, double vExp)
        {
            var p = pt.Point(x, y, z);
            (var u, var v) = patterns.Map.Cube.GetUVFront(p);

            Assert.Equal(uExp, u);
            Assert.Equal(vExp, v);
        }
        [Theory]
        [InlineData(0.5, 0.5, -1, 0.25, 0.75)]
        [InlineData(-0.5, -0.5, -1, 0.75, 0.25)]
        public void UVMappingFrontBackCube(double x, double y, double z, double uExp, double vExp)
        {
            var p = pt.Point(x, y, z);
            (var u, var v) = patterns.Map.Cube.GetUVBack(p);

            Assert.Equal(uExp, u);
            Assert.Equal(vExp, v);
        }
        [Theory]
        [InlineData(-1, 0.5, -0.5, 0.25, 0.75)]
        [InlineData(-1, -0.5, 0.5, 0.75, 0.25)]
        public void UVMappingFrontLeftCube(double x, double y, double z, double uExp, double vExp)
        {
            var p = pt.Point(x, y, z);
            (var u, var v) = patterns.Map.Cube.GetUVLeft(p);

            Assert.Equal(uExp, u);
            Assert.Equal(vExp, v);
        }
        [Theory]
        [InlineData(1, 0.5, 0.5, 0.25, 0.75)]
        [InlineData(1, -0.5, -0.5, 0.75, 0.25)]
        public void UVMappingFrontRightCube(double x, double y, double z, double uExp, double vExp)
        {
            var p = pt.Point(x, y, z);
            (var u, var v) = patterns.Map.Cube.GetUVRight(p);

            Assert.Equal(uExp, u);
            Assert.Equal(vExp, v);
        }
        [Theory]
        [InlineData(-0.5, 1, -0.5, 0.25, 0.75)]
        [InlineData(0.5, 1, 0.5, 0.75, 0.25)]
        public void UVMappingFrontUpCube(double x, double y, double z, double uExp, double vExp)
        {
            var p = pt.Point(x, y, z);
            (var u, var v) = patterns.Map.Cube.GetUVUp(p);

            Assert.Equal(uExp, u);
            Assert.Equal(vExp, v);
        }
        [Theory]
        [InlineData(-0.5, -1, 0.5, 0.25, 0.75)]
        [InlineData(0.5, -1, -0.5, 0.75, 0.25)]
        public void UVMappingFrontDownCube(double x, double y, double z, double uExp, double vExp)
        {
            var p = pt.Point(x, y, z);
            (var u, var v) = patterns.Map.Cube.GetUVDown(p);

            Assert.Equal(uExp, u);
            Assert.Equal(vExp, v);
        }

        [Theory]
        [InlineData(-1, 0, 0, "yellow")]
        [InlineData(-1, 0.9, -0.9, "cyan")]
        [InlineData(-1, 0.9, 0.9, "red")]
        [InlineData(-1, -0.9, -0.9, "blue")]
        [InlineData(-1, -0.9, 0.9, "brown")]
        [InlineData(0, 0, 1, "cyan")]
        [InlineData(-0.9, 0.9, 1, "red")]
        [InlineData(0.9, 0.9, 1, "yellow")]
        [InlineData(-0.9, -0.9, 1, "brown")]
        [InlineData(0.9, -0.9, 1, "green")]
        [InlineData(1, 0, 0, "red")]
        [InlineData(1, 0.9, 0.9, "yellow")]
        [InlineData(1, 0.9, -0.9, "purple")]
        [InlineData(1, -0.9, 0.9, "green")]
        [InlineData(1, -0.9, -0.9, "white")]
        [InlineData(0, 0, -1, "green")]
        [InlineData(0.9, 0.9, -1, "purple")]
        [InlineData(-0.9, 0.9, -1, "cyan")]
        [InlineData(0.9, -0.9, -1, "white")]
        [InlineData(-0.9, -0.9, -1, "blue")]
        [InlineData(0, 1, 0, "brown")]
        [InlineData(-0.9, 1, -0.9, "cyan")]
        [InlineData(0.9, 1, -0.9, "purple")]
        [InlineData(-0.9, 1, 0.9, "red")]
        [InlineData(0.9, 1, 0.9, "yellow")]
        [InlineData(0, -1, 0, "purple")]
        [InlineData(-0.9, -1, 0.9, "brown")]
        [InlineData(0.9, -1, 0.9, "green")]
        [InlineData(-0.9, -1, -0.9, "blue")]
        [InlineData(0.9, -1, -0.9, "white")]
        public void FindingColorsMappedCube(double x, double y, double z, string expected)
        {
            var p = pt.Point(x, y, z);
            Dictionary<string, RTF.Color> colors = new Dictionary<string, RTF.Color>
            {
                { "red", RTF.Color.Red },
                { "yellow", RTF.Color.Yellow },
                { "brown", RTF.Color.Brown },
                { "green", RTF.Color.Lime },
                { "cyan", RTF.Color.Cyan },
                { "blue", RTF.Color.Blue },
                { "purple", RTF.Color.Purple },
                { "white", RTF.Color.White }
            };
            var left = new patterns.Map.AlignCheck
                (colors["yellow"], colors["cyan"], colors["red"], colors["blue"], colors["brown"]);
            var front = new patterns.Map.AlignCheck
                (colors["cyan"], colors["red"], colors["yellow"], colors["brown"], colors["green"]);
            var right = new patterns.Map.AlignCheck
                (colors["red"], colors["yellow"], colors["purple"], colors["green"], colors["white"]);
            var back = new patterns.Map.AlignCheck
                (colors["green"], colors["purple"], colors["cyan"], colors["white"], colors["blue"]);
            var up = new patterns.Map.AlignCheck
                (colors["brown"], colors["cyan"], colors["purple"], colors["red"], colors["yellow"]);
            var down = new patterns.Map.AlignCheck
                (colors["purple"], colors["brown"], colors["green"], colors["blue"], colors["white"]);

            var cube = new patterns.Map.Cube(left, front, right, back, up, down);
            var color = cube.At(p);

            Assert.Equal(colors[expected], color);
        }

        [Fact]
        public void ReadingFileWithWrongMagicNumber()
        {
            var ppm = new List<string> {
                "P32",
                "1 1",
                "255",
                "0 0 0"
            };
            Assert.Throws<Exception>(() => RTF.Canvas.CanvasFromPPM(ppm));
        }

        [Fact]
        public void ReadingPPMReturnCanvasRightSize()
        {
            var ppm = new List<string> {
                "P3",
                "10 2",
                "255",
                "0 0 0  0 0 0  0 0 0  0 0 0  0 0 0",
                "0 0 0  0 0 0  0 0 0  0 0 0  0 0 0",
                "0 0 0  0 0 0  0 0 0  0 0 0  0 0 0",
                "0 0 0  0 0 0  0 0 0  0 0 0  0 0 0"
            };
            var canvas = RTF.Canvas.CanvasFromPPM(ppm);
            Assert.Equal(10, canvas.Width);
            Assert.Equal(2, canvas.Height);
        }

        [Theory]
        [InlineData(0, 0, 1, 0.498, 0)]
        [InlineData(1, 0, 0, 0.498, 1)]
        [InlineData(2, 0, 0.498, 1, 0)]
        [InlineData(3, 0, 1, 1, 1)]
        [InlineData(0, 1, 0, 0, 0)]
        [InlineData(1, 1, 1, 0, 0)]
        [InlineData(2, 1, 0, 1, 0)]
        [InlineData(3, 1, 0, 0, 1)]
        [InlineData(0, 2, 1, 1, 0)]
        [InlineData(1, 2, 0, 1, 1)]
        [InlineData(2, 2, 1, 0, 1)]
        [InlineData(3, 2, 0.498, 0.498, 0.498)]
        public void ReadingPixelDataFromPPM(int x, int y, double R, double G, double B)
        {
            var ppm = new List<string> {
                "P3",
                "4 3",
                "255",
                "255 127 0  0 127 255  127 255 0  255 255 255",
                "0 0 0  255 0 0  0 255 0  0 0 255",
                "255 255 0  0 255 255  255 0 255  127 127 127"
            };

            var canvas = RTF.Canvas.CanvasFromPPM(ppm);
            var expectedColor = new RTF.Color(R, G, B);
            var color = canvas.PixelAt(x, y);

            CustomAssert.Equal(expectedColor, color, 3);
        }

        [Fact]
        public void PPMParsingIgnoresCommentLines()
        {
            var ppm = new List<string> {
                "P3",
                "# this is a comment",
                "2 1",
                "# this, too",
                "255",
                "# another comment",
                "255 255 255",
                "# oh, no, comments in the pixel data!",
                "255 0 255"
            };
            var canvas = RTF.Canvas.CanvasFromPPM(ppm);
            Assert.Equal(new RTF.Color(1, 1, 1), canvas.PixelAt(0, 0));
            Assert.Equal(new RTF.Color(1, 0, 1), canvas.PixelAt(1, 0));
        }

        [Fact]
        public void PPMParsingRGBTripleSpanLines()
        {
            var ppm = new List<string> {
                "P3",
                "1 1",
                "255",
                "51",
                "153",
                "",
                "204"
            };
            var canvas = RTF.Canvas.CanvasFromPPM(ppm);
            Assert.Equal(new RTF.Color(0.2, 0.6, 0.8), canvas.PixelAt(0, 0));
        }

        [Fact]
        public void PPMParsingRespectsScaleSetting()
        {
            var ppm = new List<string> {
                "P3",
                "2 2",
                "100",
                "100 100 100  50 50 50",
                "75 50 25  0 0 0"
            };
            var canvas = RTF.Canvas.CanvasFromPPM(ppm);
            Assert.Equal(new RTF.Color(0.75, 0.5, 0.25), canvas.PixelAt(0, 1));
        }

        [Theory]
        [InlineData(0, 0, 0.9, 0.9, 0.9)]
        [InlineData(0.3, 0, 0.2, 0.2, 0.2)]
        [InlineData(0.6, 0.3, 0.1, 0.1, 0.1)]
        [InlineData(1, 1, 0.9, 0.9, 0.9)]
        public void CheckPattern2D(double u, double v, double R, double G, double B)
        {
            var ppm = new List<string> {
                "P3",
                "10 10",
                "10",
                "0 0 0  1 1 1  2 2 2  3 3 3  4 4 4  5 5 5  6 6 6  7 7 7  8 8 8  9 9 9",
                "1 1 1  2 2 2  3 3 3  4 4 4  5 5 5  6 6 6  7 7 7  8 8 8  9 9 9  0 0 0",
                "2 2 2  3 3 3  4 4 4  5 5 5  6 6 6  7 7 7  8 8 8  9 9 9  0 0 0  1 1 1",
                "3 3 3  4 4 4  5 5 5  6 6 6  7 7 7  8 8 8  9 9 9  0 0 0  1 1 1  2 2 2",
                "4 4 4  5 5 5  6 6 6  7 7 7  8 8 8  9 9 9  0 0 0  1 1 1  2 2 2  3 3 3",
                "5 5 5  6 6 6  7 7 7  8 8 8  9 9 9  0 0 0  1 1 1  2 2 2  3 3 3  4 4 4",
                "6 6 6  7 7 7  8 8 8  9 9 9  0 0 0  1 1 1  2 2 2  3 3 3  4 4 4  5 5 5",
                "7 7 7  8 8 8  9 9 9  0 0 0  1 1 1  2 2 2  3 3 3  4 4 4  5 5 5  6 6 6",
                "8 8 8  9 9 9  0 0 0  1 1 1  2 2 2  3 3 3  4 4 4  5 5 5  6 6 6  7 7 7",
                "9 9 9  0 0 0  1 1 1  2 2 2  3 3 3  4 4 4  5 5 5  6 6 6  7 7 7  8 8 8"
            };
            var expectedColor = new RTF.Color(R, G, B);

            var canvas = RTF.Canvas.CanvasFromPPM(ppm);
            var pattern = new patterns.Map.Image(canvas);
            var color = pattern.At(u, v);

            Assert.Equal(expectedColor, color);
        }
    }
}
