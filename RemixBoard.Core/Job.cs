using System;
using System.Collections.Generic;

namespace RemixBoard.Core
{
    public class Job
    {
        public virtual int Id { get; set; }
        public virtual string Origine { get; set; }

        public virtual string Description { get; set; }
        public virtual string Entreprise { get; set; }
        public virtual string EntrepriseWebSite { get; set; }
        public virtual string TypeDeContrat { get; set; }

        public virtual DateTime DateDeCréation {
            get { return dateDeCréation; }
            set { dateDeCréation = value; }
        }

        public virtual string Expérience {get; set; }

        public virtual string Etudes { get; set; }
        public virtual string Titre { get; set; }
        public virtual string Url { get; set; }
        public virtual string Localisation { get; set; }

        public virtual IList<string> Tags {
            get { return tags; }
            set { tags = value; }
        }

        public Boolean Favoris { get; set; }

        protected virtual bool Equals(Job other) {
            if (Id == other.Id)
                return true;
            return string.Equals(Titre, other.Titre)
                   && DateDeCréation.Equals(other.DateDeCréation)
                   && string.Equals(Origine, other.Origine);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (Titre != null ? Titre.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ DateDeCréation.GetHashCode();
                hashCode = (hashCode*397) ^ (Origine != null ? Origine.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Job left, Job right) {
            return Equals(left, right);
        }

        public static bool operator !=(Job left, Job right) {
            return !Equals(left, right);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Job) obj);
        }

        private IList<string> tags = new List<string>();
        private DateTime dateDeCréation = Constantes.DefaultDateTime;

 }
}