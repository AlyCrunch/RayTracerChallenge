using System;

namespace RayTracerChallenge.Features.Patterns
{
    public class TextureMap : Pattern
    {
        public Func<PointType, (double u, double v)> UVMap { get; set; }
        public Map.Pattern Pattern { get; set; }

        public TextureMap() { }
        public TextureMap(Func<PointType, (double u, double v)> map, Map.Pattern pattern)
        {
            UVMap = map;
            Pattern = pattern;
        }

        public override Color At(PointType point)
        {
            (var u, var v) = UVMap(point);
            return Pattern.At(u, v);
        }
    }
}
