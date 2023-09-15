using ReseauPsy.Controllers.Admin;
using ReseauPsy.Models.Admin;
using ReseauPsy.Models.Therapist;
using ReseauPsy.ViewModel.Admin;
using ReseauPsy.ViewModel.Therapist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class TherapistInformation
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? GenderId { get; set; }
        public DateTime HiringDate { get; set; }
        public int? MaxWeeklyRequest { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int? RegionId { get; set; }
        public int? AccreditationId { get; set; }
        public string OpqNumber { get; set; }
        public decimal HourlyWage { get; set; }
        public decimal ClientHourlyCost { get; set; }
        public bool IsTaxable { get; set; }
        public string Certification { get; set; }
        public string AdminNotes { get; set; }
        public bool? TakeClientFromAssuranceCompany { get; set; }
        public bool? TakeClientFromThirdPartyPayer { get; set; }
        public bool? IsInterestedInFormation { get; set; }
        public List<int> ProfessionalTitleIds { get; set; }
        public List<int> ClientTypeIds { get; set; }
        public List<int> ClientAgeRangeIds { get; set; }
        public List<int> ConsultationTypeIds { get; set; }
        public List<int> LanguageIds { get; set; }
        public List<int> Availabilities { get; set; }
        public List<string> Associations { get; set; }
        public List<string> OfferedServices { get; set; }
        public List<int> AreaOfPracticeIds { get; set; }
        public List<int> TheoreticalOrientationIds { get; set; }
        public List<int> SpecificSkillIds { get; set; }
        public string TpsNumber { get; set; }
        public string TvqNumber { get; set; }

        public TherapistInformation()
        {
            ProfessionalTitleIds = new List<int>();
            ClientTypeIds = new List<int>();
            ClientAgeRangeIds = new List<int>();
            ConsultationTypeIds = new List<int>();
            LanguageIds = new List<int>();
            Availabilities = new List<int>();
            Associations = new List<string>();
            OfferedServices = new List<string>();
            AreaOfPracticeIds = new List<int>();
            TheoreticalOrientationIds = new List<int>();
            SpecificSkillIds = new List<int>();
        }

        public TherapistInformation(DetailTherapistDatas datas)
            :this()
        {
            this.Id = datas.TherapistId;
            this.FirstName = datas.FirstName;
            this.LastName = datas.LastName;
            this.Email = datas.Email;
            this.PhoneNumber = datas.PhoneNumber;
            this.GenderId = datas.GenderId;
            this.HiringDate = datas.HiringDate;
            this.MaxWeeklyRequest = datas.MaxWeeklyRequest;
            this.Adress = datas.Adress;
            this.City = datas.City;
            this.PostalCode = datas.PostalCode;
            this.RegionId = datas.RegionId;
            this.AccreditationId = datas.AccreditationId;
            this.OpqNumber = datas.OpqNumber;
            this.HourlyWage = datas.HourlyWage;
            this.ClientHourlyCost = datas.ClientHourlyCost;
            this.IsTaxable = datas.IsTaxable;
            this.Certification = datas.Certification;
            this.AdminNotes = datas.AdminNotes;
            this.TakeClientFromAssuranceCompany = datas.TakeAssuaranceClient;
            this.TakeClientFromThirdPartyPayer = datas.TakeThirdPartyClient;
            this.IsInterestedInFormation = datas.IsInterestedFormation;
            this.ProfessionalTitleIds = datas.ProfessionalTitlesIds;
            this.ClientTypeIds = datas.ClientTypeIds;
            this.ClientAgeRangeIds = datas.ClientAgeIds;
            this.ConsultationTypeIds = datas.ConsultationTypesIds;
            this.LanguageIds = datas.LanguageIds;
            this.Availabilities = datas.AvailabilityIds;
            this.Associations = datas.Associations;
            this.OfferedServices = datas.OfferedServices;
            this.AreaOfPracticeIds = datas.PracticeSectorIds;
            this.TheoreticalOrientationIds = datas.OrientationIds;
            this.SpecificSkillIds = datas.SkillIds;
            this.TpsNumber = datas.TpsNumber;
            this.TvqNumber = datas.TvqNumber;
        }

        public TherapistInformation(TherapistProfileDatas datas)
            : this()
        {
            this.Id = datas.TherapistId;
            this.FirstName = datas.FirstName;
            this.LastName = datas.LastName;
            this.Email = datas.Email;
            this.PhoneNumber = datas.PhoneNumber;
            this.GenderId = datas.GenderId;
            this.MaxWeeklyRequest = datas.MaxWeeklyRequest;
            this.Adress = datas.Adress;
            this.City = datas.City;
            this.PostalCode = datas.PostalCode;
            this.RegionId = datas.RegionId;
            this.AccreditationId = datas.AccreditationId;
            this.OpqNumber = datas.OpqNumber;
            this.Certification = datas.Certification;
            this.TakeClientFromAssuranceCompany = datas.TakeAssuaranceClient;
            this.TakeClientFromThirdPartyPayer = datas.TakeThirdPartyClient;
            this.IsInterestedInFormation = datas.IsInterestedFormation;
            this.ProfessionalTitleIds = datas.ProfessionalTitlesIds;
            this.ClientTypeIds = datas.ClientTypeIds;
            this.ClientAgeRangeIds = datas.ClientAgeIds;
            this.ConsultationTypeIds = datas.ConsultationTypesIds;
            this.LanguageIds = datas.LanguageIds;
            this.Availabilities = datas.AvailabilityIds;
            this.TpsNumber = datas.TpsNumber;
            this.TvqNumber = datas.TvqNumber;
        }
    }
}