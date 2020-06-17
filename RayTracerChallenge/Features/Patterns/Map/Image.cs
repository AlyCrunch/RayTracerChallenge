using System;

namespace RayTracerChallenge.Features.Patterns.Map
{
    public class Image : Pattern
    {
        public Canvas Canvas { get; set; }

        public Image(Canvas c) { Canvas = c; }

        public override Color At(double u, double v)
        {
            v = 1 - v;
            var x = u * (Canvas.Width - 1);
            var y = v * (Canvas.Height - 1);

            return Canvas.PixelAt((int)Math.Round(x), (int)Math.Round(y));
        }
    }
}
