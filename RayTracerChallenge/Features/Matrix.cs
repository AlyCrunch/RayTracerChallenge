using RayTracerChallenge.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracerChallenge.Features
{
    public class Matrix
    {
        public double[,] Content { get; set; }

        public double this[int x, int y]
        {
            get => Content[x, y];
            set
            {
                Content[x, y] = value;
            }
        }

        public int Width { get => Content.GetLength(0); }
        public int Height { get => Content.GetLength(1); }

        public Matrix(int width, int height)
        {
            Content = new double[width, height];
        }

        public void SetRow(int index, double[] line)
        {
            if (line.Length != Content.GetLength(0))
                throw new Exception("Missing values");

            for (int i = 0; i < line.Length; i++)
            {
                Content[index, i] = line[i];
            }
        }

        public void SetColumn(int index, double[] col)
        {
            if (col.Length != Content.GetLength(1))
                throw new Exception("Missing values");

            for (int i = 0; i < col.Length; i++)
            {
                Content[i, index] = col[i];
            }
        }

        public static Matrix Transpose(Matrix m)
        {
            var M = new Matrix(m.Width, m.Height);

            for (int i = 0; i < m.Width; i++)
            {
                var row = m.Content.GetRow(i);
                M.SetColumn(i, row);
            }

            return M;
        }
        public Matrix Transpose()
            => Transpose(this);

        public static Matrix GetIdentity(int width = 4, int height = 4)
        {
            var m = new Matrix(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == y) m[x, y] = 1;
                }
            }

            return m;
        }

        public static double Determinant(Matrix m)
        {
            double d = 0;
            if (m.Height == 2 && m.Width == 2)
                d = m[0, 0] * m[1, 1] - m[0, 1] * m[1, 0];
            else
                for (int col = 0; col < m.Height; col++)
                    d += m[0, col] * Cofactor(m, 0, col);

            return d;
        }
        public double Determinant() => Determinant(this);

        public static Matrix Submatrix(Matrix m, int row, int column)
        {

            Matrix x = new Matrix(m.Width - 1, m.Height - 1);
            var newIndex = 0;

            for (int i = 0; i < m.Height; i++)
            {
                if (i == row) continue;
                var n_row = m.Content.GetRow(i).RemoveAt(column);
                x.SetRow(newIndex, n_row);
                newIndex++;
            }

            return x;
        }

        public static double Minor(Matrix m, int row, int column)
            => Determinant(Submatrix(m, row, column));

        public static double Cofactor(Matrix m, int row, int column)
        {
            var minor = Minor(m, row, column);
            return ((row + column) % 2 == 0) ? minor : -minor;
        }

        public static bool IsInvertible(Matrix m)
            => Determinant(m) != 0;
        public bool IsInvertible() => IsInvertible(this);

        public static Matrix Inverse(Matrix a)
        {
            if (!a.IsInvertible())
                throw new Exception("This matrix is not invertible.");

            var m2 = new Matrix(a.Width, a.Height);

            for (int row = 0; row < a.Height; row++)
            {
                for (int col = 0; col < a.Width; col++)
                {
                    var c = Cofactor(a, col, row);
                    m2[row, col] = c / Determinant(a);
                }
            }
            return m2;
        }
        public Matrix Inverse()
            => Inverse(this);


        #region overriding

        public static bool operator ==(Matrix a, Matrix b)
            => a.Equals(b);
        public static bool operator !=(Matrix a, Matrix b)
            => !a.Equals(b);
        public static Matrix operator *(Matrix a, Matrix b)
        {
            var m = new Matrix(a.Width, a.Height);

            if (a.Width != b.Width || a.Height != b.Height) throw new Exception("Not the same size.");

            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    double value = 0;
                    for (int i = 0; i < a.Width; i++)
                    {
                        value += a[x, i] * b[i, y];
                    }
                    m[x, y] = value;
                }
            }

            return m;
        }
        public static double[] operator *(Matrix a, double[] b)
        {
            List<double> m = new List<double>();

            if (a.Width != b.Length || a.Height != a.Width)
                throw new Exception("Not the same size.");

            for (int y = 0; y < a.Height; y++)
            {
                double value = 0;
                for (int i = 0; i < a.Width; i++)
                {
                    value += a[y, i] * b[i];
                }
                m.Add(value);
            }

            return m.ToArray();
        }
        public static PointType operator *(Matrix a, PointType b)
        {
            double[] arr = { b.X, b.Y, b.Z, b.W };
            var m = a * arr;
            return new PointType(m[0], m[1], m[2], m[3]);
        }

        public override string ToString()
        {
            string toReturn = $"Size = [{Width}x{Height}]\n";
            for (int y = 0; y < Width; y++)
            {
                toReturn += $"| {string.Join(" | ", Content.GetRow(y))} |\n";
            }
            return toReturn;
        }
        public string ToString(string format)
        {
            string toReturn = $"Size = [{Width}x{Height}]\n";
            for (int y = 0; y < Width; y++)
            {
                toReturn += $"| {string.Join(" | ", Content.GetRow(y).Select(x => x.ToString(format)))} |\n";
            }
            return toReturn;
        }
        public override bool Equals(object obj)
        {
            Matrix m = (Matrix)obj;
            for (int x = 0; x < Height; x++)
                for (int y = 0; y < Width; y++)
                    if (this[x, y] != m[x, y])
                        return false;
            return true;
        }

        public bool Equals(Matrix m, int p)
        {
            for (int x = 0; x < Height; x++)
                for (int y = 0; y < Width; y++)
                    if (Math.Round(this[x, y], p) != Math.Round(m[x, y], p))
                        return false;
            return true;
        }
        public override int GetHashCode()
        {
            return EqualityComparer<double[,]>.Default.GetHashCode(Content);
        }
        #endregion
    }
}