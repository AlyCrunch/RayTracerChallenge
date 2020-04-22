namespace RayTracerChallenge.Features.Patterns
{
    public class Stripe : Pattern
    {
        public Stripe(Color a, Color b)
        {
            Transform = Matrix.GetIdentity();
            A = a;
            B = b;
        }
        public Stripe() { Transform = Matrix.GetIdentity(); }

        public override Color At(PointType point)
        {
            if (point.X < 0)
                return (point.X % 2 >= -1) ? B : A;

            return (point.X % 2 < 1) ? A : B;
        }
    }
}