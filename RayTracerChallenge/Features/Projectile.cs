using RayTracerChallenge.Features;
using System.Collections.Generic;

namespace RayTracerChallenge.Features
{
    public class Projectile
    {
        public PointType Position { get; set; }
        public PointType Velocity { get; set; }

        public Projectile(PointType p, PointType v)
        {
            Position = p;
            Velocity = v;
        }

        public static IEnumerable<PointType> GetTick(Environment env,
                                           Projectile projectile)
        {
            var coord = projectile.Position;
            var velocity = projectile.Velocity;
            do
            {
                coord += velocity;
                velocity = velocity + env.Gravity + env.Wind;

                yield return coord;
            }
            while (coord.Y > 0);
            yield break;
        }
    }

    public class Environment
    {
        public PointType Gravity { get; set; }
        public PointType Wind { get; set; }

        public Environment(PointType g, PointType w)
        {
            Gravity = g;
            Wind = w;
        }
    }
}
