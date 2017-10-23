using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using ADAS_Simulator_Core.Mathematics;

namespace ADAS_Simulator_Tests.Mathematics
{
    [TestFixture]
    class MatrixTests
    {

        [Test]
        public void MatrixMultiplicationPlusOperatorOverload()
        {

        }

        [Test]
        public void MatrixMultiplicationMultiplyOperatorOverloadScalar()
        {

        }
        [Test]
        public void MatrixMultiplicationMultiplyOperatorOverloadMatrix()
        {

        }

        [Test]
        public void MatrixMultiplicationNormalNormal()
        {
            Matrix a = new Matrix(2, 3);
            a[0, 0] = 10;
            a[0, 1] = 3;
            a[0, 2] = 8;
            a[1, 0] = 23;
            a[1, 1] = 2;
            a[1, 2] = 9;

            Matrix b = new Matrix(3, 2);
            b[0, 0] = 7;
            b[0, 1] = 5;
            b[1, 0] = 8;
            b[1, 1] = 9;
            b[2, 0] = 50;
            b[2, 1] = 3;

            Matrix res = Matrix.Multiplication(a, b);
            res[0, 0].Should().Be(494.0);
            res[0, 1].Should().Be(101.0);
            res[1, 0].Should().Be(627.0);
            res[1, 1].Should().Be(160.0);
        }

        [Test]
        public void MatrixMultiplicationNormalTranspose()
        {
            Matrix a = new Matrix(3, 2);
            a[0, 0] = 7;
            a[0, 1] = 5;
            a[1, 0] = 8;
            a[1, 1] = 9;
            a[2, 0] = 50;
            a[2, 1] = 3;

            Matrix b = new Matrix(3, 2);
            b[0, 0] = 10;
            b[0, 1] = 3;
            b[1, 0] = 23;
            b[1, 1] = 2;
             b[2, 0] = 5;
             b[2, 1] = 4;

            Matrix res = Matrix.Multiplication(a, b);
            res.Should().BeNull();

            res = Matrix.Multiplication(a, b, false, true);
            res.Should().NotBeNull();
            res.rows.Should().Be(3);
            res.cols.Should().Be(3);
            res[0, 0].Should().Be(85.0);
            res[0, 1].Should().Be(171.0);
            res[0, 2].Should().Be(55.0);
            res[1, 0].Should().Be(107.0);
            res[1, 1].Should().Be(202.0);
            res[1, 2].Should().Be(76.0);
            res[2, 0].Should().Be(509.0);
            res[2, 1].Should().Be(1156.0);
            res[2, 2].Should().Be(262.0);
        }

        [Test]
        public void MatrixMultiplicationTransposeNormal()
        {
            Matrix a = new Matrix(3, 2);
            a[0, 0] = 7;
            a[0, 1] = 5;
            a[1, 0] = 8;
            a[1, 1] = 9;
            a[2, 0] = 50;
            a[2, 1] = 3;

            Matrix b = new Matrix(3, 2);
            b[0, 0] = 10;
            b[0, 1] = 3;
            b[1, 0] = 23;
            b[1, 1] = 2;
            b[2, 0] = 5;
            b[2, 1] = 4;

            Matrix res = Matrix.Multiplication(a, b);
            res.Should().BeNull();

            res = Matrix.Multiplication(a, b, true, false);
            res.Should().NotBeNull();
            res.rows.Should().Be(2);
            res.cols.Should().Be(2);
            res[0, 0].Should().Be(504.0);
            res[0, 1].Should().Be(237.0);
            res[1, 0].Should().Be(272.0);
            res[1, 1].Should().Be(45.0);
        }

        [Test]
        public void MatrixMultiplicationTransposeTranspose()
        {
            Matrix a = new Matrix(3, 2);
            a[0, 0] = 7;
            a[0, 1] = 5;
            a[1, 0] = 8;
            a[1, 1] = 9;
            a[2, 0] = 50;
            a[2, 1] = 3;

            Matrix b = new Matrix(2, 3);
            b[0, 0] = 10;
            b[0, 1] = 23;
            b[0, 2] = 5;
            b[1, 0] = 3;
            b[1, 1] = 8;
            b[1, 2] = 4;

            Matrix res = Matrix.Multiplication(a, b, true, true);
            res.Should().NotBeNull();
            res.rows.Should().Be(2);
            res.cols.Should().Be(2);
            res[0, 0].Should().Be(504.0);
            res[0, 1].Should().Be(285.0);
            res[1, 0].Should().Be(272.0);
            res[1, 1].Should().Be(99.0);
        }


    }
}
