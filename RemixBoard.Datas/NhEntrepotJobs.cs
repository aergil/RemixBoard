using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using RemixBoard.Core;
using RemixBoard.Datas.Infrastructure;

namespace RemixBoard.Datas
{
    public static class EntrepotJobExtensions
    {
        public static IQueryable<Job> FiltrerParVille(this IQueryable<Job> query, string ville) {
            if (string.IsNullOrEmpty(ville))
                return query;
            return query.Where(j => j.Localisation.Contains(ville));
        }

        public static IQueryable<Job> FiltrerParContrat(this IQueryable<Job> query, string contrat) {
            if (string.IsNullOrEmpty(contrat))
                return query;
            return query.Where(j => j.TypeDeContrat.Contains(contrat));
        }

        public static IQueryable<Job> FiltrerParMotsClefs(this IQueryable<Job> query, string[] mots) {

            if (mots == null)
                return query;

            mots = mots.Where(m => !string.IsNullOrEmpty(m)).ToArray();
            if (mots.Length == 0)
                return query;

            var predicate = PredicateBuilder.False<Job>();

            foreach (string mot in mots) {
                var temp = mot;
                predicate = predicate.Or(p => p.Description.Contains(temp) || p.Titre.Contains(temp));
            }
            return query.Where(predicate);
        }

        public static IOrderedQueryable<Job> Ordonné(this IQueryable<Job> query) {
            return query.OrderByDescending(j => j.DateDeCréation);
        }
    }

    public class NhEntrepotJobs : IEntrepotJobs
    {
        public ITransactionStrategie Transaction {
            get { return transactionStrategie ?? (transactionStrategie = new RealTransactionStrategie()); }
            set { transactionStrategie = value; }
        }

        public static INHibernateQueryable<Job> JobQueryable {
            get { return NhSessionManagement.Session.Linq<Job>(); }
        }

        #region IEntrepotJobs Members

        public void Add(Job job) {
            try {
                Transaction.Begin();

                var jobDejaExistant = JobQueryable.Any(j => (j.Id == job.Id)
                                                            || (j.DateDeCréation == job.DateDeCréation
                                                                && j.Titre == job.Titre
                                                                && j.Origine == job.Origine));
                if (!jobDejaExistant)
                    NhSessionManagement.Session.SaveOrUpdate(job);

                Transaction.End();
            }
            catch(Exception ex) {
                Log.Error(this, ex.Message, ex);
                Transaction.RollBack();
            }
        }

        public Job GetById(int id) {
            var job = NhSessionManagement.Session.Get<Job>(id);

            return job;
        }

        public IList<Job> GetAll() {
            IList<Job> jobs = JobQueryable.OrderByDescending(j => j.DateDeCréation).ToList();
            
            return jobs;
        }

        public void AddRange(IList<Job> jobs) {
            foreach (var job in jobs) {
                Add(job);
            }
        }

        public IList<Job> Filtrer(string contrat, string ville, string[] mots) {
            IList<Job> jobs = JobQueryable
                .FiltrerParContrat(contrat)
                .FiltrerParVille(ville)
                .FiltrerParMotsClefs(mots)
                .Ordonné()
                .ToList();
            return jobs;
        }

        public IList<Job> GetByFavoris() {
            IList<Job> jobs = JobQueryable.Where(j => j.Favoris)
                .Ordonné()
                .ToList();

            return jobs;
        }

        public void Update(Job job) {
            try {
                Transaction.Begin();
                NhSessionManagement.Session.SaveOrUpdate(job);
                Transaction.End();
            }
            catch(Exception ex) {
                Log.Error(this, ex.Message, ex);
                Transaction.RollBack();
            }
        }

        #endregion

        private ITransactionStrategie transactionStrategie;
    }
}