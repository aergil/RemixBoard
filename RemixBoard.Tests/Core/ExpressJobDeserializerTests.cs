using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RemixBoard.Core;
using RemixBoard.Core.JobsWebSiteSeeker;

namespace RemixBoard.Tests.Core
{
    [TestFixture]
    public class ExpressJobDeserializerTests
    {
        private ExpressJobDeserializer expressJobDeserializer = new ExpressJobDeserializer();

        [Test]
        public void JobvientDeExpressBoard()
        {
            var jobToken = JToken.Parse(@"{ }");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("Express-Board", job.Origine);
        }

        [Test]
        public void JobContientExpérienceDemandée() {
            var jobToken = JToken.Parse(@"{ ""experience"":""2 à 5 ans""}");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("2 à 5 ans", job.Expérience);
        }

        [Test]
        public void ExpérienceNonObligatoire() {
            var jobToken = JToken.Parse(@"{ ""experience"":""""}");
            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(string.Empty, job.Expérience);

            jobToken = JToken.Parse(@"{}");
            job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(null, job.Expérience);
        }


        [Test]
        public void JobContientLaDateDeCréation() {
            var jobToken = JToken.Parse(@"{ ""postedAt"":""2012-08-03 12:28:41.0""}");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(new DateTime(2012, 08, 03, 12, 28, 41), job.DateDeCréation);
        }

        [Test]
        public void DateDeCréationEstObligatoire() {
            var jobToken = JToken.Parse(@"{}");
            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(Constantes.DefaultDateTime, job.DateDeCréation);
        }


        [Test]
        public void JobContientLeSiteWebDeEntreprise() {
            var jobToken = JToken.Parse(@"{ ""company_url"":""http://www.express-board.fr/user/334""}");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("http://www.express-board.fr/user/334", job.EntrepriseWebSite);
        }

        [Test]
        public void JobContientLeTypeDeContrat() {
            var jobToken = JToken.Parse(@"{ ""contract"":""CDI""}");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("CDI", job.TypeDeContrat);
        }

        [Test]
        public void JobContientSaDescription() {
            var jobToken = JToken.Parse(@"{ ""instructions"":""instruction"", ""salary"":""35000KEUR""}");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("instruction<br/>35000KEUR", job.Description);
        }

        [Test]
        public void JobNeContientPasLesEtudesDemandées() {
            var jobToken = JToken.Parse(@"{ }");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(string.Empty, job.Etudes);
        }

        [Test]
        public void JobContientUnTitre() {
            var jobToken = JToken.Parse(@"{ ""title"":""Consultant senior technique Java/Web""}");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("Consultant senior technique Java/Web", job.Titre);
        }

        [Test]
        public void JobContientUneEntreprise() {
            var jobToken = JToken.Parse(@"{ ""company"":""Jalios""}");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("Jalios", job.Entreprise);
        }

        [Test]
        public void JobContientUrlExpressBoard() {
            var jobToken = JToken.Parse(@"{ ""url"":""http://www.express-board.fr/offre-d-emploi/dc738bc2388b292b0138a4130273000b""}");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("http://www.express-board.fr/offre-d-emploi/dc738bc2388b292b0138a4130273000b", job.Url);
        }

        [Test]
        public void JobContientLocalisation() {
            var jobToken = JToken.Parse(@"{""city"":""Le Chesnay"",	""area"":""Ile-de-France""}");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("Le Chesnay, Ile-de-France", job.Localisation);
        }


        [Test]
        public void JobContientDesTags() {
            var jobToken = JToken.Parse(@"{""tags"":[""ActionScript"",""CodeIgniter""]} ");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(2, job.Tags.Count);
            Assert.IsTrue(job.Tags.Contains("ActionScript"));
            Assert.IsTrue(job.Tags.Contains("CodeIgniter"));
        }

        [Test]
        public void JobContientNeContientPasDeTag() {
            var jobToken = JToken.Parse(@"{} ");

            var job = expressJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(0, job.Tags.Count);
        }
    }
}