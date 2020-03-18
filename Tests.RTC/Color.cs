using RTF = RayTracerChallenge.Features;
using System;
using Xunit;

namespace Tests.RTC
{
    public class Color
    {
        [Fact]
        public void CreateColor()
        {
            var c = new RTF.Color(-0.5m, 0.4m, 1.7m);
            Assert.Equal(-0.5m, c.Red);
            Assert.Equal(0.4m, c.Green);
            Assert.Equal(1.7m, c.Blue);
        }

        [Fact]
        public void AddingColor()
        {
            var c1 = new RTF.Color(0.9m, 0.6m, 0.75m);
            var c2 = new RTF.Color(0.7m, 0.1m, 0.25m);
            var result = c1 + c2;

            Assert.Equal(1.6m, result.Red);
            Assert.Equal(0.7m, result.Green);
            Assert.Equal(1, result.Blue);
        }
        [Fact]
        public void SubstractionColor()
        {
            var c1 = new RTF.Color(0.9m, 0.6m, 0.75m);
            var c2 = new RTF.Color(0.7m, 0.1m, 0.25m);
            var result = c1 - c2;

            Assert.Equal(0.2m, result.Red);
            Assert.Equal(0.5m, result.Green);
            Assert.Equal(0.5m, result.Blue);
        }
        [Fact]
        public void MultiplyingScalarColor()
        {
            var c1 = new RTF.Color(0.2m, 0.3m, 0.4m);
            var result = c1 * 2;

            Assert.Equal(0.4m, result.Red);
            Assert.Equal(0.6m, result.Green);
            Assert.Equal(0.8m, result.Blue);
        }

        [Fact]
        public void MultiplyingColor()
        {
            var c1 = new RTF.Color(1, 0.2m, 0.4m);
            var c2 = new RTF.Color(0.9m, 1, 0.1m);
            var result = c1 * c2;

            Assert.Equal(0.9m, result.Red);
            Assert.Equal(0.2m, result.Green);
            Assert.Equal(0.04m, result.Blue);
        }

    }
}
