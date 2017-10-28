using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.Core.Tracking.Satellite
{
    public class Constellation
    {
        public SatelliteBase[] Satellites { get; set; }
        public string Name { get; set; }
        public int SatNb { get; }
        public int FirstSatPRN { get; }
        public int LastSatPRN { get { return FirstSatPRN + SatNb; } }

        public Constellation(string name, int SatNb, int FirstSatPRN = 1)
        {
            Name = name;
            Satellites = new SatelliteBase[SatNb];
            for (int i = 0; i < SatNb; i++)
                Satellites[i] = new SatelliteBase(FirstSatPRN+i);
            this.SatNb = SatNb;
            this.FirstSatPRN = FirstSatPRN;
        }

        public static Constellation GetConstellationFromName(string ConstelName)
        {
            ConstelName = ConstelName.ToUpperInvariant();
            switch (ConstelName)
            {
                case "GPS": return new Constellation("GPS", 32);
                default: return null;
            }
        }
    }
}
