using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.Mathematics
{
    public class MonteCarloGDOP : MonteCarloTemplate
    {
        private Random rand;

        public MonteCarloGDOP(Random r = null)
        {
            this.rand = r ?? new Random();
        }


        public override int VarsCalcFunction(double[] vars, int varsCount, ref int currentTryNumber, int triesPerIteration, double[] bestGlobalLowerBounds, double[] bestGlobalUpperBounds)
        {
            int idx;

            if (currentTryNumber <= triesPerIteration)
            {
                for (idx = 0; idx < (varsCount / 3); idx++)
                {
                    Vector v = Vector.GetRandomUnitVector(this.rand);
                    vars[idx * 3] = v.X;
                    vars[idx * 3 + 1] = v.Y;
                    vars[idx * 3 + 2] = v.Z;
                }
            }
            else
            {
                for (idx = 0; idx < (varsCount / 3); idx++)
                {
                    Vector v = Vector.GetRandomBoundedUnitVector(this.rand, Math.Min(bestGlobalLowerBounds[idx * 3], bestGlobalUpperBounds[idx * 3]), Math.Max(bestGlobalLowerBounds[idx * 3], bestGlobalUpperBounds[idx * 3]));
                    vars[idx * 3] = v.X;
                    vars[idx * 3 + 1] = v.Y;
                    vars[idx * 3 + 2] = v.Z;
                }
            }
            return 1;
        }

        public override int ComputationFunction(double[] vars, int varsCount, ref double foundVal)
        {
             int j;
             Matrix hMatrix, dopMatrix;

            hMatrix = new Matrix(varsCount / 3, 4);

             for (j = 0; j < varsCount / 3; j++)
             {
                 hMatrix[j,0] = vars[j * 3];
                 hMatrix[j,1] = vars[j * 3 + 1];
                 hMatrix[j,2] = vars[j * 3 + 2];
                 hMatrix[j,3] = 1.0;
             }

            dopMatrix = Matrix.Multiplication(hMatrix, hMatrix, false, true);

            double[] multLOS = Vector.Vectorize(dopMatrix).ToArray();
            double[] inv = new double[16];

             Matrix dopResMatrix = Matrix.Inverse(dopMatrix);
             foundVal = Math.Sqrt(dopResMatrix[0,0] + dopResMatrix[1,1] + dopResMatrix[2,2] + dopResMatrix[3,3]);
            
  
             return 1;
         }

    }
}
