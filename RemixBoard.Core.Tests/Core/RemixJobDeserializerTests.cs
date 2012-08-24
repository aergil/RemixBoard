using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RemixBoard.Core.JobsWebSiteSeeker;

namespace RemixBoard.Core.Tests.Core
{
    [TestFixture]
    public class RemixJobDeserializerTests
    {
        private RemixJobDeserializer remixJobDeserializer = new RemixJobDeserializer();

        [Test]
        public void JobProvientDeRemixJob()
        {
            var jobToken = JToken.Parse(@"{ }");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("Remix-Job", job.Origine);
        }

        [Test]
        public void JobContientExpérienceDemandée() {
            var jobToken = JToken.Parse(@"{ ""experience"":""2 à 5 ans""}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("2 à 5 ans", job.Expérience);
        }

        [Test]
        public void ExpérienceNonObligatoire() {
            var jobToken = JToken.Parse(@"{ ""experience"":""""}");
            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(string.Empty, job.Expérience);

            jobToken = JToken.Parse(@"{}");
            job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(null, job.Expérience);
        }
        
        [Test]
        public void SiExpérienceEgaleNoneAlorsEmpty()
        {
            var jobToken = JToken.Parse(@"{ ""experience"":""NONE""}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(String.Empty, job.Expérience);
        }

        [Test]
        public void JobContientLaDateDeCréation() {
            var jobToken = JToken.Parse(@"{ ""creation_time"":""2012-08-10T12:59:36.0+02:00""}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(new DateTime(2012, 08, 10, 12, 59, 36), job.DateDeCréation);
        }

        [Test]
        public void DateDeCréationNonObligatoire() {
            var jobToken = JToken.Parse(@"{}");
            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(Constantes.DefaultDateTime,job.DateDeCréation);
        }

        [Test]
        public void JobContientLeSiteWebDeEntreprise() {
            var jobToken = JToken.Parse(@"{ ""company_website"":""http:\/\/www.haikara.fr""}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("http://www.haikara.fr", job.EntrepriseWebSite);
        }

        [Test]
        public void JobContientLeTypeDeContrat() {
            var jobToken = JToken.Parse(@"{ ""contract_type"":""CDI""}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("CDI", job.TypeDeContrat);
        }

        [Test]
        public void JobContientSaDescription() {
            var jobToken = JToken.Parse(@"{ ""description"":""une description""}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("une description", job.Description);
        }

        [Test]
        public void JobContientLesEtudesDemandées() {
            var jobToken = JToken.Parse(@"{ ""study"":""study""}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("study", job.Etudes);
        }
        
        [Test]
        public void SiEtudesDemandéesEgaleNoneAlorsEmpty()
        {
            var jobToken = JToken.Parse(@"{ ""study"":""NONE""}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(String.Empty, job.Etudes);
        }

        [Test]
        public void JobContientUnTitre() {
            var jobToken = JToken.Parse(@"{ ""title"":""Poste de développeur""}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("Poste de développeur", job.Titre);
        }

        [Test]
        public void JobContientUneEntreprise() {
            var jobToken = JToken.Parse(@"{ ""company_name"":""Entreprise""}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("Entreprise", job.Entreprise);
        }

        [Test]
        public void JobContientUrlRemixJob() {
            var jobToken = JToken.Parse(@"{ ""short_url"":{""short_url"":""http:\/\/rj.am\/00rl""}}");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("http://rj.am/00rl", job.Url);
        }

        [Test]
        public void JobContientLocalisation() {
            var jobToken = JToken.Parse(@"{""geolocation"":
				{
					""short_formatted_address"":""Paris\u00a0(75)"",
					""formatted_address"":""Paris, France"",
					""lat"":48.856614,""lng"":2.3522219
				}} ");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual("Paris, France", job.Localisation);
        }

        [Test]
        public void JobContientDesTags() {
            var jobToken = JToken.Parse(@"{""tags"":[""ActionScript"",""CodeIgniter""]} ");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(2, job.Tags.Count);
            Assert.IsTrue(job.Tags.Contains("ActionScript"));
            Assert.IsTrue(job.Tags.Contains("CodeIgniter"));
        }

        [Test]
        public void JobContientNeContientPasDeTag() {
            var jobToken = JToken.Parse(@"{""id"":10} ");

            var job = remixJobDeserializer.CreateJob(jobToken);
            Assert.AreEqual(0, job.Tags.Count);
        }
    }
}