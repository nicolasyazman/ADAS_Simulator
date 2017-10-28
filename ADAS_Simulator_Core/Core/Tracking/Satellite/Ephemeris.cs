using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.Core.Tracking.Satellite
{
    public class Ephemeris
    {
        public int EphemerisYear { get; }
        public int EphemerisMonth { get; }
        public int EphemerisDay { get; }
        public int EphemerisHour { get; }
        public int EphemerisMinute { get; }
        public double EphemerisSeconds { get; }

        public double SVClkBias { get; }
        public double SVClkDrift { get; }
        public double SVClkDriftRate { get; }

        // Broadcast orbit 1
        public double IODE { get; } // Issue of Data, Ephemeris 
        public double Crs { get; } // (meters)
        public double Deltan { get; } // (radians/sec)
        public double M0 { get; }  // (radians)

        // Broadcast orbit 2
        public double Cuc { get; } // (radians)
        public double e { get; }   // Eccentricity (unitless)
        public double Cus { get; } // (radians)
        public double sqrta { get; } // (sqrt(m))

        public double a { get; }

        // Broadcast orbit 3
        public double T0e { get; } // Time of Ephemeris (sec of GPS week)
        public double Cic { get; } // (radians)
        public double OMEGA { get; } // (radians)
        public double Cis { get; }   // (radians)

        // Broadcast orbit 4
        public double i0 { get; }  // (radians)
        public double Crc { get; } // (meters)
        public double omega { get; } // (radians)
        public double OmegaDot { get; } // (radians/sec)

        // Broadcast orbit 5
        public double IDot { get; } // (radians/sec)
        public double codesL2chan { get; }
        public int GPSWeek { get; } // to go with TOE, not mod(1024)
        public char L2Pdataflag { get; } // 

        // Broadcast orbit 6
        public double SVAcc { get; } // SV Accuracy (seconds)
        public double SVHealth { get; } // (bits 17-22 w 3 sf 1)
        public double TGD { get; } // (seconds)
        public double IODC { get; } // Issue of data (clock)

        // Broadcast orbit 7
        public int TrTime { get; } // Transmission time of message (sec of GPS week, derived e.g. from Z-count in Hand over word (HOW)
        public int FitInterval { get; } // (hours) (see ICD-GPS-200, 20.3.4.4) Zero if not known
        public double spare1 { get; }
        public double spare2 { get; }


    public Ephemeris(int EphemerisYear, int EphemerisMonth, int EphemerisDay, int EphemerisHour, int EphemerisMinute, double EphemerisSecs, double SVClkBias, double SVClkDrift, double SVClkDriftRate, double[] entries)
        {
            this.EphemerisYear = EphemerisYear;
            this.EphemerisMonth = EphemerisMonth;
            this.EphemerisDay = EphemerisDay;
            this.EphemerisHour = EphemerisHour;
            this.EphemerisMinute = EphemerisMinute;
            this.EphemerisSeconds = EphemerisSecs;

            IODE = entries[0];
            Crs = entries[1];
            Deltan = entries[2];
            M0 = entries[3];
            Cuc = entries[4];
            e = entries[5];
            Cus = entries[6];
            sqrta = entries[7];
            T0e = entries[8];
            Cic = entries[9];
            OMEGA = entries[10];
            Cis = entries[11];
            i0 = entries[12];
            Crc = entries[13];
            omega = entries[14];
            OmegaDot = entries[15];
            IDot = entries[16];
            codesL2chan = entries[17];
            GPSWeek = (int)entries[18];
            L2Pdataflag = (char)entries[19];
            SVAcc = entries[20];
            SVHealth = entries[21];
            TGD = entries[22];
            IODC = entries[23];
            TrTime = (int)entries[24];
            FitInterval = (int)entries[25];
            spare1 = entries[26];
            spare2 = entries[27];
        }
    }
}
