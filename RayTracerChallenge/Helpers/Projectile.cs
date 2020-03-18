using RayTracerChallenge.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Helpers
{
    public class Projectile<T> where T : IComparable<T>
    {
        public PointType<T> Position { get; set; }
        public PointType<T> Velocity { get; set; }

        public Projectile(PointType<T> p, PointType<T> v)
        {
            Position = p;
            Velocity = v;
        }

        public static IEnumerable<PointType<float>> GetTick(Environment<float> env,
                                           Projectile<float> projectile)
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

    public class Environment<T> where T : IComparable<T>
    {
        public PointType<T> Gravity { get; set; }
        public PointType<T> Wind { get; set; }

        public Environment(PointType<T> g, PointType<T> w)
        {
            Gravity = g;
            Wind = w;
        }
    }
}
