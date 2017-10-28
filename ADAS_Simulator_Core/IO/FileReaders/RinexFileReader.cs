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
    public class RinexFileReader
    {
        const int LabelsStartId = 60;
        const int LabelEndId = 80;

        public enum eRinexFileType { UNHANDLEDRINEXTYPE = -1, RNXNAVIGATIONFILE = 0 };

        protected StreamReader _stream;

        public int RinexVersion { get; set; }
        public eRinexFileType RinexType { get; set; }
        public string Pgm { get; set; }
        public string RunBy { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public double[] IonAlpha { get; set; }
        public double[] IonBeta { get; set; }

        // UTC Time values
        public double[] DeltaAUTC { get; set; }
        public int WeekNumber { get; set; }
        public int SecondsOfWeek { get; set; }

        public int LeapSeconds { get; set; }
        public int HeaderStatus { get; }

        private string[] HeaderLabels = { "RINEX VERSION", "TYPE", "PGM", "RUN BY", "DATE", "COMMENT", "ION ALPHA", "ION BETA", "DELTA-UTC", "LEAP SECONDS", "END OF HEADER"};
        private enum eHeaderLabels  {UNKNOWNLABEL = -1, RINEXVERSION = 0, TYPE, PGM, RUNBY, DATE, COMMENT, IONALPHA, IONBETA, DELTAUTC, LEAPSECONDS, ENDOFHEADER};

        private Dictionary<string, string> _months;
        private Dictionary<string, eRinexFileType> _rinexFileType;

        public RinexFileReader(StreamReader s)
        {
            _stream = s;
            InitDictionaries();
            HeaderStatus = ReadHeader();
        }

        public RinexFileReader(string path)
        {
            _stream = new StreamReader(path);
            InitDictionaries();
            HeaderStatus = ReadHeader();
        }

        private void InitDictionaries()
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

            _rinexFileType = new Dictionary<string, eRinexFileType>()
            {
                { "NAVIGATION DATA", eRinexFileType.RNXNAVIGATIONFILE }

            };
        }

        private eHeaderLabels GetHeaderLabelId(string s, int startSubstringSearch, ref int charsFromStartOfStringToEndOfLabel, out string headerLabelStr)
        {
            int i = 0, idxOfLabel, labelLen = 0;
            s = s.Substring(startSubstringSearch);
            foreach (string Label in HeaderLabels)
            {
                if ((idxOfLabel = s.IndexOf(Label)) >= 0)
                {
                    while (idxOfLabel + labelLen < s.Length && s[idxOfLabel + labelLen] != '/')
                        labelLen++;
                    charsFromStartOfStringToEndOfLabel = idxOfLabel + labelLen;
                    headerLabelStr = s.Substring(idxOfLabel, labelLen);
                    return (eHeaderLabels)i;
                }
                i++;
            }
            headerLabelStr = null;
            return eHeaderLabels.UNKNOWNLABEL;
        }

        protected double[] ParseAndGetScientificValues(ref int i, string[] lineElems, int expectedNumberOfValues)
        {
            double[] ionValsToFill = null;
            List<string> ionAlphaValues = new List<string>();
            while (ionAlphaValues.Count < expectedNumberOfValues && i < lineElems.Length)
            {
                string[] splitIAV = lineElems[i].Split(' ');
                
                ionAlphaValues.AddRange(splitIAV.Take(Math.Min(splitIAV.Length, expectedNumberOfValues)));
                if (ionAlphaValues.Count <= expectedNumberOfValues)
                    i++;
            }
            if (ionAlphaValues.Count != expectedNumberOfValues) return null;
            ionValsToFill = new double[expectedNumberOfValues];
            foreach (var item in ionAlphaValues.Select((val, j) => new { j, val }))
                ionValsToFill[item.j] = Double.Parse(item.val.Replace('D', 'E'), CultureInfo.InvariantCulture);
            return ionValsToFill;
        }

        private int ParseAndAssignLabelValue(eHeaderLabels HeaderLabel, ref int i, string[] lineElems, string HeaderLabelStr)
        {
            switch (HeaderLabel)
            {
                case eHeaderLabels.RINEXVERSION:
                    RinexVersion = int.Parse(lineElems[i]);
                    break;
                case eHeaderLabels.TYPE:
                    RinexType = _rinexFileType.ContainsKey(lineElems[i]) ? _rinexFileType[lineElems[i]] : eRinexFileType.UNHANDLEDRINEXTYPE;
                    if (RinexType == eRinexFileType.UNHANDLEDRINEXTYPE)
                        return -1;
                    break;

                case eHeaderLabels.PGM:
                    Pgm = lineElems[i];
                    break;
                case eHeaderLabels.RUNBY:
                    RunBy = lineElems[i];
                    break;

                case eHeaderLabels.IONALPHA:
                    if (null == (IonAlpha = ParseAndGetScientificValues(ref i, lineElems, 4)))
                        return -1;
                    break;

                case eHeaderLabels.IONBETA:
                    if (null == (IonBeta = ParseAndGetScientificValues(ref i, lineElems, 4)))
                        return -1;
                    break;

                case eHeaderLabels.DELTAUTC:
                    string[] timeFormat = HeaderLabelStr?.Split(':')?[1]?.Split(',') ?? null;
                    if (timeFormat == null) return -1;

                    if (null == (DeltaAUTC = ParseAndGetScientificValues(ref i, lineElems, 2)))
                        return -1;
                    foreach (string timeFormatElem in timeFormat)
                    {
                        string elem;
                        if ((elem = timeFormatElem.Trim()).Length == 0)
                            return -1;
                        switch (timeFormatElem.Trim())
                        {
                            case "A0": case "A1":
                                break;
                            case "T":
                                SecondsOfWeek = int.Parse(lineElems[i++].Trim());
                                break;
                            case "W":
                                WeekNumber = int.Parse(lineElems[i++].Trim());
                                break;
                            default: return -1;
                        }
                    }
                    break;

                case eHeaderLabels.UNKNOWNLABEL: return -1;

                case eHeaderLabels.COMMENT:
                    Comment = lineElems[i];
                    break;
                case eHeaderLabels.DATE:
                    string dateString = lineElems[i].Trim();
                    string format = "dd-MMM-yy hh:mm";
                    Date = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
                    break;


                case eHeaderLabels.ENDOFHEADER: return 1;

                default: return 0;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns true if the header has been correctly formed, false otherwise.</returns>
        protected int ReadHeader()
        {
            while (!_stream.EndOfStream)
            {
                string line = _stream.ReadLine();
                string[] lineElems = Regex.Split(line.Substring(0, LabelsStartId), "[ ]{2}").Where((val) => { return val.Trim() != String.Empty; }).ToArray();
                int nbLabels = line.Count((c) => { return c == '/'; })+1;
                int res;
                string headerLabelStr;

                int startIdx = LabelsStartId, lengthUntilEndLabel = 0;
                for (int i = 0; i < nbLabels; i++)
                {
                    eHeaderLabels HeaderLabel = GetHeaderLabelId(line, startIdx, ref lengthUntilEndLabel, out headerLabelStr);
                    startIdx += lengthUntilEndLabel;
                    if ((res = ParseAndAssignLabelValue(HeaderLabel, ref i, lineElems, headerLabelStr)) != 0)
                        return res;
                }

            }
            return -1;
        }

        // Factory method
        public static RinexFileReader CreateAppropriateRinexFileReader(string path)
        {
            return new RinexFileReader(path);
        }
    }
}
