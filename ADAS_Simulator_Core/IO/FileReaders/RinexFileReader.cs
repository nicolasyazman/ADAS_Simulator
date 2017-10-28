using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.IO.FileReaders
{
    public class RinexFileReader
    {
        public enum eRinexFileType { RNXNAVIGATIONFILE = 1 };

        private StreamReader _stream;

        public int RinexVersion { get; set; }
        public eRinexFileType RinexType { get; set; }
        public string Pgm { get; set; }
        public string RunBy { get; set; }
        public DateTime Data { get; set; }
        public string Comment { get; set; }
        public double[] IonAlpha { get; set; }
        public double[] IonBeta { get; set; }
        public double[] DeltaUTC { get; set; }
        public int LeapSeconds { get; set; }
        public bool HeaderStatus { get; }

        private string[] HeaderLabels = { "RINEX VERSION", "TYPE", "PGM", "RUN BY", "DATE", "COMMENT", "ION ALPHA", "ION BETA", "DELTA-UTC", "LEAP SECONDS", "END OF HEADER"};
        private enum eHeaderLabels  {UNKNOWNLABEL = -1, RINEXVERSION = 0, TYPE, PGM, RUNBY, DATE, COMMENT, IONALPHA, IONBETA, DELTAUTC, LEAPSECONDS, ENDOFHEADER};

        private Dictionary<string, string> _months;

        public RinexFileReader(StreamReader s)
        {
            _stream = s;
            InitMonthsDictionary();
            HeaderStatus = ReadHeader();
        }

        public RinexFileReader(string path)
        {
            _stream = new StreamReader(path);
            InitMonthsDictionary();
            HeaderStatus = ReadHeader();
        }

        private void InitMonthsDictionary()
        {
            _months = new Dictionary<string, string>()
            {
                {"JAN" , "01"},
                {"FEB" , "02"},
                {"MAR" , "03"},
                {"APR" , "04"},
                {"MAY" , "05"},
                {"JUN" , "06"},
                {"JUL" , "07"},
                {"AUG" , "08"},
                {"SEP" , "09"},
                {"OCT" , "10"},
                {"NOV" , "11"},
                {"DEC" , "12"},
            };
        }

        private eHeaderLabels GetHeaderLabelId(string s, ref int startIdx)
        {
            int i = 0, idxOfLabel;
            s = s.Substring(startIdx);
            foreach (string Label in HeaderLabels)
            {
                if ((idxOfLabel = s.IndexOf(Label)) >= 0)
                {
                    startIdx = idxOfLabel + Label.Length;
                    return (eHeaderLabels)i;
                }
                i++;
            }
            return eHeaderLabels.UNKNOWNLABEL;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns true if the header has been correctly formed, false otherwise.</returns>
        protected bool ReadHeader()
        {
            /*
             
                         2              NAVIGATION DATA                         RINEX VERSION / TYPE
CCRINEXN V1.6.0 UX CDDIS               21-MAR-17 10:30     PGM / RUN BY / DATE
IGS BROADCAST EPHEMERIS FILE COMMENT             
    0.7451D-08 -0.1490D-07 -0.5960D-07  0.1192D-06          ION ALPHA           
    0.9216D+05 -0.1147D+06 -0.1311D+06  0.7209D+06          ION BETA            
    0.931322574615D-09 0.355271367880D-14   233472     1930 DELTA-UTC: A0,A1,T,W
    17                                                      LEAP SECONDS
                                                            END OF HEADER
             
             
             */
            while (!_stream.EndOfStream)
            {
                string line = _stream.ReadLine();
                string[] lineElems = Regex.Split(line, "[ ]{2}");
                int nbLabels = line.Count((c) => { return c == '/'; });

                int startIdx = 0, idxOfLabel = 0;
                for (int i = 0; i < nbLabels; i++)
                { 
                    eHeaderLabels HeaderLabel = GetHeaderLabelId(line, ref idxOfLabel);
                    startIdx += idxOfLabel;
                    switch (HeaderLabel)
                    {
                        case eHeaderLabels.ENDOFHEADER: return true;

                        case eHeaderLabels.UNKNOWNLABEL: return false;

                        case eHeaderLabels.COMMENT: Comment = lineElems[i];
                            break;
                        case eHeaderLabels.DATE:
                            string[] splt = lineElems[i].Split('-');
                            string date = Regex.Replace(lineElems[i], "-[A-Za-z]{3}-", "-" + _months[splt[1]] + "-");
                            Data = DateTime.ParseExact(date, "DD-MM-YY HH:mm", null);
                            break;
                        default: continue;
                    }
                }

            }
            return false;
        }

        // Factory method
        public static RinexFileReader CreateAppropriateRinexFileReader(string path)
        {
            return new RinexFileReader(path);
        }
    }
}
