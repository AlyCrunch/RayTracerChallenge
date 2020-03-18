using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracerChallenge.Features
{
    public class Color
    {
        public decimal Red { get; set; }
        public decimal Green { get; set; }
        public decimal Blue { get; set; }

        public int Red255 { get => To255(Red); }
        public int Green255 { get => To255(Green); }
        public int Blue255 { get => To255(Blue); }

        public Color(decimal red, decimal green, decimal blue)
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

        public string To255()
            => $"{Red255} {Green255} {Blue255}";

        private int To255(decimal c)
        {
            var newc = c * 255;
            if (newc > 255) return 255;
            if (newc < 0) return 0;
            return decimal.ToInt32(decimal.Round(newc));
        }

        #region operator
        public static Color operator +(Color a, Color b)
            => new Color(a.Red + b.Red, a.Green + b.Green, a.Blue + b.Blue);
        public static Color operator -(Color a, Color b)
            => new Color(a.Red - b.Red, a.Green - b.Green, a.Blue - b.Blue);
        public static Color operator *(Color a, int s)
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

        public override int GetHashCode()
        {
            var hashCode = -1058441243;
            hashCode = hashCode * -1521134295 + Red.GetHashCode();
            hashCode = hashCode * -1521134295 + Green.GetHashCode();
            hashCode = hashCode * -1521134295 + Blue.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Color a, Color b)
            => a.Equals(b);
        public static bool operator !=(Color a, Color b)
            => !a.Equals(b);

        #endregion
    }
}
