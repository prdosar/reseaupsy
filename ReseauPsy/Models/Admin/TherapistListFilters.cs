using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Admin
{
    public class TherapistListFilters
    {
        public bool IsDeleted { get; set; }
        public int? RegionId { get; set; }
        public int? GenderId { get; set; }
        public bool? IsAvailable { get; set; }
        public int? ClientTypeId { get; set; }
        public int? ConsultationTypeId { get; set; }
        public int? ClientAgeRangeId { get; set; }
        public int? ProfessionalTitleId { get; set; }
        public int? LanguageId { get; set; }
        public string OfferedServices { get; set; }
        public string WeekAvailabilities { get; set; }
        public int PageNumber { get; set; }
    }
}