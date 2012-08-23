using Moq;
using NUnit.Framework;
using Newtonsoft.Json;
using RemixBoard.Core.JobsWebSiteSeeker;
using log4net;

namespace RemixBoard.Core.Tests
{
    [TestFixture]
    public class JobDeserializerTests
    {
        [Test]
        public void ErreurLoggéeSiDeserialisationImpossible() {
            var mockLog = new Mock<ILog>();
            Log.GeneralLogger = mockLog.Object;

            var mockJobDeserialiser = new Mock<JobDeserializer>();
            mockJobDeserialiser.Setup(x => x.SiteName).Returns("TestSiteName");
            const string stringJson = @"ERREUR";

            var jobs = mockJobDeserialiser.Object.Deserialize(stringJson);

            Assert.AreEqual(0, jobs.Count);
            mockLog.Verify(p => p.Error("Le flux JSON provenant de TestSiteName ne peut être déserialisé", It.IsAny<JsonReaderException>()));
        }

        [Test]
        public void DeserialiseLaListeDeJobs() {
            var mockJobDeserialiser = new Mock<JobDeserializer>();
            const string stringJson = @"{""jobs"":[{},{},{}]}";

            var jobs = mockJobDeserialiser.Object.Deserialize(stringJson);
            Assert.AreEqual(3, jobs.Count);
        }

        [Test]
        public void SiUnJobNePeutEtreCrééLeTraitementContinue() {
            var mockLog = new Mock<ILog>();
            Log.GeneralLogger = mockLog.Object;

            var mockJobDeserialiser = new Mock<JobDeserializer>();
            mockJobDeserialiser.Setup(x => x.CreateJob(@"{}")).Returns(new Job());
            mockJobDeserialiser.Setup(x => x.CreateJob(@"{""prop"": ""ERREUR""}")).Throws(new JobException("Erreur"));

            const string stringJson = @"{""jobs"":[{""prop"": ""ERREUR""},{}]}";

            var jobs = mockJobDeserialiser.Object.Deserialize(stringJson);

            mockLog.Verify(log => log.Error("Erreur", It.IsAny<JobException>()));
            Assert.AreEqual(1, jobs.Count);
        }
    }
}