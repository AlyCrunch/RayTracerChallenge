using RTF = RayTracerChallenge.Features;
using Xunit;

namespace Tests.RTC
{
    public class Matrice
    {
        RTF.Comparer.MatrixEqualityComparer comp = new RTF.Comparer.MatrixEqualityComparer();

        [Fact]
        public void ConstructAndInspectMatrice4x4()
        {
            var M = new RTF.Matrix(4, 4);
            M.SetRow(0, new double[] { 1, 2, 3, 4 });
            M.SetRow(1, new double[] { 5.5, 6.5, 7.5, 8.5 });
            M.SetRow(2, new double[] { 9, 10, 11, 12 });
            M.SetRow(3, new double[] { 13.5, 14.5, 15.5, 16.5 });

            Assert.Equal(1, M[0, 0]);
            Assert.Equal(4, M[0, 3]);
            Assert.Equal(5.5, M[1, 0]);
            Assert.Equal(7.5, M[1, 2]);
            Assert.Equal(11, M[2, 2]);
            Assert.Equal(13.5, M[3, 0]);
            Assert.Equal(15.5, M[3, 2]);
        }

        [Fact]
        public void OughtTo2x2()
        {
            var M = new RTF.Matrix(2, 2);
            M.SetColumn(0, new double[] { -3, 1 });
            M.SetColumn(1, new double[] { 5, -2 });

            Assert.Equal(-3, M[0, 0]);
            Assert.Equal(5, M[0, 1]);
            Assert.Equal(1, M[1, 0]);
            Assert.Equal(-2, M[1, 1]);
        }

        [Fact]
        public void OughtTo3x3()
        {
            var M = new RTF.Matrix(3, 3);
            M.SetRow(0, new double[] { -3, 5, 0 });
            M.SetRow(1, new double[] { 1, -2, 7 });
            M.SetRow(2, new double[] { 0, 1, 1 });

            Assert.Equal(-3, M[0, 0]);
            Assert.Equal(-2, M[1, 1]);
            Assert.Equal(1, M[2, 2]);
        }

        [Fact]
        public void EqualityTrue()
        {
            var M1 = new RTF.Matrix(4, 4);
            M1.SetRow(0, new double[] { 1, 2, 3, 4 });
            M1.SetRow(1, new double[] { 5, 6, 7, 8 });
            M1.SetRow(2, new double[] { 9, 8, 7, 6 });
            M1.SetRow(3, new double[] { 5, 4, 3, 2 });

            var M2 = new RTF.Matrix(4, 4);
            M2.SetRow(0, new double[] { 1, 2, 3, 4 });
            M2.SetRow(1, new double[] { 5, 6, 7, 8 });
            M2.SetRow(2, new double[] { 9, 8, 7, 6 });
            M2.SetRow(3, new double[] { 5, 4, 3, 2 });

            Assert.True(M1 == M2);
        }
        [Fact]
        public void EqualityTrueSmall()
        {
            var M1 = new RTF.Matrix(2, 2);
            M1.SetRow(0, new double[] { 0, 0 });
            M1.SetRow(1, new double[] { 1, 1 });

            var M2 = new RTF.Matrix(2, 2);
            M2.SetRow(0, new double[] { 0, 0 });
            M2.SetRow(1, new double[] { 1, 1 });

            Assert.True(M1 == M2);
        }
        [Fact]
        public void EqualityFalse()
        {
            var M1 = new RTF.Matrix(4, 4);
            M1.SetRow(0, new double[] { 1, 2, 3, 4 });
            M1.SetRow(1, new double[] { 5, 6, 7, 8 });
            M1.SetRow(2, new double[] { 9, 8, 7, 6 });
            M1.SetRow(3, new double[] { 5, 4, 3, 2 });

            var M2 = new RTF.Matrix(4, 4);
            M2.SetRow(0, new double[] { 2, 3, 4, 5 });
            M2.SetRow(1, new double[] { 6, 7, 8, 9 });
            M2.SetRow(2, new double[] { 8, 7, 6, 5 });
            M2.SetRow(3, new double[] { 4, 3, 2, 1 });

            Assert.False(M1 == M2);
        }
        [Fact]
        public void Multiply()
        {
            var M1 = new RTF.Matrix(4, 4);
            M1.SetRow(0, new double[] { 1, 2, 3, 4 });
            M1.SetRow(1, new double[] { 2, 3, 4, 5 });
            M1.SetRow(2, new double[] { 3, 4, 5, 6 });
            M1.SetRow(3, new double[] { 4, 5, 6, 7 });

            var M2 = new RTF.Matrix(4, 4);
            M2.SetRow(0, new double[] { 0, 1, 2, 4 });
            M2.SetRow(1, new double[] { 1, 2, 4, 8 });
            M2.SetRow(2, new double[] { 2, 4, 8, 16 });
            M2.SetRow(3, new double[] { 4, 8, 16, 32 });

            var M = M1 * M2;
            Assert.Equal(31, M[0, 1]);
        }
        [Fact]
        public void MultiplyTuple()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new double[] { 1, 2, 3, 4 });
            a.SetRow(1, new double[] { 2, 4, 4, 2 });
            a.SetRow(2, new double[] { 8, 6, 4, 1 });
            a.SetRow(3, new double[] { 0, 0, 0, 1 });

