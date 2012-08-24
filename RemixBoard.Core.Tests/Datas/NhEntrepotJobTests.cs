using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RemixBoard.Datas;
using RemixBoard.Datas.Infrastructure;

namespace RemixBoard.Core.Tests.Datas
{
    [TestFixture]
    public class NhEntrepotJobTests : NhibernateTestFixture
    {
        [Test]
        public void TestNhSessionManagement() {
            NhSessionManagement.Initialize();
            var job = new Job { Entreprise = "entreprise", DateDeCréation = new DateTime(2012, 08, 18) };
            Job jobRetrouvé;

            using (var tx = NhSessionManagement.Session.BeginTransaction()) {
                NhSessionManagement.Session.SaveOrUpdate(job);
                NhSessionManagement.Session.Flush();
                jobRetrouvé = NhSessionManagement.Session.Get<Job>(job.Id);
                tx.Rollback();
            }

            Assert.AreEqual("entreprise", jobRetrouvé.Entreprise);
        }

        [Test]
        public void UnJobPeutÊtreAjoutéEtRetrouvé() {
            var dateDeCréation = new DateTime(2012, 08, 18);
            const string entreprise = "Haikara";
            const string description = "une description";
            const string etudes = "Bac";
            const string expérience = "2 ans";
            const string localisation = "Bordeaux";
            const string remixJob = "Remix-Job";
            const string developpeurAgile = "Développeur agile";
            const string typeDeContrat = "CDI";
            const string url = "http://www.google.fr";
            const bool favoris = true;
            const string entrepriseWebSite = "http://www.haikara.fr";

            var job = new Job {
                                  Entreprise = entreprise,
                                  DateDeCréation = dateDeCréation,
                                  Description = description,
                                  EntrepriseWebSite = entrepriseWebSite,
                                  Etudes = etudes,
                                  Expérience = expérience,
                                  Localisation = localisation,
                                  Origine = remixJob,
                                  Titre = developpeurAgile,
                                  TypeDeContrat = typeDeContrat,
                                  Url = url,
                                  Tags = new List<string> {".Net", "Ruby"},
                                  Favoris = favoris
                              };
            Entrepots.Jobs.Add(job);

            var jobRetrouvé = Entrepots.Jobs.GetById(job.Id);

            Assert.AreEqual(entreprise, jobRetrouvé.Entreprise);
            Assert.AreEqual(dateDeCréation, jobRetrouvé.DateDeCréation);
            Assert.AreEqual(description, jobRetrouvé.Description);
            Assert.AreEqual(entrepriseWebSite, jobRetrouvé.EntrepriseWebSite);
            Assert.AreEqual(etudes, jobRetrouvé.Etudes);
            Assert.AreEqual(expérience, jobRetrouvé.Expérience);
            Assert.AreEqual(localisation, jobRetrouvé.Localisation);
            Assert.AreEqual(remixJob, jobRetrouvé.Origine);
            Assert.AreEqual(developpeurAgile, jobRetrouvé.Titre);
            Assert.AreEqual(typeDeContrat, jobRetrouvé.TypeDeContrat);
            Assert.AreEqual(url, jobRetrouvé.Url);
            Assert.AreEqual(favoris, jobRetrouvé.Favoris);
            Assert.IsTrue(jobRetrouvé.Tags.Contains(".Net"));
            Assert.IsTrue(jobRetrouvé.Tags.Contains("Ruby"));
        }

        [Test]
        public void TousLesJobsSontRemontésParDateDécroissante()
        {
            var job1 = new Job{DateDeCréation =  new DateTime(2012, 08, 18)};
            var job2 = new Job{DateDeCréation =  new DateTime(2012, 08, 19)};
            var job3 = new Job{DateDeCréation =  new DateTime(2012, 08, 20)};
            var job4 = new Job{DateDeCréation =  new DateTime(2012, 08, 21)};

            Entrepots.Jobs.Add(job1);
            Entrepots.Jobs.Add(job2);
            Entrepots.Jobs.Add(job3);
            Entrepots.Jobs.Add(job4);

            IList<Job> jobs = Entrepots.Jobs.GetAll();

            Assert.AreEqual(4, jobs.Count);
            Assert.AreEqual(job4, jobs[0]);
            Assert.AreEqual(job3, jobs[1]);
            Assert.AreEqual(job2, jobs[2]);
            Assert.AreEqual(job1, jobs[3]);
        }
    
