﻿using RayTracerChallenge.Features.Patterns;
using RayTracerChallenge.Features.Shapes;
using System;
using System.Collections.Generic;

namespace RayTracerChallenge.Features
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

        public static PointType Reflect(PointType vector, PointType normal)
            => vector - normal * 2 * PointType.DotProduct(vector, normal);

        public static bool IsShadowed(World world, PointType point)
        {
            foreach (var light in world.Lights)
            {
                var v = light.Position - point;
                var distance = v.Magnetude();
                var direction = v.Normalize();

                var r = new Ray(point, direction);
                var intersections = world.Intersect(r);

                var h = Intersection.Hit(intersections);
                if (!(h is null) && h.T < distance)
                    continue;
                else
                    return false;
            }

            return true;
        }

        public static Color Lighting(Material material, Shape obj, Light light, PointType point, PointType eyeVector, PointType normalVector, bool inShadow)
        {
            var color = material.Color;
            if (material.Pattern != null)
                color = material.Pattern.AtObject(obj, point);

            var effectiveColor = color * light.Intensity;
            var lightVector = (light.Position - point).Normalize();

            var ambient = effectiveColor * material.Ambient;

            var diffuse = Color.Black;
            var specular = Color.Black;

            var lightDotNormal = PointType.DotProduct(lightVector, normalVector);

            if (lightDotNormal >= 0)
            {
                diffuse = effectiveColor * material.Diffuse * lightDotNormal;
                var reflectVector = Reflect(-lightVector, normalVector);
                var reflectDotEye = PointType.DotProduct(reflectVector, eyeVector);
                if (reflectDotEye > 0)
                {
                    var factor = Math.Pow(reflectDotEye, material.Shininess);
                    specular = light.Intensity * material.Specular * factor;
                }
            }
            return ambient + ((!inShadow) ? diffuse + specular : Color.Black);
        }

        public Color Lighting(Material material, Shape obj, PointType point, PointType eyeVector, PointType normalVector, bool inShadow)
            => Lighting(material, obj, this, point, eyeVector, normalVector, inShadow);

        #region Overriding
        public override bool Equals(object obj)
        {
            return obj is Light light &&
                   Position.Equals(light.Position) &&
                   Intensity.Equals(light.Intensity);
        }

        public override int GetHashCode()
        {
            int hashCode = 978863716;
            hashCode = hashCode * -1521134295 + EqualityComparer<PointType>.Default.GetHashCode(Position);
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(Intensity);
            return hashCode;
        }
        #endregion
    }
}
