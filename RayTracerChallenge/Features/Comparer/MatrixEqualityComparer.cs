using System;
using System.Collections.Generic;

namespace RayTracerChallenge.Features.Comparer
{

    public class MatrixEqualityComparer : IEqualityComparer<Matrix>
    {
        public bool Equals(Matrix a, Matrix b, int precision)
        {
            for (int x = 0; x < a.Height; x++)
                for (int y = 0; y < a.Width; y++)
                    if (Math.Round(a[x, y], precision) != Math.Round(b[x, y], precision))
                        return false;

            return true;
        }

        public bool Equals(Matrix a, Matrix b)
        {
            for (int x = 0; x < a.Height; x++)
            {
                for (int y = 0; y < a.Width; y++)
                {
                    if (a[x, y] != b[x, y]) return false;
                }
            }
            return true;
        }

        public int GetHashCode(Matrix m)
            => EqualityComparer<double[,]>.Default.GetHashCode(m.Content);
    }
}