        [Test]
        public void LesJobsDejaEnBaseNeSontPasAjoutés() {
            var job = new Job {DateDeCréation = new DateTime(2012, 08, 18), Origine = "Remix", Titre = "titre"};
            Entrepots.Jobs.Add(job);

            var job2 = new Job { DateDeCréation = new DateTime(2012, 08, 18), Origine = "Remix", Titre = "titre"};
            Entrepots.Jobs.Add(job2);

            Assert.AreEqual(1, Entrepots.Jobs.GetAll().Count);
        }

        [Test]
        public void LesJobsPeuventEtreFiltrésParVille()
        {
            var job = new Job { Titre = "Developpeur", Localisation = "Bordeaux" };
            Entrepots.Jobs.Add(job);

            var job2 = new Job { Titre = "Scrum Master", Localisation = "Paris" };
            Entrepots.Jobs.Add(job2);

            Assert.AreEqual("Scrum Master", NhEntrepotJobs.JobQueryable.FiltrerParVille("Paris").ToList().First().Titre);
        }

        [Test]
        public void LesJobsPeuventEtreFiltrésParContratEtVilleEtMotsClef()
        {
            var job = new Job { Titre = "Developpeur", Localisation = "Bordeaux", TypeDeContrat = "CDI"};
            Entrepots.Jobs.Add(job);

            var job2 = new Job { Titre = "job attendu", Localisation = "Bordeaux", TypeDeContrat = "CDD" };
            Entrepots.Jobs.Add(job2);

            var job3 = new Job { Titre = "Scrum Master", Localisation = "Paris" };
            Entrepots.Jobs.Add(job3);

            Assert.AreEqual("job attendu", Entrepots.Jobs.Filtrer("CDD", "Bor", null)[0].Titre);
            Assert.AreEqual("job attendu", Entrepots.Jobs.Filtrer("", "", new[] { "job" })[0].Titre);
        }

        [Test]
        public void LesMotsClefsPortentSurLaDescription()
        {
            var jobNonPertinent1 = new Job { Titre = "job Non Pertinent 1", Localisation = "Bordeaux", TypeDeContrat = "CDI", Description = " une description simple"};
            Entrepots.Jobs.Add(jobNonPertinent1);

            var jobAttendu1 = new Job { Titre = "job attendu", Localisation = "Bordeaux", TypeDeContrat = "CDD", Description = " une description clef" };
            Entrepots.Jobs.Add(jobAttendu1);

            var jobAttendu2 = new Job { Titre = "job attendu 2", Localisation = "Bordeaux", TypeDeContrat = "CDD", Description = " une description mot" };
            Entrepots.Jobs.Add(jobAttendu2);

            var jobNonPertinent2 = new Job { Titre = "job Non Pertinent 2", Localisation = "Bordeaux", TypeDeContrat = "CDI" };
            Entrepots.Jobs.Add(jobNonPertinent2);

            var jobFiltrés = NhEntrepotJobs.JobQueryable.FiltrerParMotsClefs(new[] {"clef", "mot"}).ToList();
            Assert.IsFalse(jobFiltrés.Contains(jobNonPertinent1));
            Assert.IsFalse(jobFiltrés.Contains(jobNonPertinent2));
            Assert.IsTrue(jobFiltrés.Contains(jobAttendu1));
            Assert.IsTrue(jobFiltrés.Contains(jobAttendu2));
        }

        [Test]
        public void LesMotsClefsPortentSurLesTags()
        {
            var jobNonPertinent1 = new Job { Titre = "job Non Pertinent 1", Localisation = "Bordeaux", TypeDeContrat = "CDI", Description = " une description simple" };
            Entrepots.Jobs.Add(jobNonPertinent1);

            var jobAttendu1 = new Job { Titre = "job attendu", Localisation = "Bordeaux", TypeDeContrat = "CDD", Description = " une description" , 
                                        Tags = new List<string>(){"tag1", "mot"}};
            Entrepots.Jobs.Add(jobAttendu1);

            var jobAttendu2 = new Job { Titre = "job attendu 2", Localisation = "Bordeaux", TypeDeContrat = "CDD", Description = " une description",
                                        Tags = new List<string>(){"tag1", "motclef"}};
            Entrepots.Jobs.Add(jobAttendu2);

            var jobNonPertinent2 = new Job { Titre = "job Non Pertinent 2", Localisation = "Bordeaux", TypeDeContrat = "CDI",
                                             Tags = new List<string>() { "tag1", "tag2" }};
            Entrepots.Jobs.Add(jobNonPertinent2);

            var jobFiltrés = NhEntrepotJobs.JobQueryable.FiltrerParMotsClefs(new[] { "clef", "mot" }).ToList();
            Assert.IsFalse(jobFiltrés.Contains(jobNonPertinent1));
            Assert.IsFalse(jobFiltrés.Contains(jobNonPertinent2));
            Assert.IsTrue(jobFiltrés.Contains(jobAttendu1));
            Assert.IsTrue(jobFiltrés.Contains(jobAttendu2));
        }

