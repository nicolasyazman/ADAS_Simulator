using ADAS_Simulator_Core.Core.Tracking.Satellite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.IO.FileReaders
{
    public class RinexNavFileReader : RinexFileReader
    {
        public RinexNavFileReader(string path) : base(path)
        {

        }

        public RinexNavFileReader(StreamReader stream) : base(stream)
        {

        }

        public bool ParseSatellitesInformation(Constellation constel)
        {
            int PRN = -1;
            double[] SatInfo = new double[28];
            double[] clkVals = new double[3];
            int Year, Month, Day, Hour, Minute, Second;
            Year = Month = Day = Hour = Minute = 0;
            double Secs = 0.0;
            int CurOrbitInfo = 0;
            while (!this._stream.EndOfStream)
            {
                string line = _stream.ReadLine();

                string[] tmpSatElems = line.Split(' ').Where(str => str != string.Empty).ToArray();
                List<string> lsatElems = new List<string>();

                foreach (string satElem in tmpSatElems)
                {
                    MatchCollection m = Regex.Matches(satElem, @"[ -]?[0-9]+\.[0-9]+D[\+-]{1,1}[0-9]{2,2}");
                    
                    if (m.Count == 0)
                        lsatElems.Add(satElem);
                    else
                    {
                        foreach (Match curMatch in m)
                        {
                            if (curMatch.Index > 0 && !Regex.IsMatch(satElem.Substring(0, curMatch.Index), @"[ -]?[0-9]+\.[0-9]+D[\+-]{1,1}[0-9]{2,2}"))
                                lsatElems.Add(satElem.Substring(0, curMatch.Index));
                            lsatElems.Add(curMatch.Value);
                        }
                                                      
                    }
                        
                }
                    
                string[] satElems = lsatElems.Where(str => str != string.Empty).ToArray();
                
                if (satElems.Length == 10)
                {
                    PRN = int.Parse(satElems[0]);
                    if (!(PRN >= constel.FirstSatPRN && PRN <= constel.LastSatPRN))
                        return false;

                     Year = int.Parse(satElems[1]);
                     Month = int.Parse(satElems[2]);
                     Day = int.Parse(satElems[3]);
                     Hour = int.Parse(satElems[4]);
                     Minute = int.Parse(satElems[5]);
                     Secs = double.Parse(satElems[6], CultureInfo.InvariantCulture);
                    int i = 7;
                    clkVals = ParseAndGetScientificValues(ref i, satElems, 3);
                    if (null == clkVals)
                        return false;
                    CurOrbitInfo = 0;
                    Array.Clear(SatInfo, 0, 28);
                }
                else if (satElems.Length == 4)
                {
                    if (CurOrbitInfo > 7 || PRN == -1)
                        return false;

                    double[] parsedSatInfo;
                    int i = 0;
                    if (null == (parsedSatInfo = ParseAndGetScientificValues(ref i, satElems, 4)))
                        return false;
                    Array.Copy(parsedSatInfo, 0, SatInfo, CurOrbitInfo * 4, 4);
                    CurOrbitInfo++;
                    if (CurOrbitInfo == 7)
                        constel.Satellites[PRN - constel.FirstSatPRN].AddEphemeris(new Ephemeris(Year, Month, Day, Hour, Minute, Secs, clkVals[0], clkVals[1], clkVals[2], SatInfo));
                }
                else
                    return false;
            }
            return true;
        }
    }
}
