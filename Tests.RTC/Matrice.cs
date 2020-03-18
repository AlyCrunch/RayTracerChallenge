using RTF = RayTracerChallenge.Features;
using Xunit;

namespace Tests.RTC
{
    public class Matrice
    {
        [Fact]
        public void ConstructAndInspectMatrice4x4()
        {
            var M = new RTF.Matrix(4, 4);
            M.SetRow(0, new decimal[] { 1, 2, 3, 4 });
            M.SetRow(1, new decimal[] { 5.5M, 6.5M, 7.5M, 8.5M });
            M.SetRow(2, new decimal[] { 9, 10, 11, 12 });
            M.SetRow(3, new decimal[] { 13.5M, 14.5M, 15.5M, 16.5M });

            Assert.Equal(1, M[0, 0]);
            Assert.Equal(4, M[0, 3]);
            Assert.Equal(5.5M, M[1, 0]);
            Assert.Equal(7.5M, M[1, 2]);
            Assert.Equal(11, M[2, 2]);
            Assert.Equal(13.5M, M[3, 0]);
            Assert.Equal(15.5M, M[3, 2]);
        }

        [Fact]
        public void OughtTo2x2()
        {
            var M = new RTF.Matrix(2, 2);
            M.SetColumn(0, new decimal[] { -3, 1 });
            M.SetColumn(1, new decimal[] { 5, -2 });

            Assert.Equal(-3, M[0, 0]);
            Assert.Equal(5, M[0, 1]);
            Assert.Equal(1, M[1, 0]);
            Assert.Equal(-2, M[1, 1]);
        }

        [Fact]
        public void OughtTo3x3()
        {
            var M = new RTF.Matrix(3, 3);
            M.SetRow(0, new decimal[] { -3, 5, 0 });
            M.SetRow(1, new decimal[] { 1, -2, 7 });
            M.SetRow(2, new decimal[] { 0, 1, 1 });

            Assert.Equal(-3, M[0, 0]);
            Assert.Equal(-2, M[1, 1]);
            Assert.Equal(1, M[2, 2]);
        }

        [Fact]
        public void EqualityTrue()
        {
            var M1 = new RTF.Matrix(4, 4);
            M1.SetRow(0, new decimal[] { 1, 2, 3, 4 });
            M1.SetRow(1, new decimal[] { 5, 6, 7, 8 });
            M1.SetRow(2, new decimal[] { 9, 8, 7, 6 });
            M1.SetRow(3, new decimal[] { 5, 4, 3, 2 });

            var M2 = new RTF.Matrix(4, 4);
            M2.SetRow(0, new decimal[] { 1, 2, 3, 4 });
            M2.SetRow(1, new decimal[] { 5, 6, 7, 8 });
            M2.SetRow(2, new decimal[] { 9, 8, 7, 6 });
            M2.SetRow(3, new decimal[] { 5, 4, 3, 2 });

            Assert.True(M1 == M2);
        }
        [Fact]
        public void EqualityTrueSmall()
        {
            var M1 = new RTF.Matrix(2, 2);
            M1.SetRow(0, new decimal[] { 0, 0 });
            M1.SetRow(1, new decimal[] { 1, 1 });

            var M2 = new RTF.Matrix(2, 2);
            M2.SetRow(0, new decimal[] { 0, 0 });
            M2.SetRow(1, new decimal[] { 1, 1 });

            Assert.True(M1 == M2);
        }
        [Fact]
        public void EqualityFalse()
        {
            var M1 = new RTF.Matrix(4, 4);
            M1.SetRow(0, new decimal[] { 1, 2, 3, 4 });
            M1.SetRow(1, new decimal[] { 5, 6, 7, 8 });
            M1.SetRow(2, new decimal[] { 9, 8, 7, 6 });
            M1.SetRow(3, new decimal[] { 5, 4, 3, 2 });

            var M2 = new RTF.Matrix(4, 4);
            M2.SetRow(0, new decimal[] { 2, 3, 4, 5 });
            M2.SetRow(1, new decimal[] { 6, 7, 8, 9 });
            M2.SetRow(2, new decimal[] { 8, 7, 6, 5 });
            M2.SetRow(3, new decimal[] { 4, 3, 2, 1 });

            Assert.False(M1 == M2);
        }
        [Fact]
        public void Multiply()
        {
            var M1 = new RTF.Matrix(4, 4);
            M1.SetRow(0, new decimal[] { 1, 2, 3, 4 });
            M1.SetRow(1, new decimal[] { 2, 3, 4, 5 });
            M1.SetRow(2, new decimal[] { 3, 4, 5, 6 });
            M1.SetRow(3, new decimal[] { 4, 5, 6, 7 });

            var M2 = new RTF.Matrix(4, 4);
            M2.SetRow(0, new decimal[] { 0, 1, 2, 4 });
            M2.SetRow(1, new decimal[] { 1, 2, 4, 8 });
            M2.SetRow(2, new decimal[] { 2, 4, 8, 16 });
            M2.SetRow(3, new decimal[] { 4, 8, 16, 32 });

            var M = M1 * M2;
            Assert.Equal(31, M[0, 1]);
        }
        [Fact]
        public void MultiplyTuple()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new decimal[] { 1, 2, 3, 4 });
            a.SetRow(1, new decimal[] { 2, 4, 4, 2 });
            a.SetRow(2, new decimal[] { 8, 6, 4, 1 });
            a.SetRow(3, new decimal[] { 0, 0, 0, 1 });

