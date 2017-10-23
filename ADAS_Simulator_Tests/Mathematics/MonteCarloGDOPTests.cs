using ADAS_Simulator_Core.Core.Tracking.Receiver;
using ADAS_Simulator_Core.Mathematics;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Tests.Mathematics
{
    [TestFixture]
    public class MonteCarloGDOPTests
    {
        [Test]
        public void Monte_Carlo_Instantiate_Then_Set_Values_Using_Setters_And_Getting_Satellites_LOS()
        {
            DOP dop = new DOP();
            dop.ExpectedDOPValue = (10);
            dop.ExpectedPrecision = 0.001;
            dop.ExpectedSatellitesNumber = 4;
            dop.MaxNumberIterations = 10;
            dop.TriesPerIteration = 100000;

            int NbOfTries = 0;
            
            int retCode = dop.GetSatellitesLOSFromExpectedGDOP(ref NbOfTries);

            dop.Results.Should().NotBeNull();

            retCode.Should().NotBe(-1);

            MonteCarloGDOP mcgdop = new MonteCarloGDOP();

            double foundVal = 0;
            mcgdop.ComputationFunction(dop.Results, 12, ref foundVal);

            foundVal.Should().BeInRange(9.999, 10.001);

        }

        /*
TEST(MonteCarloTests, Monte_Carlo_Using_Manually_Instantiated_Solver)
    {
        MonteCarloGDOP* solver = new MonteCarloGDOP();
        int currentTryNb = 0;
        double* Results = new double[12];
        memset(Results, 0, sizeof(double) * 12);

        testing::internal::CaptureStdout();
    MathsADAS::MonteCarloMethod(12, 10.0, 0.001, 100000, 10, Results, currentTryNb, solver);
	double foundVal = 0;
    solver->ComputationFunction(Results, 12, foundVal);
    std::string output = testing::internal::GetCapturedStdout();
    std::cout << output << std::endl;

#ifdef _DEBUG
	int i;
	for (i = 0; i< 4; i++)

        printf("[%f, %f, %f]\n", Results[i * 3], Results[i * 3 + 1], Results[i * 3 + 2]);

    printf("Found gdop = %f\n", foundVal);

    printf("Number of tries: %d\n", currentTryNb);
#endif

    delete solver;
    delete Results;
}
*/
    }
}
