using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class DetailClientDatas
    {
        public int? ClientId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int RegionId { get; set; }
        public int GenderId { get; set; }
        public int TherapistId { get; set; }
        public decimal? TherapistHourlyWage { get; set; }
        public decimal? ClientHourlyCost { get; set; }
        public int CommunicationLanguageId { get; set; }
        public int ConsultationReasonId { get; set; }
        public int ClientAgeRangeId { get; set; }
        public List<int> AvailabilityIds { get; set; }
        public List<int> LanguageIds { get; set; }
        public List<int> ConsultationTypesIds { get; set; }

        public DetailClientDatas()
        {
            this.AvailabilityIds = new List<int>();
            LanguageIds = new List<int>();
            ConsultationTypesIds = new List<int>();
        }

    }
}