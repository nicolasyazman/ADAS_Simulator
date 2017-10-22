using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.Mathematics
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double GetNorm()
        {
            return Math.Sqrt((X) * (X) + (Y) * (Y) + (Z) * (Z));
        }

        public static double RandomFloat(double minimum, double maximum, Random random = null)
        {
            if (null == random)
               random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static Vector GetRandomUnitVector(double? xsqrNullable = null, Random random = null)
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


        public static Vector GetRandomBoundedUnitVector(double LowerXBound = 0.0, double UpperXBound = 1.0, Random random = null)
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

            return GetRandomUnitVector(xsqr);
        }

    }
}
