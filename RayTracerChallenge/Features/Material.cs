using System.Collections.Generic;
using RayTracerChallenge.Features.Patterns;

namespace RayTracerChallenge.Features
{
    public class Material
    {
        public double Ambient { get; set; }
        public Color Color { get; set; }
        public double Diffuse { get; set; }
        public double Shininess { get; set; }
        public double Specular { get; set; }
        public Pattern Pattern { get; set; }

        public Material()
        {
            Color = new Color(1, 1, 1);
            Ambient = 0.1;
            Diffuse = 0.9;
            Specular = 0.9;
            Shininess = 200;
        }

        public Material(Color color, double diffuse, double specular)
        {
            Color = color;
            Diffuse = diffuse;
            Specular = specular;
            Ambient = 0.1;
            Shininess = 200;
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
