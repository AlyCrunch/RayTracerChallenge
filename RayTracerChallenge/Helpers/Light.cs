using RayTracerChallenge.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Helpers
{
    public class Light
    {
        public PointType Position { get; set; }
        public Color Intensity { get; set; }

        public Light(PointType position, Color intensity)
        {
            Position = position;
            Intensity = intensity;
        }

        public static PointType NormalAt(Sphere s, PointType worldPoint)
        {
            var objectPoint = s.Transform.Inverse() * worldPoint;
            var objectNormal = objectPoint - PointType.Point(0, 0, 0);

            worldPoint = Matrix.Transpose(s.Transform.Inverse()) * objectNormal;
            worldPoint.W = 0;

            return worldPoint.Normalizing();
        }
        public static PointType NormalAt(object s, PointType worldPoint)
        {
            if (s is Sphere)
                return NormalAt((Sphere)s, worldPoint);
            else
                throw new NotImplementedException();
        }

        public static PointType Reflect(PointType vector, PointType normal)
            => vector - normal * 2 * PointType.DotProduct(vector, normal);

        public static Color Lighting(Material material, Light light, PointType point, PointType eyeVector, PointType normalVector)
        {
            var effectiveColor = material.Color * light.Intensity;
            var lightVector = (light.Position - point).Normalizing();

            var ambient = effectiveColor * material.Ambient;

            var diffuse = Color.Black();
            var specular = Color.Black();

            var lightDotNormal = PointType.DotProduct(lightVector, normalVector);

            if (lightDotNormal >= 0)
            {
                diffuse = effectiveColor * material.Diffuse * lightDotNormal;
                var reflectVector = Reflect(-lightVector, normalVector);
                var reflectDotEye = PointType.DotProduct(reflectVector, eyeVector);
                if(reflectDotEye > 0)
                {
                    var factor = Math.Pow(reflectDotEye, material.Shininess);
                    specular = light.Intensity * material.Specular * factor;
                }
            }
            return ambient + diffuse + specular;
        }

        public Color Lighting(Material material, PointType point, PointType eyeVector, PointType normalVector)
            => Lighting(material, this, point, eyeVector, normalVector);
    }
}
