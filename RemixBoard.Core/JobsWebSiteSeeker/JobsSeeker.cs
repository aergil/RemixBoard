using System.Collections.Generic;
using System.Threading;

namespace RemixBoard.Core.JobsWebSiteSeeker
{
    public class JobsSeeker
    {
        public static JobsSeeker Instance {
            get { return instance ?? (instance = new JobsSeeker()); }
        }

        public WebRequestJson WebRequest {
            get { return webRequest ?? (webRequest = new WebRequestJson()); }
            set { webRequest = value; }
        }

        protected JobsSeeker() {
            jobDeserializers = new List<JobDeserializer> {
                                                             new RemixJobDeserializer(),
                                                             new ExpressJobDeserializer()
                                                         };
        }

        public virtual IList<Job> GetAll() {
            var jobs = new List<Job>();

            foreach (var jobDeserializer in jobDeserializers) {
                Log.Info(this, string.Format("Récupération des données de {0}", jobDeserializer.SiteName));
                jobs.AddRange(jobDeserializer.Deserialize(WebRequest.Get(jobDeserializer.UriSite)));
            }

            return jobs;
        }

        public void RefreshEntrepotJobs() {
            Entrepots.Jobs.AddRange(GetAll());
        }

        private static JobsSeeker instance;

        private IList<JobDeserializer> jobDeserializers;
        private WebRequestJson webRequest;
    }
}