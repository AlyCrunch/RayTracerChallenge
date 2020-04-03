using RTF = RayTracerChallenge.Features;
using Xunit;

namespace Tests.RTC
{
    public class Canvas
    {
        [Fact]
        public void CreateCanvas()
        {
            var c = new RTF.Canvas(10, 20);

            Assert.Equal(10, c.Width);
            Assert.Equal(20, c.Height);
            Assert.True(c.EveryPixelsAre(new RTF.Color(0, 0, 0)));
        }

        [Fact]
        public void WritingPixelToCanvas()
        {
            var c = new RTF.Canvas(10, 20);
            var red = new RTF.Color(1, 0, 0);
            c.WritePixel(2, 3, red);

            Assert.Equal(red, c.PixelAt(2, 3));
        }

        [Fact]
        public void ConstructPPMHeader()
        {
            string expected = "P3\n5 3\n255";
            var canvas = new RTF.Canvas(5, 3);

            Assert.Equal(expected, string.Join("\n", canvas.CreatePPMHeader()));
        }

        [Fact]
        public void ConstructPPMPixelData()
        {
            var expected = "255 0 0 0 0 0 0 0 0 0 0 0 0 0 0\n" +
                           "0 0 0 0 0 0 0 128 0 0 0 0 0 0 0\n" +
                           "0 0 0 0 0 0 0 0 0 0 0 0 0 0 255";

            var c = new RTF.Canvas(5, 3);
            var c1 = new RTF.Color(1.5, 0, 0);
            var c2 = new RTF.Color(0, 0.5, 0);
            var c3 = new RTF.Color(-0.5, 0, 1);

            c.WritePixel(0, 0, c1);
            c.WritePixel(2, 1, c2);
            c.WritePixel(4, 2, c3);

            Assert.Equal(expected, string.Join("\n", c.CreatePPMCanvas()));
        }
    }
}