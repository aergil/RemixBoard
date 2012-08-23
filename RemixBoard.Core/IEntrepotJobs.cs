using System.Collections.Generic;

namespace RemixBoard.Core
{
    public interface IEntrepotJobs
    {
        void Add(Job job);
        Job GetById(int id);
        IList<Job> GetAll();
        void AddRange(IList<Job> jobs);
        IList<Job> Filtrer(string contrat, string ville, string[] mots);
        IList<Job> GetByFavoris();
        void Update(Job job);
    }
}