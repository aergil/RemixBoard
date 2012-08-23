using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RemixBoard.Core.JobsWebSiteSeeker
{
    public class RemixJobDeserializer : JobDeserializer
    {
        public override string SiteName {
            get { return "Remix-Job"; }
        }

        public override string UriSite {
            get { return Constantes.RemixJobUri; }
        }


        public override Job CreateJob(JToken jobToken) {
            var job = new Job();

            job.Origine = SiteName;

            DateTime? dateCréation = (DateTime?) jobToken["creation_time"];
            job.DateDeCréation = dateCréation.HasValue ? dateCréation.Value : Constantes.DefaultDateTime;

            job.Entreprise = (string) jobToken["company_name"];
            job.EntrepriseWebSite = (string) jobToken["company_website"];
            job.TypeDeContrat = (string) jobToken["contract_type"];

            var experience = (string)jobToken["experience"];
            if (!string.IsNullOrEmpty(experience) && experience == "NONE")
                experience = string.Empty;
            job.Expérience = experience;


            var etudes = (string) jobToken["study"];
            if (!string.IsNullOrEmpty(etudes) && etudes == "NONE")
                etudes = string.Empty;
            job.Etudes = etudes;
            job.Titre = (string) jobToken["title"];
            job.Description = (string) jobToken["description"];

            var urlToken = jobToken["short_url"];
            if (urlToken != null) {
                job.Url = (string) urlToken["short_url"];
            }

            var localisationToken = jobToken["geolocation"];
            if (localisationToken != null) {
                job.Localisation = (string) localisationToken["formatted_address"];
            }

            var tagsToken = jobToken["tags"];
            if (tagsToken != null) {
                job.Tags = tagsToken.ToObject<List<string>>();
            }

            return job;
        }
    }
}