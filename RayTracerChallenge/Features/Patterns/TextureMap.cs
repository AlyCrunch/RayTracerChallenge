using System;

namespace RayTracerChallenge.Features.Patterns
{
    public class TextureMap : Pattern
    {
        public Func<PointType, (double u, double v)> UVMap { get; set; }
        public UV.Pattern UVPattern { get; set; }

        public TextureMap() { }
        public TextureMap(Func<PointType, (double u, double v)> map, UV.Pattern pattern)
        {
            UVMap = map;
            UVPattern = pattern;
        }

        public override Color At(PointType point)
        {
            (var u, var v) = UVMap(point);
            return UVPattern.At(u, v);
        }
    }
}