            decimal[] b = { 1, 2, 3, 1 };
            decimal[] e = { 18, 24, 33, 1 };

            var M = a * b;
            Assert.Equal(e, M);
        }

        [Fact]
        public void GetIdentity()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new decimal[] { 1, 0, 0, 0 });
            a.SetRow(1, new decimal[] { 0, 1, 0, 0 });
            a.SetRow(2, new decimal[] { 0, 0, 1, 0 });
            a.SetRow(3, new decimal[] { 0, 0, 0, 1 });

            var M = RTF.Matrix.GetIdentity(4, 4);
            Assert.Equal(a, M);
        }
        [Fact]
        public void MultiplyingIdentityByMatrice()
        {
            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new decimal[] { 0, 1, 2, 4 });
            e.SetRow(1, new decimal[] { 1, 2, 4, 8 });
            e.SetRow(2, new decimal[] { 2, 4, 8, 16 });
            e.SetRow(3, new decimal[] { 4, 8, 16, 32 });

            var M = RTF.Matrix.GetIdentity(4, 4) * e;
            Assert.Equal(e, M);
        }
        [Fact]
        public void MultiplyingIdentityByTuple()
        {
            var M = RTF.Matrix.GetIdentity(4, 4) * new decimal[] { 1, 2, 3, 4 };
            Assert.Equal(new decimal[] { 1, 2, 3, 4 }, M);
        }

        [Fact]
        public void TestTranspose()
        {
            var b = new RTF.Matrix(4, 4);
            b.SetRow(0, new decimal[] { 0, 9, 3, 0 });
            b.SetRow(1, new decimal[] { 9, 8, 0, 8 });
            b.SetRow(2, new decimal[] { 1, 8, 5, 3 });
            b.SetRow(3, new decimal[] { 0, 0, 5, 8 });

            var e = new RTF.Matrix(4, 4);
            e.SetColumn(0, new decimal[] { 0, 9, 3, 0 });
            e.SetColumn(1, new decimal[] { 9, 8, 0, 8 });
            e.SetColumn(2, new decimal[] { 1, 8, 5, 3 });
            e.SetColumn(3, new decimal[] { 0, 0, 5, 8 });

            Assert.Equal(e, b.Transpose());
        }

        [Fact]
        public void Determinant2x2()
        {
            var b = new RTF.Matrix(2, 2);
            b.SetRow(0, new decimal[] { 1, 5 });
            b.SetRow(1, new decimal[] { -3, 2 });

            Assert.Equal(17, b.Determinant());
        }

        [Fact]
        public void Submatrix3x3in2x2()
        {
            var b = new RTF.Matrix(3, 3);
            b.SetRow(0, new decimal[] { 1, 5, 0 });
            b.SetRow(1, new decimal[] { -3, 2, 7 });
            b.SetRow(2, new decimal[] { 0, 6, -3 });

            var e = new RTF.Matrix(2, 2);
            e.SetRow(0, new decimal[] { -3, 2 });
            e.SetRow(1, new decimal[] { 0, 6 });

            Assert.Equal(e, RTF.Matrix.Submatrix(b, 0, 2));
        }
        [Fact]
        public void Submatrix4x4in3x3()
        {
            var b = new RTF.Matrix(4, 4);
            b.SetRow(0, new decimal[] { -6, 1, 1, 6 });
            b.SetRow(1, new decimal[] { -8, 5, 8, 6, });
            b.SetRow(2, new decimal[] { -1, 0, 8, 2 });
            b.SetRow(3, new decimal[] { -7, 1, -1, 1 });

            var e = new RTF.Matrix(3, 3);
            e.SetRow(0, new decimal[] { -6, 1, 6 });
            e.SetRow(1, new decimal[] { -8, 8, 6 });
            e.SetRow(2, new decimal[] { -7, -1, 1 });

            Assert.Equal(e, RTF.Matrix.Submatrix(b, 2, 1));
        }
        [Fact]
        public void Minor3x3()
        {
            var e = new RTF.Matrix(3, 3);
            e.SetRow(0, new decimal[] { 3, 5, 0 });
            e.SetRow(1, new decimal[] { 2, -1, -7 });
            e.SetRow(2, new decimal[] { 6, -1, 5 });

            var b = RTF.Matrix.Submatrix(e, 1, 0);

            Assert.Equal(25, b.Determinant());
            Assert.Equal(25, RTF.Matrix.Minor(e, 1, 0));
        }

        [Fact]
        public void Cofactor3x3()
        {
            var e = new RTF.Matrix(3, 3);
            e.SetRow(0, new decimal[] { 3, 5, 0 });
            e.SetRow(1, new decimal[] { 2, -1, -7 });
            e.SetRow(2, new decimal[] { 6, -1, 5 });

            Assert.Equal(-12, RTF.Matrix.Minor(e, 0, 0));
            Assert.Equal(-12, RTF.Matrix.Cofactor(e, 0, 0));
            Assert.Equal(25, RTF.Matrix.Minor(e, 1, 0));
            Assert.Equal(-25, RTF.Matrix.Cofactor(e, 1, 0));
        }

        [Fact]
        public void Determinant3x3()
        {
            var e = new RTF.Matrix(3, 3);
            e.SetRow(0, new decimal[] { 1, 2, 6 });
            e.SetRow(1, new decimal[] { -5, 8, -4 });
            e.SetRow(2, new decimal[] { 2, 6, 4 });

            Assert.Equal(56, RTF.Matrix.Cofactor(e, 0, 0));
            Assert.Equal(12, RTF.Matrix.Cofactor(e, 0, 1));
            Assert.Equal(-46, RTF.Matrix.Cofactor(e, 0, 2));
            Assert.Equal(-196, RTF.Matrix.Determinant(e));
        }

        [Fact]
        public void Determinant4x4()
        {
            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new decimal[] { -2, -8, 3, 5 });
            e.SetRow(1, new decimal[] { -3, 1, 7, 3 });
            e.SetRow(2, new decimal[] { 1, 2, -9, 6 });
            e.SetRow(3, new decimal[] { -6, 7, 7, -9 });

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
            e.SetRow(0, new decimal[] { 6, 4, 4, 4 });
            e.SetRow(1, new decimal[] { 5, 5, 7, 6 });
            e.SetRow(2, new decimal[] { 4, -9, 3, -7 });
            e.SetRow(3, new decimal[] { 9, 1, 7, -6 });

            Assert.True(e.IsInvertible());
        }
        [Fact]
        public void NonInvertibleMatrix()
        {
            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new decimal[] { -4, 2, -2, -3 });
            e.SetRow(1, new decimal[] { 9, 6, 2, 6 });
            e.SetRow(2, new decimal[] { 0, -5, 1, -5 });
            e.SetRow(3, new decimal[] { 0, 0, 0, 0 });

            Assert.False(e.IsInvertible());
        }

        [Fact]
        public void Inverse()
        {
            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new decimal[] { -5, 2, 6, -8 });
            e.SetRow(1, new decimal[] { 1, -5, 1, 8 });
            e.SetRow(2, new decimal[] { 7, 7, -6, -7 });
            e.SetRow(3, new decimal[] { 1, -3, 7, 4 });

            decimal r1 = -160M / 532M;
            decimal r2 = 105M / 532M;
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
            a.SetRow(0, new decimal[] { 8, -5M, 9, 2 });
            a.SetRow(1, new decimal[] { 7, 5, 6, 1 });
            a.SetRow(2, new decimal[] { -6M, 0, 9, 6 });
            a.SetRow(3, new decimal[] { -3M, 0, -9M, -4M });

            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new decimal[] { -0.15385M, -0.15385M, -0.28205M, -0.53846M });
            e.SetRow(1, new decimal[] { -0.07692M, 0.12308M, 0.02564M, 0.03077M });
            e.SetRow(2, new decimal[] { 0.35897M, 0.35897M, 0.43590M, 0.92308M });
            e.SetRow(3, new decimal[] { -0.69231M, -0.69231M, -0.76923M, -1.92308M });

            Assert.True(e.Equals(a.Inverse(), 5));
        }
        [Fact]
        public void Inverse3()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new decimal[] { 9, 3, 0, 9 });
            a.SetRow(1, new decimal[] { -5, -2, -6, -3 });
            a.SetRow(2, new decimal[] { -4, 9, 6, 4 });
            a.SetRow(3, new decimal[] { -7, 6, 6, 2 });

            var e = new RTF.Matrix(4, 4);
            e.SetRow(0, new decimal[] { -0.04074M, -0.07778M, 0.14444M, -0.22222M });
            e.SetRow(1, new decimal[] { -0.07778M, 0.03333M, 0.36667M, -0.33333M });
            e.SetRow(2, new decimal[] { -0.02901M, -0.14630M, -0.10926M, 0.12963M });
            e.SetRow(3, new decimal[] { 0.17778M, 0.06667M, -0.26667M, 0.33333M });

            Assert.True(e.Equals(a.Inverse(), 5));
        }

        [Fact]
        public void MultiplyingProductByInverse()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new decimal[] { 3, -9, 7, 3 });
            a.SetRow(1, new decimal[] { 3, -8, 2, -9 });
            a.SetRow(2, new decimal[] { -4, 4, 4, 1 });
            a.SetRow(3, new decimal[] { -6, 5, -1, 1 });

            var b = new RTF.Matrix(4, 4);
            b.SetRow(0, new decimal[] { 8, 2, 2, 2 });
            b.SetRow(1, new decimal[] { 3, -1, 7, 0 });
            b.SetRow(2, new decimal[] { 7, 0, 5, 4 });
            b.SetRow(3, new decimal[] { 6, -2, 0, 5 });

            var c = a * b;
            var e = c * b.Inverse();

            Assert.True(e.Equals(a, 0)
                , $"Expected : {a.ToString()}\nActual: {e.ToString()}");
        }

        [Fact]
        public void PuttingItTogether()
        {
            var a = new RTF.Matrix(4, 4);
            a.SetRow(0, new decimal[] { 3, -9, 7, 3 });
            a.SetRow(1, new decimal[] { 3, -8, 2, -9 });
            a.SetRow(2, new decimal[] { -4, 4, 4, 1 });
            a.SetRow(3, new decimal[] { -6, 5, -1, 1 });

            var idInverted = a * a.Inverse();

            var transposeInvert = a.Transpose().Inverse();
            var invertTranspose = a.Inverse().Transpose();

            var identity = RTF.Matrix.GetIdentity(4, 4);
            Assert.Equal(identity, identity.Inverse());
            Assert.True(identity.Equals(idInverted, 0)
                , $"Expected : {identity.ToString()}\nActual: {idInverted.ToString()}");
            Assert.True(transposeInvert.Equals(invertTranspose, 0)
                , $"Expected : {transposeInvert.ToString()}\nActual: {invertTranspose.ToString()}");
        }
    }
}