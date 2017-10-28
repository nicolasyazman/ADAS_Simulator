using ADAS_Simulator_Core.Core.Tracking.Satellite;
using ADAS_Simulator_Core.IO.FileReaders;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Tests.IO.FileReaders
{
    [TestFixture]
    public class RinexFileReaderTests
    {

        [Test]
        public void ParseRinexV2FileNavFileHeader()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\NavDataEphemerisIGSBroadcast_1_Jan_2017.nav";
            RinexFileReader rfr = new RinexFileReader(path);
            rfr.HeaderStatus.Should().Be(1);

            rfr.RinexVersion.Should().Be(2);
            rfr.RinexType.Should().Be(RinexFileReader.eRinexFileType.RNXNAVIGATIONFILE);
            rfr.Pgm.Should().Be("CCRINEXN V1.6.0 UX");
            rfr.RunBy.Should().Be("CDDIS");
            rfr.Date.Day.Should().Be(21);
            rfr.Date.Hour.Should().Be(10);
            rfr.Date.Minute.Should().Be(30);
            rfr.Date.Month.Should().Be(3);
            rfr.Date.Second.Should().Be(0);
            rfr.Date.Year.Should().Be(2017);
            rfr.Comment.Should().Be("IGS BROADCAST EPHEMERIS FILE");
            rfr.IonAlpha.Should().NotBeNullOrEmpty();
            rfr.IonAlpha[0].Should().Be(0.7451E-08);
            rfr.IonAlpha[1].Should().Be(-0.1490E-07);
            rfr.IonAlpha[2].Should().Be(-0.5960E-07);
            rfr.IonAlpha[3].Should().Be(0.1192E-06);
            rfr.IonBeta.Should().NotBeNullOrEmpty();
            rfr.IonBeta[0].Should().Be(0.9216E+05);
            rfr.IonBeta[1].Should().Be(-0.1147E+06);
            rfr.IonBeta[2].Should().Be(-0.1311E+06);
            rfr.IonBeta[3].Should().Be(0.7209E+06);
            rfr.DeltaAUTC.Should().NotBeNullOrEmpty();
            rfr.DeltaAUTC.Length.Should().Be(2);
            rfr.DeltaAUTC[0].Should().Be(0.931322574615E-09);
            rfr.DeltaAUTC[1].Should().Be(0.355271367880E-14);
            rfr.WeekNumber.Should().Be(1930);
            rfr.SecondsOfWeek.Should().Be(233472);
        }

        [Test]
        public void ParseRinexV2NavFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\NavDataEphemerisIGSBroadcast_1_Jan_2017.nav";
            RinexNavFileReader rfr = new RinexNavFileReader(path);

            Constellation gpsConstel = Constellation.GetConstellationFromName("gps");

            rfr.ParseSatellitesInformation(gpsConstel).Should().BeTrue();

            gpsConstel.Satellites[1].GetPosition(100);
            double[] pos = gpsConstel.Satellites[1].X;
            double ecefDist = new ADAS_Simulator_Core.Mathematics.Vector(pos, 3).GetNorm();
            double ecefDistInKm = ecefDist / 1000;
            ecefDistInKm.Should().BeInRange(26000, 27000);
        }
    }
}
