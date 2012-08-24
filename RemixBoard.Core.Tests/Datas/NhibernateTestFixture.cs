using NUnit.Framework;
using RemixBoard.Datas;
using RemixBoard.Datas.Infrastructure;

namespace RemixBoard.Core.Tests.Datas
{
    public class NhibernateTestFixture
    {
        [TearDown]
        public void TearDown() {
            if (testTransationStrategie != null)
                testTransationStrategie.RollBack();
        }

        [TestFixtureSetUp]
        public void SetUpFixture() {
            NhSessionManagement.Initialize();
            nhEntrepotJobs = new NhEntrepotJobs();
            Entrepots.Jobs = nhEntrepotJobs;
            testTransationStrategie = new TestTransactionStrategie();
            nhEntrepotJobs.Transaction = testTransationStrategie;
        }

        private TestTransactionStrategie testTransationStrategie;
        private NhEntrepotJobs nhEntrepotJobs;
    }
}