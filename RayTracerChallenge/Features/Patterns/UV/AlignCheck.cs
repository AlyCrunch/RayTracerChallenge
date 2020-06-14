namespace RayTracerChallenge.Features.Patterns.UV
{
    public class AlignCheck : Pattern
    {
        public Color Main { get; set; }
        public Color UpLeft { get; set; }
        public Color UpRight { get; set; }
        public Color BottomLeft { get; set; }
        public Color BottomRight { get; set; }

        public AlignCheck(Color main, Color upLeft, Color upRight, Color bottomLeft, Color bottomRight)
        {
            Main = main;
            UpLeft = upLeft;
            UpRight = upRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
        }

        public override Color At(double u, double v)
        {
            if (v > 0.8)
            { 
                if (u < 0.2) return UpLeft;
                if (u > 0.8) return UpRight;
            }
            else if (v < 0.2)
            {
                if (u < 0.2) return BottomLeft;
                if (u > 0.8) return BottomRight;
            }
            return Main;
        }
    }
}