        [Test]
        public void LesMotsClefsPortentSurLeTitre()
        {
            var jobNonPertinent1 = new Job { Titre = "job Non Pertinent 1", Localisation = "Bordeaux", TypeDeContrat = "CDI" };
            Entrepots.Jobs.Add(jobNonPertinent1);

            var jobAttendu1 = new Job { Titre = "job attendu", Localisation = "Bordeaux", TypeDeContrat = "CDD"};
            Entrepots.Jobs.Add(jobAttendu1);

            var jobAttendu2 = new Job { Titre = "job attendu 2", Localisation = "Bordeaux", TypeDeContrat = "CDD" };
            Entrepots.Jobs.Add(jobAttendu2);

            var jobNonPertinent2 = new Job { Titre = "job Non Pertinent 2", Localisation = "Bordeaux", TypeDeContrat = "CDI" };
            Entrepots.Jobs.Add(jobNonPertinent2);

            var jobFiltrés = NhEntrepotJobs.JobQueryable.FiltrerParMotsClefs(new[] { "attendu" }).ToList();
            Assert.IsFalse(jobFiltrés.Contains(jobNonPertinent1));
            Assert.IsFalse(jobFiltrés.Contains(jobNonPertinent2));
            Assert.IsTrue(jobFiltrés.Contains(jobAttendu1));
            Assert.IsTrue(jobFiltrés.Contains(jobAttendu2));
        }

        [Test]
        public void LesMotsClefsVidesNeSontPasUtilisés()
        {
            var jobNonPertinent1 = new Job { Titre = "job Non Pertinent 1", Localisation = "Bordeaux", TypeDeContrat = "CDI" };
            Entrepots.Jobs.Add(jobNonPertinent1);

            var jobAttendu1 = new Job { Titre = "job attendu", Localisation = "Bordeaux", TypeDeContrat = "CDD" };
            Entrepots.Jobs.Add(jobAttendu1);

            var jobFiltrés = NhEntrepotJobs.JobQueryable.FiltrerParMotsClefs(new[] {"", "attendu" }).ToList();
            Assert.IsFalse(jobFiltrés.Contains(jobNonPertinent1));
            Assert.IsTrue(jobFiltrés.Contains(jobAttendu1));

            jobFiltrés = NhEntrepotJobs.JobQueryable.FiltrerParMotsClefs(new[] { "", "" }).ToList();
            Assert.IsTrue(jobFiltrés.Contains(jobNonPertinent1));
            Assert.IsTrue(jobFiltrés.Contains(jobAttendu1));
        }

        [Test]
        public void LesJobsPeuventEtreFiltrésParTypeDeContrat()
        {
            var job = new Job { Titre = "Developpeur", TypeDeContrat = "CDI" };
            Entrepots.Jobs.Add(job);

            var job2 = new Job { Titre = "Scrum Master", TypeDeContrat = "CDD" };
            Entrepots.Jobs.Add(job2);

            Assert.AreEqual("Scrum Master", NhEntrepotJobs.JobQueryable.FiltrerParContrat("CDD").ToList().First().Titre);
        }

        [Test]
        public void LesJobsPeuventEtreFiltréParFavoris() {
            var job = new Job { Titre = "Developpeur", Favoris = false};
            Entrepots.Jobs.Add(job);

            var job2 = new Job { Titre = "Scrum Master", Favoris = true};
            Entrepots.Jobs.Add(job2);

            Assert.AreEqual("Scrum Master", Entrepots.Jobs.GetByFavoris()[0].Titre);
        }

        [Test]
        public void UnJobPeutEtreMisAJour()
        {
            var job = new Job { Titre = "Developpeur", Favoris = false };
            Entrepots.Jobs.Add(job);

            Job jobDeEntreprot = Entrepots.Jobs.GetById(job.Id);
            jobDeEntreprot.Favoris = true;
            Entrepots.Jobs.Update(jobDeEntreprot);

            Job jobUpdate =  Entrepots.Jobs.GetById(job.Id);

            Assert.AreEqual(true, jobUpdate.Favoris);
        }
    }
}