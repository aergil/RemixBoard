using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RemixBoard.Core.JobsWebSiteSeeker
{
    public abstract class JobDeserializer
    {
        public abstract string SiteName { get; }
        public abstract string UriSite { get; }

        public IList<Job> Deserialize(string stringJson) {
            IList<Job> jobList = new List<Job>();

            try {
                var deserializedProduct = JsonConvert.DeserializeObject<JObject>(stringJson);
                var jobsToken = deserializedProduct.SelectToken("jobs");
                foreach (JToken job in jobsToken) {
                    try {
                        jobList.Add(CreateJob(job));
                    }
                    catch (Exception e) {
                        Log.Error(this, e.Message, e);
                    }
                }
            }
            catch (Exception ex) {
                Log.Error(this, string.Format("Le flux JSON provenant de {0} ne peut être déserialisé", SiteName), ex);
            }

            return jobList;
        }

        public abstract Job CreateJob(JToken job);
    }
}