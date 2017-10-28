using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.Core.Tracking.Satellite
{
    public class SatelliteBase
    {
        const double GM = 3.986005e-14; // WGS-84 value for the product of gravitational constant G and the mass of the Earth M (m3 / s2)
        const double OmegaE = 7.292115e-5; // WGS-84 value of the Earth’s rotation rate (rad/s)
        const double M_PI = 3.14159265358979323846;
        
		public double[] X { get; set; } // state vector containing x, y, z, vx, vy, vz in cartesian coordinates (ECEF) (m,m,m,m/s,m/s,m/s)
        public int PRN { get; set; }        // Pseudo range number, unique SV identifier

        // Ephemeris data
        // Keplerian Parameters


        public List<Ephemeris> SatEphemeris { get; set; }

        private Ephemeris CurEphemeris;


        public SatelliteBase(int PRN)
        {
            this.PRN = PRN;
            this.SatEphemeris = new List<Ephemeris>();
            this.X = new double[6];
        }

        double ComputeClockOffset(double t, double t0, double ClockBias, double ClockDrift, double ClockDriftRate)
        {
            return ClockBias + ClockDrift * (t - t0) + ClockDriftRate * (t - t0) * (t - t0);
        }


        public void AddEphemeris(Ephemeris eph)
        {
            this.SatEphemeris.Add(eph);
        }


        public void GetPosition(int t)
        {
            CurEphemeris = SatEphemeris[0];

            double a = CurEphemeris.sqrta * CurEphemeris.sqrta;
            double tk;
            tk = ComputeClockOffset(t, CurEphemeris.T0e, CurEphemeris.SVClkBias, CurEphemeris.SVClkDrift, CurEphemeris.SVClkDriftRate);
            // TODO calculating tk


            double T = 2 * M_PI / Math.Sqrt(GM / (a * a * a));   // Satellite orbital period
            double n0 = Math.Sqrt(GM / (a * a * a));             // Computed mean motion
            double n = n0 + CurEphemeris.Deltan;                     // Corrected mean motion
            double Mk = CurEphemeris.M0 + n * tk;

            double Ek = Mk;                             // Kepler's equation of eccentric anomaly is solved by iteration
            int i;                                      // Because of the small eccentricity of GPS orbit (e < 0.001), two steps are usually sufficient 
            for (i = 0; i < 2; i++)
                Ek = Mk + CurEphemeris.e * Math.Sin(Ek);

            // double cosvk = Math.Cos(Ek - CurEphemeris.e) / (1 - CurEphemeris.e * Math.Cos(Ek)); // True anomaly
            //double vk = Math.Acos(cosvk);

            double vk = Math.Atan(Math.Sqrt(1 - CurEphemeris.e * CurEphemeris.e) * Math.Sin(Ek) / (Math.Cos(Ek) - CurEphemeris.e));

            double Phik = vk + CurEphemeris.omega;                               // Argument of latitude;
            double dUk = CurEphemeris.Cuc * Math.Cos(2 * Phik) + CurEphemeris.Cus * Math.Sin(2 * Phik); // Argument of latitude correction
            double dRk = CurEphemeris.Crc * Math.Cos(2 * Phik) + CurEphemeris.Crs * Math.Sin(2 * Phik); // Radius correction
            double dik = CurEphemeris.Cic * Math.Cos(2 * Phik) + CurEphemeris.Cis * Math.Sin(2 * Phik); // Inclination correction
            double uk = Phik + dUk;                                 // Corrected argument of latitude
            double rk = a * (1 - CurEphemeris.e * Math.Cos(Ek)) + dRk;                    // Corrected radius
            double ik = CurEphemeris.i0 + CurEphemeris.IDot * tk + dik;                       // Corrected inclination
            double Xprimek = rk * Math.Cos(uk);                          // Position in the orbital plane
            double Yprimek = rk * Math.Sin(uk);                          // Position in the orbital plane
            double Omegak = CurEphemeris.OMEGA + (CurEphemeris.OmegaDot - OmegaE) * tk - OmegaE * CurEphemeris.T0e;

            double Xk = Xprimek * Math.Cos(Omegak) - Yprimek * Math.Sin(Omegak) * Math.Cos(ik);
            double Yk = Xprimek * Math.Sin(Omegak) + Yprimek * Math.Cos(Omegak) * Math.Cos(ik);
            double Zk = Yprimek * Math.Sin(ik);

            this.X[0] = Xk;
            this.X[1] = Yk;
            this.X[2] = Zk;
        }



    }
}
