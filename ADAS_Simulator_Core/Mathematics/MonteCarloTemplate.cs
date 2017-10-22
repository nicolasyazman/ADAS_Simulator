using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.Mathematics
{
    public abstract class MonteCarloTemplate
    {
        
        public abstract int VarsCalcFunction(double[] vars, int varsCount, ref int currentTryNumber, int triesPerIteration, double[] bestGlobalLowerBounds, double[] bestGlobalUpperBounds);
        public abstract int ComputationFunction(double[] vars, int varsCount, ref double foundVal);
        
        // Return code -1 = MEMORY ERROR, 1 = SUCCESS
        protected int Compute(double[] vars, int varsCount, ref double res, ref int currentTryNumber, int triesPerIteration, double[] bestGlobalLowerBounds, double[] bestGlobalUpperBounds)
        {
            int retCode;
            if ((retCode = VarsCalcFunction(vars, varsCount,ref currentTryNumber, triesPerIteration, bestGlobalLowerBounds, bestGlobalUpperBounds)) != 1)
                return retCode;
            if ((retCode = ComputationFunction(vars, varsCount,ref res)) != 1)
                return retCode;
            return 1;
        }

    }
}
