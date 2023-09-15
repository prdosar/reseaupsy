using Business;
using Library.Helper;
using ReseauPsy.Controllers.Admin;
using ReseauPsy.Models;
using ReseauPsy.Models.Therapist;
using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReseauPsy.ViewModel.Therapist
{
    public class MyProfileViewModel
    {
        public Business.Therapist Therapist { get; set; }
        public SelectList Genders { get; set; }
        public SelectList Regions { get; set; }
        public SelectList Accreditations { get; set; }
        public List<ProfessionalTitle> ProfessionalTitles { get; set; }
        public List<ClientType> ClientTypes { get; set; }
        public List<ClientsAgeRange> ClientsAgeRanges { get; set; }
        public List<ConsultationType> ConsultationTypes { get; set; }
        public List<Language> Languages { get; set; }
        public bool IsApplyTaxes { get; set; }
        public bool IsInscriptionCompleted { get; set; }


        public List<DaysOfTheWeek> Days { get; set; }
        public string TherapistLastName { get; set; }
        public string TherapistFirstName { get; set; }
        public string TherapistEmail { get; set; }
        public string TherapistPhoneNumber { get; set; }
        public int? TherapistGenderId { get; set; }
        public DateTime? TherapistHiringDate { get; set; }
        public int? TherapistMaxWeeklyRequest { get; set; }
        public string TherapistAdress { get; set; }
        public string TherapistCity { get; set; }
        public string TherapistPostalCode { get; set; }
        public int? TherapistRegionId { get; set; }
        public int? TherapistAccreditationId { get; set; }
        public string TherapistOpqNumber { get; set; }
        public string TherapistTpsNumber { get; set; }
        public string TherapistTvqNumber { get; set; }
        public List<int> TherapistProfessionalTitleIds { get; set; }
        public List<int> TherapistClientTypeIds { get; set; }
        public List<int> TherapistClientsAgeRangeIds { get; set; }
        public List<int> TherapistConsultationTypeIds { get; set; }
        public List<int> TherapistLanguageIds { get; set; }
        public string TherapistCertification { get; set; }
        public List<int> TherapistAvailabilities { get; set; }
        public bool? TherapistTakeClientFromAssuranceCompany { get; set; }
        public bool? TherapistTakeClientFromThirdPayer { get; set; }
        public bool? TherapistIsInterestedForFormation { get; set; }



        public MyProfileViewModel()
        {
            TherapistProfessionalTitleIds = new List<int>();
            TherapistClientTypeIds = new List<int>();
            TherapistClientsAgeRangeIds = new List<int>();
            TherapistConsultationTypeIds = new List<int>();
            TherapistLanguageIds = new List<int>();
            TherapistAvailabilities = new List<int>();
            Days = new List<DaysOfTheWeek>();
        }

        public MyProfileViewModel(ReseauPsyEntities _context)
            :this()
        {
            Genders = new SelectList(_context.Genders.ToList(), "Id", Database.Gender_Language);
            Regions = new SelectList(_context.Regions.ToList(), "Id", "Region1");
            Accreditations = new SelectList(_context.Accreditations.ToList(), "Id", Database.Accreditation_Language);


            ProfessionalTitles = _context.ProfessionalTitles.ToList();
            ClientTypes = _context.ClientTypes.ToList();
            ClientsAgeRanges = _context.ClientsAgeRanges.ToList();
            ConsultationTypes = _context.ConsultationTypes.ToList();
            Languages = _context.Languages.ToList();
            Days = _context.DaysOfTheWeeks.ToList();

        }

        public MyProfileViewModel(ReseauPsyEntities _context, Business.Therapist therapist)
            : this(_context)
        {
            this.IsInscriptionCompleted = _context.VerifiyIfTherapistInscriptionCompleted(therapist.Id).First().IsInscriptionCompleted;
            this.TherapistHiringDate = therapist.HiringDate;
            this.TherapistMaxWeeklyRequest = therapist.MaxWeeklyRequest;

            var therapistInfo = new GetTherapistInfo(_context, therapist.Id);
            therapistInfo.GetMostRecentTherapistInfo();

            this.TherapistLastName = therapistInfo.LastName.Decode();
            this.TherapistFirstName = therapistInfo.FirstName.Decode();
            this.TherapistEmail = therapistInfo.Email.Decode();
            this.TherapistPhoneNumber = therapistInfo.PhoneNumber.Decode();
            this.TherapistGenderId = therapistInfo.GenderId;
            this.TherapistAdress = therapistInfo.Adress.Decode();
            this.TherapistCity = therapistInfo.City.Decode();
            this.TherapistPostalCode = therapistInfo.PostalCode.Decode();
            this.TherapistRegionId = therapistInfo.RegionId;
            this.TherapistAccreditationId = therapistInfo.AccreditationId;
            this.TherapistOpqNumber = therapistInfo.OpqNumber.Decode();
            this.TherapistCertification = therapistInfo.CertificationAndSpecialities.Decode();
            this.TherapistTakeClientFromAssuranceCompany = therapistInfo.TakeClientFromAssuranceCompany;
            this.TherapistTakeClientFromThirdPayer = therapistInfo.TakeClientFromThirdPartyPayer;
            this.TherapistIsInterestedForFormation = therapistInfo.IsInterestedForFormation;
            this.TherapistTpsNumber = therapistInfo.TpsNumber.Decode();
            this.TherapistTvqNumber = therapistInfo.TvqNumber.Decode();

            this.IsApplyTaxes = therapistInfo.IsTaxable;


            this.TherapistProfessionalTitleIds = _context.TherapistProfessionalTitles
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.ProfessionalTitleId)
                .ToList();

            this.TherapistClientTypeIds = _context.TherapistClientTypes
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.ClientTypeId)
                .ToList();

            this.TherapistClientsAgeRangeIds = _context.TherapistClientAgeRanges
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.ClientAgeRangeId)
                .ToList();

            this.TherapistConsultationTypeIds = _context.TherapistConsultationTypes
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.ConsultationTypeId)
                .ToList();

            this.TherapistLanguageIds = _context.TherapistLanguages
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.LanguageId)
                .ToList();

            this.TherapistAvailabilities = _context.TherapistAvailabilities
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.DayOfTheWeek_PeriodOfTheDay_Id)
                .ToList();
        }

        public MyProfileViewModel(ReseauPsyEntities _context, MyProfileViewModel viewModel)
            : this(_context)
        {
            this.TherapistLastName = viewModel.TherapistLastName;
            this.TherapistFirstName = viewModel.TherapistFirstName;
            this.TherapistEmail = viewModel.TherapistEmail;
            this.TherapistPhoneNumber = viewModel.TherapistPhoneNumber;
            this.TherapistGenderId = viewModel.TherapistGenderId;
            this.TherapistHiringDate = viewModel.TherapistHiringDate;
            this.TherapistMaxWeeklyRequest = viewModel.TherapistMaxWeeklyRequest;
            this.TherapistAdress = viewModel.TherapistAdress;
            this.TherapistCity = viewModel.TherapistCity;
            this.TherapistPostalCode = viewModel.TherapistPostalCode;
            this.TherapistRegionId = viewModel.TherapistRegionId;
            this.TherapistAccreditationId = viewModel.TherapistAccreditationId;
            this.TherapistOpqNumber = viewModel.TherapistOpqNumber;
            this.TherapistCertification = viewModel.TherapistCertification;
            this.TherapistTakeClientFromAssuranceCompany = viewModel.TherapistTakeClientFromAssuranceCompany;
            this.TherapistTakeClientFromThirdPayer = viewModel.TherapistTakeClientFromThirdPayer;
            this.TherapistIsInterestedForFormation = viewModel.TherapistIsInterestedForFormation;
            this.TherapistProfessionalTitleIds = viewModel.TherapistProfessionalTitleIds;
            this.TherapistClientTypeIds = viewModel.TherapistClientTypeIds;
            this.TherapistClientsAgeRangeIds = viewModel.TherapistClientsAgeRangeIds;
            this.TherapistConsultationTypeIds = viewModel.TherapistConsultationTypeIds;
            this.TherapistLanguageIds = viewModel.TherapistLanguageIds;
            this.TherapistAvailabilities = viewModel.TherapistAvailabilities;
            this.TherapistTpsNumber = viewModel.TherapistTpsNumber;
            this.TherapistTvqNumber = viewModel.TherapistTvqNumber;
            this.IsApplyTaxes = viewModel.IsApplyTaxes;
        }

    }

}