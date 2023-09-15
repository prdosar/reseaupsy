using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Therapist
{
    public class TherapistProfileDatas
    {
        public int TherapistId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int GenderId { get; set; }
        public int MaxWeeklyRequest { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int RegionId { get; set; }
        public int AccreditationId { get; set; }
        public string OpqNumber { get; set; }
        public string TpsNumber { get; set; }
        public string TvqNumber { get; set; }
        public List<int> ProfessionalTitlesIds { get; set; }
        public List<int> ClientTypeIds { get; set; }
        public List<int> ClientAgeIds { get; set; }
        public List<int> ConsultationTypesIds { get; set; }
        public List<int> LanguageIds { get; set; }
        public string Certification { get; set; }
        public List<int> AvailabilityIds { get; set; }
        public bool TakeAssuaranceClient { get; set; }
        public bool TakeThirdPartyClient { get; set; }
        public bool IsInterestedFormation { get; set; }

        public TherapistProfileDatas()
        {
            ProfessionalTitlesIds = new List<int>();
            ClientTypeIds = new List<int>();
            ClientAgeIds = new List<int>();
            LanguageIds = new List<int>();
            AvailabilityIds = new List<int>();
            ConsultationTypesIds = new List<int>();
        }
    }


}