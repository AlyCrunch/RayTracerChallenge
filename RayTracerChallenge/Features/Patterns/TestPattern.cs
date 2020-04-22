using RayTracerChallenge.Features.Shapes;

namespace RayTracerChallenge.Features.Patterns
{
    public class TestPattern : Pattern
    {
        public TestPattern(){ Transform = Matrix.GetIdentity(); }
        public TestPattern(Color a, Color b)
        {
            Transform = Matrix.GetIdentity();
            A = a;
            B = b;
        }

        public override Color At(PointType point)
        {
            return new Color(point.X, point.Y, point.Z);
        }
    }
}
