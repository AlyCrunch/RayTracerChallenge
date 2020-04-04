using System;

namespace RayTracerChallenge.Features
{
    public class Color
    {
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }

        public int Red256 { get => To256(Red); }
        public int Green256 { get => To256(Green); }
        public int Blue256 { get => To256(Blue); }

        public Color(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
        public Color()
        {
            Red = 0;
            Green = 0;
            Blue = 0;
        }

        public string To256()
            => $"{Red256} {Green256} {Blue256}";

        private int To256(double c)
        {
            var newc = c * 255;
            if (newc > 255) return 255;
            if (newc < 0) return 0;
            return (int)Math.Round(newc);
        }

        #region operator
        public static Color operator +(Color a, Color b)
            => new Color(a.Red + b.Red, a.Green + b.Green, a.Blue + b.Blue);
        public static Color operator -(Color a, Color b)
            => new Color(a.Red - b.Red, a.Green - b.Green, a.Blue - b.Blue);
        public static Color operator *(Color a, int s)
            => new Color(a.Red * s, a.Green * s, a.Blue * s);
        
        #region Colors
        public static Color Black() => new Color(0, 0, 0);
        public static Color White() => new Color(1, 1, 1);
        public static Color Red_() => new Color(1, 0, 0);
        public static Color Lime() => new Color(0, 1, 0);
        public static Color Blue_() => new Color(0, 0, 1);
        public static Color Yellow() => new Color(1, 1, 0);
        public static Color Magenta() => new Color(1, 0, 1);
        public static Color Cyan() => new Color(0, 1, 1);
        public static Color Silver() => new Color(0.75, 0.75, 0.75);
        public static Color Gray() => new Color(0.5, 0.5, 0.5);
        public static Color Maroon() => new Color(0.5, 0, 0);
        public static Color Olive() => new Color(0.5, 0.5, 0);
        public static Color Green_() => new Color(0, 0.5, 0);
        public static Color Purple() => new Color(0.5, 0, 0.5);
        public static Color Teal() => new Color(0, 0.5, 0.5);
        public static Color Navy() => new Color(0, 0, 0.5);
        #endregion

        public static Color operator *(Color a, double s)
            => new Color(a.Red * s, a.Green * s, a.Blue * s);
        public static Color operator *(Color a, Color b)
            => new Color(a.Red * b.Red, a.Green * b.Green, a.Blue * b.Blue);

        #endregion

        #region overriding

        public override bool Equals(object obj)
        {
            return obj is Color color &&
                   Red == color.Red &&
                   Green == color.Green &&
                   Blue == color.Blue;
        }

        public bool Equals(object obj, int p)
        {
            return obj is Color color &&
                   Math.Round(Red, p) == Math.Round(color.Red, p) &&
                   Math.Round(Green, p) == Math.Round(color.Green, p) &&
                   Math.Round(Blue, p) == Math.Round(color.Blue, p);
        }

        public override int GetHashCode()
        {
            var hashCode = -1058441243;
            hashCode = hashCode * -1521134295 + Red.GetHashCode();
            hashCode = hashCode * -1521134295 + Green.GetHashCode();
            hashCode = hashCode * -1521134295 + Blue.GetHashCode();
            return hashCode;
        }

        public override string ToString()
            => $"({Red},{Green},{Blue})";
        public string ToString(string format)
            => $"({Red.ToString(format)},{Green.ToString(format)},{Blue.ToString(format)})";

        public static bool operator ==(Color a, Color b)
            => a.Equals(b);
        public static bool operator !=(Color a, Color b)
            => !a.Equals(b);

        #endregion
    }
}
