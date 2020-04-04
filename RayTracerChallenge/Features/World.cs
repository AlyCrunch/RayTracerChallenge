using RayTracerChallenge.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Features
{
    public class World
    {
        public List<object> Objects { get; set; } = new List<object>();
        public Light Light { get; set; }

        public static World Default()
        {
            return new World
            {
                Light = new Light(
                    PointType.Point(-10, 10, -10),
                    Color.White()),
                Objects = new List<object>
                {
                    new Sphere(
                            new Material(
                                new Color(0.8, 1, 0.6),
                                0.7,
                                0.2
                            )
                        ),
                    new Sphere(
                            Transformations.Scaling(0.5, 0.5, 0.5)
                            )
                }
            };
        }
    }
}
