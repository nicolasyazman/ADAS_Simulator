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

        public int MonteCarloMethodMainLoop(double[] vars, int varsCount, double expected, double precision, ref int currentTryNb, int triesPerIteration, ref double bestUpperFoundVal, ref double bestLowerFoundVal, double[] bestLocalLowerBounds, double[] bestLocalUpperBounds, double[] bestGlobalLowerBounds, double[] bestGlobalUpperBounds)
        {
            int i, idx, retCode;
            double foundVal = 0.0;

            for (idx = 0; idx < varsCount; idx++)
            {
                bestLocalLowerBounds[idx] = -20000000.0;
                bestLocalUpperBounds[idx] = 20000000.0;
            }

            for (i = 0; i < triesPerIteration; i++)
            {
                if ((retCode = Compute(vars, varsCount, ref foundVal, ref currentTryNb, triesPerIteration, bestGlobalLowerBounds, bestGlobalUpperBounds)) != 1)
                    return retCode;
                currentTryNb++;

                if (foundVal > expected && foundVal < bestUpperFoundVal)
                {
                    bestUpperFoundVal = foundVal;
                    for (idx = 0; idx < varsCount; idx++)
                        bestLocalUpperBounds[idx] = vars[idx];
                }
                else if (foundVal < expected && foundVal > bestLowerFoundVal)
                {
                    bestLowerFoundVal = foundVal;
                    for (idx = 0; idx < varsCount; idx++)
                        bestLocalLowerBounds[idx] = vars[idx];

                }
                //printf("FOUNDVAL = %f\n", foundVal);
                if (foundVal > expected - precision && foundVal < expected + precision)
                {
                    int varId = 0;
                    for (varId = 0; varId < varsCount; varId++)
                    {
                        if (bestLocalLowerBounds[varId] > -20000000)
                            bestGlobalLowerBounds[varId] = bestLocalLowerBounds[varId];
                        if (bestLocalUpperBounds[varId] < 20000000)
                            bestGlobalUpperBounds[varId] = bestLocalUpperBounds[varId];
                    }
                #if DEBUG
                
				//println("Expected gdop = %lf with precision %lf. Found = %lf\n", expected, precision, (double)foundVal);
                #endif
                    return 1;
                }


            }
            int varIdx;
            for (varIdx = 0; varIdx < varsCount; varIdx++)
            {
                if (bestLocalUpperBounds[varIdx] < bestGlobalUpperBounds[varIdx])
                    bestGlobalUpperBounds[varIdx] = bestLocalUpperBounds[varIdx];
                if (bestLocalLowerBounds[varIdx] > bestGlobalLowerBounds[varIdx])
                    bestGlobalLowerBounds[varIdx] = bestLocalLowerBounds[varIdx];
            }

            //printf("Best lower gdop = %f, Best upper gdop = %f\n", bestGlobalLowerBounds[0], bestGlobalUpperBounds[0]);
            return 0;
        }


        public int MonteCarloMethod(int varsCount, double expected, double precision, int triesPerIteration, int nbOfIterations, double[] Results, ref int currentTryNb)
        {
            int i, res = 0;
            double[] bestLocalLowerBounds, bestLocalUpperBounds, bestGlobalLowerBounds, bestGlobalUpperBounds;
            double bestUpperFoundVal, bestLowerFoundVal;

            int correctNb = 0;
            
            bestLocalLowerBounds = new double[varsCount];
            bestLocalUpperBounds = new double[varsCount];
            bestGlobalLowerBounds = new double[varsCount];
            bestGlobalUpperBounds = new double[varsCount];

            int varIdx;
            for (varIdx = 0; varIdx < varsCount; varIdx++)
            {
                bestGlobalUpperBounds[varIdx] = 1;
                bestGlobalLowerBounds[varIdx] = 0;
            }
            bestUpperFoundVal = 20000000.0;
            bestLowerFoundVal = -20000000.0;
            i = res = 0;
            while (i < nbOfIterations && res != 1)
            {
                res = MonteCarloMethodMainLoop(Results, varsCount, expected, precision, ref currentTryNb, triesPerIteration, ref bestUpperFoundVal, ref bestLowerFoundVal, bestLocalLowerBounds, bestLocalUpperBounds, bestGlobalLowerBounds, bestGlobalUpperBounds);
                i++;
            }
            
            return res;
        }



    }
}
