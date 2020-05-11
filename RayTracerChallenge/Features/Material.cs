using System.Collections.Generic;
using RayTracerChallenge.Features.Patterns;

namespace RayTracerChallenge.Features
{
    public class Material
    {
        public double Ambient { get; set; } = 0.1;
        public Color Color { get; set; } = Color.White;
        public double Diffuse { get; set; } = 0.9;
        public double Shininess { get; set; } = 200;
        public double Specular { get; set; } = 0.9;
        public Pattern Pattern { get; set; }
        public double Reflective { get; set; } = 0;
        public double Transparency { get; set; } = 0;
        public double RefractiveIndex { get; set; } = 1;

        public Material() { }

        public Material(Color color, double diffuse, double specular)
        {
            Color = color;
            Diffuse = diffuse;
            Specular = specular;
        }

        #region Overriding
        public override bool Equals(object obj)
        {
            return obj is Material material &&
                   Ambient == material.Ambient &&
                   Color == material.Color &&
                   Diffuse == material.Diffuse &&
                   Shininess == material.Shininess &&
                   Specular == material.Specular;
        }
        public override int GetHashCode()
        {
            int hashCode = -2043694805;
            hashCode = hashCode * -1521134295 + Ambient.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(Color);
            hashCode = hashCode * -1521134295 + Diffuse.GetHashCode();
            hashCode = hashCode * -1521134295 + Shininess.GetHashCode();
            hashCode = hashCode * -1521134295 + Specular.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Material a, Material b)
            => a.Equals(b);
        public static bool operator !=(Material a, Material b)
            => !a.Equals(b);
        #endregion
    }
}
