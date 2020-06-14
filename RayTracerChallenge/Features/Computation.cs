using RayTracerChallenge.Features.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using transform = RayTracerChallenge.Helpers.Transformations;
using pt = RayTracerChallenge.Features.PointType;
using System.Threading;

namespace RayTracerChallenge.Features
{
    public class Computation
    {
        private const double EPSILON = 0.0001;
        public double T { get; set; }
        public Shape Object { get; set; }
        public PointType Point { get; set; }
        public PointType OverPoint { get; set; }
        public PointType UnderPoint { get; set; }
        public PointType EyeV { get; set; }
        public PointType NormalV { get; set; }
        public bool Inside { get; set; }
        public PointType RelflectV { get; set; }

        public double N1 { get; set; }
        public double N2 { get; set; }

        public static Computation PrepareComputations(Intersection i, Ray r, Intersection[] xs = null)
        {
            var c = new Computation
            {
                T = i.T,
                Object = i.Object
            };

            c.Point = transform.Position(r, c.T);
            c.EyeV = -r.Direction;
            c.NormalV = (c.Object as Shape).NormalAt(c.Point, i);

            if (PointType.DotProduct(c.NormalV, c.EyeV) < 0)
            {
                c.Inside = true;
                c.NormalV = -c.NormalV;
            }
            else
                c.Inside = false;

            c.OverPoint = c.Point + c.NormalV * EPSILON;
            c.UnderPoint = c.Point - c.NormalV * EPSILON;

            c.RelflectV = Light.Reflect(r.Direction, c.NormalV);


            if (xs == null) xs = new Intersection[] { i };
            GetRefractions(i, xs, out var n1, out var n2);
            c.N1 = n1;
            c.N2 = n2;

            return c;
        }

        public static void GetRefractions(Intersection hit, Intersection[] xs, out double n1, out double n2)
        {
            n1 = 0;
            n2 = 0;

            var container = new List<Shape>();
            foreach(var i in xs)
            {
                if(i == hit)
                {
                    n1 = (container.Count == 0) ? 
                        1 : 
                        container.Last().Material.RefractiveIndex;
                }

                if (container.Contains(i.Object))
                    container.Remove(i.Object);
                else 
                    container.Add(i.Object);

                if(i == hit)
                {
                    n2 = (container.Count == 0) ?
                        1 :
                        container.Last().Material.RefractiveIndex;
                    break;
                }
            }
        }

        public double Schlick()
        {
            var cos = pt.DotProduct(EyeV, NormalV);
            if(N1 > N2)
            {
                var n = N1 / N2;
                var sin2T = n * n * (1.0 - cos * cos);
                if (sin2T > 1) return 1;

                var cosT = Math.Sqrt(1 - sin2T);
                cos = cosT;
            }

            var r0 = Math.Pow((N1 - N2) / (N1 + N2), 2);

            return r0 + (1 - r0) * Math.Pow(1 - cos, 5);
        }
    }
}