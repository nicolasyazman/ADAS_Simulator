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
        public void ParseRinexV2FileNavFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\NavDataEphemerisIGSBroadcast_1_Jan_2017.nav";
            RinexFileReader rfr = new RinexFileReader(path);
            rfr.HeaderStatus.Should().BeTrue();
        }
    }
}
