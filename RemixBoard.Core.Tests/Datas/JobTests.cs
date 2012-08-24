using System;
using NUnit.Framework;

namespace RemixBoard.Core.Tests.Datas
{
    [TestFixture]
    public class JobTests
    {
        [Test]
        public void DeuxJobsSontEgauxSiMemeId() {
            var job1 = new Job {Id = 1};
            var job2 = new Job {Id = 1};

            Assert.AreEqual(job1, job2);
            Assert.IsTrue(job1 == job2);
            Assert.IsTrue(job1.Equals(job2));
        }

        [Test]
        public void DeuxJobsSontEgauxSiMemeOrigineEtDateCréationEtTitre() {
            var job1 = new Job {Origine = "Remix", DateDeCréation = new DateTime(2012, 08, 18), Titre = "Titre"};
            var job2 = new Job {Origine = "Remix", DateDeCréation = new DateTime(2012, 08, 18), Titre = "Titre"};

            Assert.AreEqual(job1, job2);
            Assert.IsTrue(job1 == job2);
            Assert.IsTrue(job1.Equals(job2));
        }
    }
}