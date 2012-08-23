using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RemixBoard.Core.JobsWebSiteSeeker;

namespace RemixBoard.Core.Tests
{
    [TestFixture]
    public class JobsSeekersTests
    {
        private static void MockWebRequest(string remixJson, string expressJson) {
            var mockWebRequest = new Mock<WebRequestJson>();
            mockWebRequest.Setup(x => x.Get(Constantes.RemixJobUri)).Returns(remixJson);
            mockWebRequest.Setup(x => x.Get(Constantes.ExpressBoardUri)).Returns(expressJson);
            JobsSeeker.Instance.WebRequest = mockWebRequest.Object;
        }

        [Test]
        public void JobsSeekerInterrogeLes2Sites() {
            var mockWebRequest = new Mock<WebRequestJson>();
            mockWebRequest.Setup(x => x.Get(Constantes.RemixJobUri)).Returns(string.Empty);
            mockWebRequest.Setup(x => x.Get(Constantes.ExpressBoardUri)).Returns(string.Empty);
            JobsSeeker.Instance.WebRequest = mockWebRequest.Object;

            JobsSeeker.Instance.GetAll();

            mockWebRequest.Verify(x => x.Get(Constantes.ExpressBoardUri));
            mockWebRequest.Verify(x => x.Get(Constantes.RemixJobUri));
        }

        [Test]
        public void JobsSeekerRetourneTousLesjobs() {
            MockWebRequest(expressJson: @"{""jobs"":[{},{}]}", remixJson: @"{""jobs"":[{},{}]}");

            var jobs = JobsSeeker.Instance.GetAll();

            Assert.AreEqual(4, jobs.Count);
        }

        [Test]
        public void JobsSeekerNeCassePasSiUnFluxNeFonctionnePas() {
            MockWebRequest(expressJson: @"{""jobs"":[{},{}]}", remixJson: "");

            var jobs = JobsSeeker.Instance.GetAll();

            Assert.AreEqual(2, jobs.Count);
        }

        [Test]
        public void JobsSeekerPeurRafraichirEntrepotJobs() {
            MockWebRequest(expressJson: @"{""jobs"":[{}]}", remixJson: @"{""jobs"":[{}]}");
            var mockEntrepot = new Mock<IEntrepotJobs>();
            Entrepots.Jobs = mockEntrepot.Object;

            JobsSeeker.Instance.RefreshEntrepotJobs();

            IList<Job> jobsAttendus = JobsSeeker.Instance.GetAll();
            mockEntrepot.Verify(m => m.AddRange(jobsAttendus));
        }
    }
}