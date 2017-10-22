using ADAS_Simulator_Core.Mathematics;
using NUnit.Framework;
using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Tests.Mathematics
{
    [TestFixture]
    class VectorTests
    {


        [Test]
        public void RandomUnitVectorNormShouldBeOneOrNearOne()
        {
            Vector v = Vector.GetRandomUnitVector();
            double norm = v.GetNorm();

            norm.Should().BeGreaterOrEqualTo(0.999);
            norm.Should().BeLessOrEqualTo(1.001);
        }

        [Test]
        public void Random_Unit_Vectors_Coordinates_Should_Be_Between_0_And_1()
        {
            Vector v = Vector.GetRandomUnitVector();
            v.X.Should().BeGreaterOrEqualTo(0.0);
            v.Y.Should().BeGreaterOrEqualTo(0.0);
            v.Z.Should().BeGreaterOrEqualTo(0.0);
            v.X.Should().BeLessOrEqualTo(1.0);
            v.Y.Should().BeLessOrEqualTo(1.0);
            v.Z.Should().BeLessOrEqualTo(1.0);

        }
    }
        
}
