﻿using RTF = RayTracerChallenge.Features;
using System;
using Xunit;

namespace Tests.RTC
{
    public class Color
    {
        [Fact]
        public void CreateColor()
        {
            var c = new RTF.Color(-0.5, 0.4, 1.7);
            Assert.Equal(-0.5, c.R, 5);
            Assert.Equal(0.4, c.G, 5);
            Assert.Equal(1.7, c.B, 5);
        }

        [Fact]
        public void AddingColor()
        {
            var c1 = new RTF.Color(0.9, 0.6, 0.75);
            var c2 = new RTF.Color(0.7, 0.1, 0.25);
            var result = c1 + c2;

            Assert.Equal(1.6, result.R, 5);
            Assert.Equal(0.7, result.G, 5);
            Assert.Equal(1, result.B, 5);
        }
        [Fact]
        public void SubstractionColor()
        {
            var c1 = new RTF.Color(0.9, 0.6, 0.75);
            var c2 = new RTF.Color(0.7, 0.1, 0.25);
            var result = c1 - c2;

            Assert.Equal(0.2, result.R, 5);
            Assert.Equal(0.5, result.G, 5);
            Assert.Equal(0.5, result.B, 5);
        }
        [Fact]
        public void MultiplyingScalarColor()
        {
            var c1 = new RTF.Color(0.2, 0.3, 0.4);
            var result = c1 * 2;

            Assert.Equal(0.4, result.R, 5);
            Assert.Equal(0.6, result.G, 5);
            Assert.Equal(0.8, result.B, 5);
        }

        [Fact]
        public void MultiplyingColor()
        {
            var c1 = new RTF.Color(1, 0.2, 0.4);
            var c2 = new RTF.Color(0.9, 1, 0.1);
            var result = c1 * c2;

            Assert.Equal(0.9, result.R, 5);
            Assert.Equal(0.2, result.G, 5);
            Assert.Equal(0.04, result.B, 5);
        }

    }
}