            double[] b = { 1, 2, 3, 1 };
            double[] e = { 18, 24, 33, 1 };

            var M = a * b;
            Assert.Equal(e, M);
        }

        [Fact]
        public void GetIdentity()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new double[] { 1, 0, 0, 0 });
            a.SetRow(1, new double[] { 0, 1, 0, 0 });
            a.SetRow(2, new double[] { 0, 0, 1, 0 });
            a.SetRow(3, new double[] { 0, 0, 0, 1 });

            var M = RTF.Matrix.GetIdentity(4, 4);
            Assert.Equal(a, M);
        }
        [Fact]
        public void MultiplyingIdentityByMatrice()
        {
            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new double[] { 0, 1, 2, 4 });
            e.SetRow(1, new double[] { 1, 2, 4, 8 });
            e.SetRow(2, new double[] { 2, 4, 8, 16 });
            e.SetRow(3, new double[] { 4, 8, 16, 32 });

            var M = RTF.Matrix.GetIdentity(4, 4) * e;
            Assert.Equal(e, M);
        }
        [Fact]
        public void MultiplyingIdentityByTuple()
        {
            var M = RTF.Matrix.GetIdentity(4, 4) * new double[] { 1, 2, 3, 4 };
            Assert.Equal(new double[] { 1, 2, 3, 4 }, M);
        }

        [Fact]
        public void TestTranspose()
        {
            var b = new RTF.Matrix(4, 4);
            b.SetRow(0, new double[] { 0, 9, 3, 0 });
            b.SetRow(1, new double[] { 9, 8, 0, 8 });
            b.SetRow(2, new double[] { 1, 8, 5, 3 });
            b.SetRow(3, new double[] { 0, 0, 5, 8 });

            var e = new RTF.Matrix(4, 4);
            e.SetColumn(0, new double[] { 0, 9, 3, 0 });
            e.SetColumn(1, new double[] { 9, 8, 0, 8 });
            e.SetColumn(2, new double[] { 1, 8, 5, 3 });
            e.SetColumn(3, new double[] { 0, 0, 5, 8 });

            Assert.Equal(e, b.Transpose());
        }

        [Fact]
        public void Determinant2x2()
        {
            var b = new RTF.Matrix(2, 2);
            b.SetRow(0, new double[] { 1, 5 });
            b.SetRow(1, new double[] { -3, 2 });

            Assert.Equal(17, b.Determinant());
        }

        [Fact]
        public void Submatrix3x3in2x2()
        {
            var b = new RTF.Matrix(3, 3);
            b.SetRow(0, new double[] { 1, 5, 0 });
            b.SetRow(1, new double[] { -3, 2, 7 });
            b.SetRow(2, new double[] { 0, 6, -3 });

            var e = new RTF.Matrix(2, 2);
            e.SetRow(0, new double[] { -3, 2 });
            e.SetRow(1, new double[] { 0, 6 });

            Assert.Equal(e, RTF.Matrix.Submatrix(b, 0, 2));
        }
        [Fact]
        public void Submatrix4x4in3x3()
        {
            var b = new RTF.Matrix(4, 4);
            b.SetRow(0, new double[] { -6, 1, 1, 6 });
            b.SetRow(1, new double[] { -8, 5, 8, 6, });
            b.SetRow(2, new double[] { -1, 0, 8, 2 });
            b.SetRow(3, new double[] { -7, 1, -1, 1 });

            var e = new RTF.Matrix(3, 3);
            e.SetRow(0, new double[] { -6, 1, 6 });
            e.SetRow(1, new double[] { -8, 8, 6 });
            e.SetRow(2, new double[] { -7, -1, 1 });

            Assert.Equal(e, RTF.Matrix.Submatrix(b, 2, 1));
        }
        [Fact]
        public void Minor3x3()
        {
            var e = new RTF.Matrix(3, 3);
            e.SetRow(0, new double[] { 3, 5, 0 });
            e.SetRow(1, new double[] { 2, -1, -7 });
            e.SetRow(2, new double[] { 6, -1, 5 });

            var b = RTF.Matrix.Submatrix(e, 1, 0);

            Assert.Equal(25, b.Determinant());
            Assert.Equal(25, RTF.Matrix.Minor(e, 1, 0));
        }

        [Fact]
        public void Cofactor3x3()
        {
            var e = new RTF.Matrix(3, 3);
            e.SetRow(0, new double[] { 3, 5, 0 });
            e.SetRow(1, new double[] { 2, -1, -7 });
            e.SetRow(2, new double[] { 6, -1, 5 });

            Assert.Equal(-12, RTF.Matrix.Minor(e, 0, 0));
            Assert.Equal(-12, RTF.Matrix.Cofactor(e, 0, 0));
            Assert.Equal(25, RTF.Matrix.Minor(e, 1, 0));
            Assert.Equal(-25, RTF.Matrix.Cofactor(e, 1, 0));
        }

        [Fact]
        public void Determinant3x3()
        {
            var e = new RTF.Matrix(3, 3);
            e.SetRow(0, new double[] { 1, 2, 6 });
            e.SetRow(1, new double[] { -5, 8, -4 });
            e.SetRow(2, new double[] { 2, 6, 4 });

            Assert.Equal(56, RTF.Matrix.Cofactor(e, 0, 0));
            Assert.Equal(12, RTF.Matrix.Cofactor(e, 0, 1));
            Assert.Equal(-46, RTF.Matrix.Cofactor(e, 0, 2));
            Assert.Equal(-196, RTF.Matrix.Determinant(e));
        }

        [Fact]
        public void Determinant4x4()
        {
            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new double[] { -2, -8, 3, 5 });
            e.SetRow(1, new double[] { -3, 1, 7, 3 });
            e.SetRow(2, new double[] { 1, 2, -9, 6 });
            e.SetRow(3, new double[] { -6, 7, 7, -9 });

            Assert.Equal(690, RTF.Matrix.Cofactor(e, 0, 0));
            Assert.Equal(447, RTF.Matrix.Cofactor(e, 0, 1));
            Assert.Equal(210, RTF.Matrix.Cofactor(e, 0, 2));
            Assert.Equal(51, RTF.Matrix.Cofactor(e, 0, 3));
            Assert.Equal(-4071, e.Determinant());
        }

        [Fact]
        public void InvertibleMatrix()
        {
            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new double[] { 6, 4, 4, 4 });
            e.SetRow(1, new double[] { 5, 5, 7, 6 });
            e.SetRow(2, new double[] { 4, -9, 3, -7 });
            e.SetRow(3, new double[] { 9, 1, 7, -6 });

            Assert.True(e.IsInvertible());
        }
        [Fact]
        public void NonInvertibleMatrix()
        {
            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new double[] { -4, 2, -2, -3 });
            e.SetRow(1, new double[] { 9, 6, 2, 6 });
            e.SetRow(2, new double[] { 0, -5, 1, -5 });
            e.SetRow(3, new double[] { 0, 0, 0, 0 });

            Assert.False(e.IsInvertible());
        }

        [Fact]
        public void Inverse()
        {
            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new double[] { -5, 2, 6, -8 });
            e.SetRow(1, new double[] { 1, -5, 1, 8 });
            e.SetRow(2, new double[] { 7, 7, -6, -7 });
            e.SetRow(3, new double[] { 1, -3, 7, 4 });

            double r1 = -160d / 532;
            double r2 = 105d / 532;
            var b = e.Inverse();

            Assert.Equal(532, e.Determinant());
            Assert.Equal(-160, RTF.Matrix.Cofactor(e, 2, 3));
            Assert.Equal(r1, b[3, 2]);
            Assert.Equal(105, RTF.Matrix.Cofactor(e, 3, 2));
            Assert.Equal(r2, b[2, 3]);

        }
        [Fact]
        public void Inverse2()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new double[] { 8, -5, 9, 2 });
            a.SetRow(1, new double[] { 7, 5, 6, 1 });
            a.SetRow(2, new double[] { -6, 0, 9, 6 });
            a.SetRow(3, new double[] { -3, 0, -9, -4 });

            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new double[] { -0.15385, -0.15385, -0.28205, -0.53846 });
            e.SetRow(1, new double[] { -0.07692, 0.12308, 0.02564, 0.03077 });
            e.SetRow(2, new double[] { 0.35897, 0.35897, 0.43590, 0.92308 });
            e.SetRow(3, new double[] { -0.69231, -0.69231, -0.76923, -1.92308 });

            var inverse = a.Inverse();

            Assert.Equal(e, inverse);
        }
        [Fact]
        public void Inverse3()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new double[] { 9, 3, 0, 9 });
            a.SetRow(1, new double[] { -5, -2, -6, -3 });
            a.SetRow(2, new double[] { -4, 9, 6, 4 });
            a.SetRow(3, new double[] { -7, 6, 6, 2 });

            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new double[] { -0.04074, -0.07778, 0.14444, -0.22222 });
            e.SetRow(1, new double[] { -0.07778, 0.03333, 0.36667, -0.33333 });
            e.SetRow(2, new double[] { -0.02901, -0.14630, -0.10926, 0.12963 });
            e.SetRow(3, new double[] { 0.17778, 0.06667, -0.26667, 0.33333 });

            var inverse = a.Inverse();
            
            Assert.Equal(e, inverse);
        }

        [Fact]
        public void MultiplyingProductByInverse()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new double[] { 3, -9, 7, 3 });
            a.SetRow(1, new double[] { 3, -8, 2, -9 });
            a.SetRow(2, new double[] { -4, 4, 4, 1 });
            a.SetRow(3, new double[] { -6, 5, -1, 1 });

            var b = new RTF.Matrix(4, 4);
            b.SetRow(0, new double[] { 8, 2, 2, 2 });
            b.SetRow(1, new double[] { 3, -1, 7, 0 });
            b.SetRow(2, new double[] { 7, 0, 5, 4 });
            b.SetRow(3, new double[] { 6, -2, 0, 5 });

            var c = a * b;
            var e = c * b.Inverse();

            Assert.True(e.Equals(a)
                , $"Expected : {a.ToString()}\nActual: {e.ToString()}");
        }

        [Fact]
        public void PuttingItTogether()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new double[] { 3, -9, 7, 3 });
            a.SetRow(1, new double[] { 3, -8, 2, -9 });
            a.SetRow(2, new double[] { -4, 4, 4, 1 });
            a.SetRow(3, new double[] { -6, 5, -1, 1 });

            var identity = RTF.Matrix.GetIdentity(4, 4);
            Assert.Equal(identity, identity.Inverse());

            var idInverted = a * a.Inverse();
            Assert.True(identity.Equals(idInverted)
                , $"Expected : {identity.ToString()}\nActual: {idInverted.ToString()}");

            var transposeInvert = a.Transpose().Inverse();
            var invertTranspose = a.Inverse().Transpose();
            Assert.True(transposeInvert.Equals(invertTranspose)
                , $"Expected : {transposeInvert.ToString()}\nActual: {invertTranspose.ToString()}");

            var modifiedIdentity = RTF.Matrix.GetIdentity(4, 4);
            modifiedIdentity[2, 2] = 6;
            var tuple = new double[] { 1, 2, 3, 4 };
            var tupleIdentity = identity * tuple;
            var newTuple = modifiedIdentity * tuple;

            Assert.Equal(tuple, tupleIdentity);
            Assert.NotEqual(tuple, newTuple);
        }
    }
}