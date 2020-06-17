using System;
using System.Security.Cryptography;

namespace RayTracerChallenge.Features
{
    public class Color
    {
        public double R { get; set; } = 0;
        public double G { get; set; } = 0;
        public double B { get; set; } = 0;

        public int Red256 { get => To256(R); }
        public int Green256 { get => To256(G); }
        public int Blue256 { get => To256(B); }

        public Color(double red, double green, double blue, int @base = 1)
        {
            R = red / @base;
            G = green / @base;
            B = blue / @base;
        }
        public Color() { }

        public string To256()
            => $"{Red256} {Green256} {Blue256}";

        private int To256(double c)
        {
            var newc = c * 255;
            if (newc > 255) return 255;
            if (newc < 0) return 0;
            return (int)Math.Round(newc);
        }

        public static Color From256(int r, int g, int b)
            => new Color(r / 256, g / 256, b / 256);

        public static Color FromHex(string color)
        {
            if (color.Length < 6 && color.Length > 7) throw new Exception("Invalid color");

            if (color[0] == '#')
                color = color.Remove(0, 1);

            var hexR = color.Substring(0, 2);
            var hexG = color.Substring(2, 2);
            var hexB = color.Substring(4, 2);

            return From256(Convert.ToInt32(hexR, 16), Convert.ToInt32(hexG, 16), Convert.ToInt32(hexB, 16));
        }

        #region operator
        public static Color operator +(Color a, Color b)
            => new Color(a.R + b.R, a.G + b.G, a.B + b.B);
        public static Color operator -(Color a, Color b)
            => new Color(a.R - b.R, a.G - b.G, a.B - b.B);
        public static Color operator *(Color a, int s)
            => new Color(a.R * s, a.G * s, a.B * s);

        #region Colors
        public static Color Black { get => new Color(0, 0, 0); }
        public static Color White { get => new Color(1, 1, 1); }
        public static Color Red { get => new Color(1, 0, 0); }
        public static Color Lime { get => new Color(0, 1, 0); }
        public static Color Blue { get => new Color(0, 0, 1); }
        public static Color Yellow { get => new Color(1, 1, 0); }
        public static Color Magenta { get => new Color(1, 0, 1); }
        public static Color Cyan { get => new Color(0, 1, 1); }
        public static Color Brown { get => new Color(1, 0.5, 0); }
        public static Color Silver { get => new Color(0.75, 0.75, 0.75); }
        public static Color Gray { get => new Color(0.5, 0.5, 0.5); }
        public static Color Maroon { get => new Color(0.5, 0, 0); }
        public static Color Olive { get => new Color(0.5, 0.5, 0); }
        public static Color Green { get => new Color(0, 0.5, 0); }
        public static Color Purple { get => new Color(0.5, 0, 0.5); }
        public static Color Pink { get => new Color(1, 0.5, 0.93); }
        public static Color Teal { get => new Color(0, 0.5, 0.5); }
        public static Color Navy { get => new Color(0, 0, 0.5); }
        #endregion

        public static Color operator *(Color a, double s)
            => new Color(a.R * s, a.G * s, a.B * s);
        public static Color operator *(Color a, Color b)
            => new Color(a.R * b.R, a.G * b.G, a.B * b.B);

        #endregion

        #region overriding

        public override bool Equals(object obj)
        {
            return obj is Color color &&
                   R == color.R &&
                   G == color.G &&
                   B == color.B;
        }

        public bool Equals(object obj, int p)
        {
            return obj is Color color &&
                   Math.Round(R, p) == Math.Round(color.R, p) &&
                   Math.Round(G, p) == Math.Round(color.G, p) &&
                   Math.Round(B, p) == Math.Round(color.B, p);
        }

        public override int GetHashCode()
        {
            var hashCode = -1058441243;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }

        public override string ToString()
            => $"({R}, {G}, {B})";
        public string ToString(string format)
            => $"({R.ToString(format)}, {G.ToString(format)}, {B.ToString(format)})";

        public static bool operator ==(Color a, Color b)
            => a.Equals(b);
        public static bool operator !=(Color a, Color b)
            => !a.Equals(b);

        #endregion
    }
}
