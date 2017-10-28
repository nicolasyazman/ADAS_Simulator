using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.Mathematics
{
    public class Vector
    {
        const double FORBIDDENVALUE = -2000000000.0;

        // Using nullable double is also a possibility, though cumbersome 
        public double X { get { if (Size >= 1) return Data[0]; else return FORBIDDENVALUE; } set { Data[0] = value; } }
        public double Y { get { if (Size >= 2) return Data[1]; else return FORBIDDENVALUE; } set { Data[1] = value; } }
        public double Z { get { if (Size >= 3) return Data[2]; else return FORBIDDENVALUE; } set { Data[2] = value; } }
        public double[] Data { get; set; }
        public int Size { get { return Data.Length; } }

        public Vector(double x)
        {
            Data = new double[1];
            X = x;
        }

        public Vector(double x, double y)
        {
            Data = new double[2];
            X = x;
            Y = y;
        }

        public Vector(double x, double y, double z)
        {
            Data = new double[3];
            X = x;
            Y = y;
            Z = z;
        }

        public Vector(int size)
        {
            Data = new double[size];
        }

        public Vector(double[] data, int numberOfElemsInVector = 0)
        {
            if (numberOfElemsInVector <= 0)
            {
                Data = new double[data.Length];
                Array.Copy(data, Data, data.Length);
            }
            else
            {
                Data = new double[numberOfElemsInVector];
                Array.Copy(data, Data, numberOfElemsInVector);
            }
                
        }

        public double GetNorm()
        {
            double sum = 0.0;
            for (int i = 0; i < Size; i++)
                sum += Data[i] * Data[i];
            return Math.Sqrt(sum);
        }

        public static double RandomFloat(double minimum, double maximum, Random random = null)
        {
            if (null == random)
               random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static Vector GetRandomUnitVector(Random random = null, double ? xsqrNullable = null)
        {
            
            double xsqr, ysqr, newvec_x, newvec_y, newvec_z;

            if (xsqrNullable.HasValue)
                xsqr = xsqrNullable.Value;
            else
                xsqr = RandomFloat(0.001, 0.999, random);

            ysqr = RandomFloat(0.001, 1 - xsqr, random);

            newvec_x = Math.Sqrt(xsqr);
            newvec_y = Math.Sqrt(ysqr);
            newvec_z = Math.Sqrt(1 - xsqr - ysqr);

            return new Vector(newvec_x, newvec_y, newvec_z);
        }


        public static Vector GetRandomBoundedUnitVector(Random random = null, double LowerXBound = 0.0, double UpperXBound = 1.0)
        {
            double xsqr;
            
            if (LowerXBound * LowerXBound > UpperXBound * UpperXBound)
            {
                double tmp = LowerXBound;
                LowerXBound = UpperXBound;
                UpperXBound = tmp;
            }
            xsqr = RandomFloat(LowerXBound * LowerXBound, UpperXBound * UpperXBound);
            xsqr *= xsqr; // Introducing this to search around x, remove this line if you want to search exactly in the boundary of x. Useful for Monte Carlo algorithm with correlated variables.

            return GetRandomUnitVector(random, xsqr);
        }

        public static Vector Vectorize(Matrix mat)
        {
            int totSize = mat.rows * mat.cols;
            Vector newVec = new Vector(totSize);
            for (int i = 0; i < mat.rows; i++)
                for (int j = 0; j < mat.cols; j++)
                    newVec[i * mat.cols + j] = mat[i, j];
            return newVec;
        }

        public double[] ToArray()
        {
            double[] array = new double[Size];
            Array.Copy(Data, array, Size);
            return array;
        }

        public double this[int i]
        {
            get
            {
                if (i > Size)
                    return FORBIDDENVALUE;
                return Data[i];
            }
            set
            {
                if (i <= Size)
                    Data[i] = value;
            }
        }
    }
}
