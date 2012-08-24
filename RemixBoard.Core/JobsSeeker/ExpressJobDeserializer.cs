using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RemixBoard.Core.JobsWebSiteSeeker
{
    public class ExpressJobDeserializer : JobDeserializer
    {
        public override string SiteName {
            get { return "Express-Board"; }
        }

        public override string UriSite {
            get { return Constantes.ExpressBoardUri; }
        }

        public override Job CreateJob(JToken jobToken) {
            var job = new Job();

            job.Origine = SiteName;

            DateTime dateCréation;
            var resultParse = DateTime.TryParse((string) jobToken["postedAt"], out dateCréation);
            job.DateDeCréation = resultParse ?  dateCréation : Constantes.DefaultDateTime;

            job.Entreprise = (string) jobToken["company"];
            job.EntrepriseWebSite = (string) jobToken["company_url"];
            job.TypeDeContrat = (string) jobToken["contract"];

            job.Expérience = (string) jobToken["experience"];
            job.Etudes = string.Empty;
            job.Titre = (string) jobToken["title"];
            var instructions = (string) jobToken["instructions"];
            var salary = (string) jobToken["salary"];
            job.Description = string.Format("{0}<br/>{1}", instructions, salary);

            job.Url = (string) jobToken["url"];

            var city = (string) jobToken["city"];
            var area = (string) jobToken["area"];
            job.Localisation = string.Format("{0}, {1}", city, area);

            var tagsToken = jobToken["tags"];
            if (tagsToken != null) {
                job.Tags = tagsToken.ToObject<List<string>>();
            }

            return job;
        }
    }
}