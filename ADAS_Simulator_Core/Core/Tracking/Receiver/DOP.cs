using ADAS_Simulator_Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.Core.Tracking.Receiver
{
    public class DOP
    {
        private int _ExpectedSatellitesNumber;

        public double ExpectedDOPValue { get; set; }
        public double ExpectedPrecision { get; set; }
        public int ExpectedSatellitesNumber { get { return _ExpectedSatellitesNumber; } set
            {
                if (value >= 4 && value <= 20)
                {
                    _ExpectedSatellitesNumber = value;
                    Results = new double[_ExpectedSatellitesNumber * 3];
                }
            } }

        public int MaxNumberIterations { get; set; }
        public int TriesPerIteration { get; set; }

        public double[] Results { get; private set;  }

        public DOP()
        {
            this.Results = new Double[ExpectedSatellitesNumber * 3];
        }

        public int GetSatellitesLOSFromExpectedGDOP(ref int NumberOfTries)
        {
            NumberOfTries = 0;
            for (int i = 0; i < ExpectedSatellitesNumber; i++)
                Results[i] = 0.0;
            MonteCarloGDOP solver = new MonteCarloGDOP();
            return solver.MonteCarloMethod(ExpectedSatellitesNumber * 3, ExpectedDOPValue, ExpectedPrecision, TriesPerIteration, MaxNumberIterations, Results, ref NumberOfTries); // TODO Add complementary arguments
        }
    }
}
