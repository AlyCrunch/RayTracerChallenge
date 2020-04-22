using RayTracerChallenge.Features.Shapes;

namespace RayTracerChallenge.Features.Patterns
{
    abstract public class Pattern
    {
        public Color A { get; set; }
        public Color B { get; set; }
        public Matrix Transform { get; set; }

        public Pattern() { Transform = Matrix.GetIdentity(); }
        public Pattern(Color a, Color b)
        {
            Transform = Matrix.GetIdentity();
            A = a;
            B = b;
        }

        public static Stripe GetStripePattern(Color a, Color b) => new Stripe(a, b);
        public static Gradient GetGradientPattern(Color a, Color b) => new Gradient(a, b);
        public static Ring GetRingPattern(Color a, Color b) => new Ring(a, b);
        public static Checker GetCheckerPattern(Color a, Color b) => new Checker(a, b);

        abstract public Color At(PointType point);

        public Color AtObject(Shape obj, PointType worldPoint)
        {
            var objectPoint = obj.Transform.Inverse() * worldPoint;
            var patternPoint = Transform.Inverse() * objectPoint;

            return At(patternPoint);
        }
    }
}