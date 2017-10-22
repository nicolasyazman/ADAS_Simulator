using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.Mathematics
{
    public class MonteCarloGDOP : MonteCarloTemplate
    {
        public override int VarsCalcFunction(double[] vars, int varsCount, ref int currentTryNumber, int triesPerIteration, double[] bestGlobalLowerBounds, double[] bestGlobalUpperBounds)
        {
            int idx;

            if (currentTryNumber <= triesPerIteration)
            {
                for (idx = 0; idx < (varsCount / 3); idx++)
                {
                    Vector v = Vector.GetRandomUnitVector();
                    vars[idx * 3] = v.X;
                    vars[idx * 3 + 1] = v.Y;
                    vars[idx * 3 + 2] = v.Z;
                }
            }
            else
            {
                for (idx = 0; idx < (varsCount / 3); idx++)
                {
                    Vector v = Vector.GetRandomBoundedUnitVector(Math.Min(bestGlobalLowerBounds[idx * 3], bestGlobalUpperBounds[idx * 3]), Math.Max(bestGlobalLowerBounds[idx * 3], bestGlobalUpperBounds[idx * 3]));
                    vars[idx * 3] = v.X;
                    vars[idx * 3 + 1] = v.Y;
                    vars[idx * 3 + 2] = v.Z;
                }
            }
            return 1;
        }

        public override int ComputationFunction(double[] vars, int varsCount, ref double foundVal)
        {
            /* int i, j;
             double** hMatrix, **dopMatrix;

             if ((hMatrix = MyMatrix::init_matrix_double(varsCount / 3, 4)) == NULL)
                 return -1;

             for (j = 0; j < varsCount / 3; j++)
             {
                 hMatrix[j][0] = vars[j * 3];
                 hMatrix[j][1] = vars[j * 3 + 1];
                 hMatrix[j][2] = vars[j * 3 + 2];
                 hMatrix[j][3] = 1.0L;
             }

             if ((dopMatrix = MyMatrix::init_matrix_double(4, 4)) == NULL)
                 return -1;

             MyMatrix::matrix_multiplication_transpose(hMatrix, hMatrix, dopMatrix, varsCount / 3, 4, varsCount / 3, 4);
             double* multLOS = (double*)malloc(sizeof(double) * 16);
             double* inv = (double*)malloc(sizeof(double) * 16);
             for (i = 0; i < 4; i++)
             {
                 for (j = 0; j < 4; j++)
                 {
                     multLOS[i * 4 + j] = dopMatrix[i][j];
                 }
             }
             MyMatrix::MatrixInvert4(vars, inv);
             foundVal = sqrt(fabs((inv[0] + inv[5] + inv[10] + inv[15])));

             free(inv);
             free(multLOS);
             MyMatrix::free_matrix_double(&hMatrix, varsCount / 3);
             MyMatrix::free_matrix_double(&dopMatrix, 4);
             */
             return 1;
         }

    }
}
