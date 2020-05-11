using System;
using RayTracerChallenge.Helpers;

namespace RayTracerChallenge.Features.Patterns
{
    public class Perturbed : Pattern
    {
        public Pattern JitteredPattern { get; set; }

        public Perturbed(Pattern p)
        {
            JitteredPattern = p;
        }

        public override Color At(PointType point)
        {
            var noise = ImprovedNoise.Noise(point);

            return JitteredPattern.At(JitteredPattern.Transform.Inverse() * point * noise);
        }
    }
}
