using Business;
using Library.Helper;
using ReseauPsy.Controllers.Admin;
using ReseauPsy.Models;
using ReseauPsy.Models.Admin;
using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReseauPsy.ViewModel.Admin
{
    public class DetailsTherapistViewModel
    {
        public string ButtonAddEditText { get; set; }


        public int? TherapistId { get; set; }
        public SelectList Genders { get; set; }
        public SelectList Regions { get; set; }
        public SelectList Accreditations { get; set; }
        public List<ProfessionalTitle> ProfessionalTitles { get; set; }
        public List<ClientType> ClientTypes { get; set; }
        public List<ClientsAgeRange> ClientsAgeRanges { get; set; }
        public List<ConsultationType> ConsultationTypes { get; set; }
        public List<Language> Languages { get; set; }
        public List<DaysOfTheWeek> Days { get; set; }
        public List<Models.OfferedService> OfferedServices { get; set; }
        public List<DbTableIdNameProperties> AreasOfPractice { get; set; }
        public List<DbTableIdNameProperties> TheoreticalOrientations { get; set; }
        public List<DbTableIdNameProperties> SpecificSkills { get; set; }
        public List<DbTableIdNameProperties> ServiceName { get; set; }
        public List<DbTableIdNameProperties> ServiceTypeNames { get; set; }
        


        public string TherapistLastName { get; set; }
        public string TherapistFirstName { get; set; }
        public string TherapistEmail { get; set; }
        public string TherapistPhoneNumber { get; set; }
        public int? TherapistGenderId { get; set; }
        public string TherapistHiringDate { get; set; }
        public int? TherapistMaxWeeklyRequest { get; set; }
        public string TherapistAdress { get; set; }
        public string TherapistCity { get; set; }
        public string TherapistPostalCode { get; set; }
        public int? TherapistRegionId { get; set; }
        public int? TherapistAccreditationId { get; set; }
        public string TherapistOpqNumber { get; set; }
        public List<TherapistPayInformations> TherapistWages { get; set; }
        public bool TherapistIsTaxable { get; set; }
        public string TherapistTpsNumber { get; set; }
        public string TherapistTvqNumber { get; set; }
        public List<int> TherapistProfessionalTitleIds { get; set; }
        public List<int> TherapistClientTypeIds { get; set; }
        public List<int> TherapistClientsAgeRangeIds { get; set; }
        public List<int> TherapistConsultationTypeIds { get; set; }
        public List<int> TherapistLanguageIds { get; set; }
        public string TherapistCertification { get; set; }
        public string TherapistAdminNotes { get; set; }
        public List<int> TherapistAvailabilities { get; set; }
        public bool? TherapistTakeClientFromAssuranceCompany { get; set; }
        public bool? TherapistTakeClientFromThirdPayer { get; set; }
        public bool? TherapistIsInterestedForFormation { get; set; }
        public List<string> TherapistAssociation { get; set; }
        public List<string> TherapistService { get; set; }
        public List<int> TherapistAreasOfPracticeIds { get; set; }
        public List<int> TherapistTheoreticalOrientationIds { get; set; }
        public List<int> TherapistSpecificSkillIds { get; set; }




        public DetailsTherapistViewModel()
        {
            TherapistProfessionalTitleIds = new List<int>();
            TherapistClientTypeIds = new List<int>();
            TherapistClientsAgeRangeIds = new List<int>();
            TherapistConsultationTypeIds = new List<int>();
            TherapistLanguageIds = new List<int>();
            TherapistAvailabilities = new List<int>();
            TherapistAssociation = new List<string>();
            TherapistService = new List<string>();
            TherapistAreasOfPracticeIds = new List<int>();
            TherapistTheoreticalOrientationIds = new List<int>();
            TherapistSpecificSkillIds = new List<int>();

            this.Days = new List<DaysOfTheWeek>();
            this.OfferedServices = new List<Models.OfferedService>();
            this.AreasOfPractice = new List<DbTableIdNameProperties>();
            this.TheoreticalOrientations = new List<DbTableIdNameProperties>();
            this.SpecificSkills = new List<DbTableIdNameProperties>();
            this.ServiceName = new List<DbTableIdNameProperties>();
            this.ServiceTypeNames = new List<DbTableIdNameProperties>();
            this.TherapistWages = new List<TherapistPayInformations>();
        }

        public DetailsTherapistViewModel(ReseauPsyEntities _context)
            : this()
        {
            Genders = new SelectList(_context.Genders.ToList(), "Id", Database.Gender_Language);
            Regions = new SelectList(_context.Regions.ToList(), "Id", "Region1");
            Accreditations = new SelectList(_context.Accreditations.ToList(), "Id", Database.Accreditation_Language);

            ProfessionalTitles = _context.ProfessionalTitles.ToList();
            ClientTypes = _context.ClientTypes.ToList();
            ClientsAgeRanges = _context.ClientsAgeRanges.ToList();
            ConsultationTypes = _context.ConsultationTypes.ToList();
            Languages = _context.Languages.ToList();
            ButtonAddEditText = Global.Add;

            foreach (var item in _context.AreasOfPractices.ToList())
            {
                AreasOfPractice.Add(new DbTableIdNameProperties
                {
                    Id = item.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? item.AreaOfPractice : item.AreaOfPracticeEN
                });
            }

            foreach (var item in _context.TheoreticalOrientations.ToList())
            {

                TheoreticalOrientations.Add(new DbTableIdNameProperties
                {
                    Id = item.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? item.TheoreticalOrientation1 : item.TheoreticalOrientationEN
                });
            }

            foreach (var item in _context.SpecificSkills.ToList())
            {
                SpecificSkills.Add(new DbTableIdNameProperties
                {
                    Id = item.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? item.SpecificSkill1 : item.SpecificSkillEN
                });
            }

            foreach (var item in _context.OfferedServices.ToList())
            {
                ServiceName.Add(new DbTableIdNameProperties
                {
                    Id = item.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? item.OfferedService1 : item.OfferedServiceEN
                });
            }


            foreach (var item in _context.OfferedServiceTypes.ToList())
            { 
                ServiceTypeNames.Add(new DbTableIdNameProperties 
                {
                    Id = item.Id, 
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? item.ServiceType : item.ServiceTypeEN
                });
            }

            foreach (var service in _context.OfferedServices.ToList())
            {
                OfferedServices.Add(new Models.OfferedService
                {
                    Service = new DbTableIdNameProperties
                    {
                        Id = service.Id,
                        Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? service.OfferedService1 : service.OfferedServiceEN
                    },
                    ServiceTypes = _context.OfferedServiceTypes
                            .Select(x => new DbTableIdNameProperties
                            {
                                Id = x.Id,
                                Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? x.ServiceType : x.ServiceTypeEN
                            })
                            .ToList()
                });
            }

            this.Days = _context.DaysOfTheWeeks.ToList();
        }

        public DetailsTherapistViewModel(ReseauPsyEntities _context, Business.Therapist therapist)
            : this(_context)
        {

            
            this.ButtonAddEditText = Global.Edit;

            this.TherapistId = therapist.Id;
            this.TherapistHiringDate = therapist.HiringDate.ToString("dd-MM-yyyy");
            this.TherapistMaxWeeklyRequest = therapist.MaxWeeklyRequest;
            this.TherapistAdminNotes = therapist.AdminNotes.Decode();

            var getTherapistInfo = new GetTherapistInfo(_context, therapist.Id);
            getTherapistInfo.GetMostRecentTherapistInfo();

            this.TherapistLastName = getTherapistInfo.LastName.Decode();
            this.TherapistFirstName = getTherapistInfo.FirstName.Decode();
            this.TherapistEmail = getTherapistInfo.Email.Decode();
            this.TherapistPhoneNumber = getTherapistInfo.PhoneNumber.Decode();
            this.TherapistGenderId = getTherapistInfo.GenderId;
            this.TherapistAdress = getTherapistInfo.Adress.Decode();
            this.TherapistCity = getTherapistInfo.City.Decode();
            this.TherapistPostalCode = getTherapistInfo.PostalCode.Decode();
            this.TherapistRegionId = getTherapistInfo.RegionId;
            this.TherapistAccreditationId = getTherapistInfo.AccreditationId;
            this.TherapistOpqNumber = getTherapistInfo.OpqNumber.Decode();
            this.TherapistCertification = getTherapistInfo.CertificationAndSpecialities.Decode();
            this.TherapistTakeClientFromAssuranceCompany = getTherapistInfo.TakeClientFromAssuranceCompany;
            this.TherapistTakeClientFromThirdPayer = getTherapistInfo.TakeClientFromThirdPartyPayer;
            this.TherapistIsInterestedForFormation = getTherapistInfo.IsInterestedForFormation;
            this.TherapistTpsNumber = getTherapistInfo.TpsNumber.Decode();
            this.TherapistTvqNumber = getTherapistInfo.TvqNumber.Decode();
            this.TherapistIsTaxable = getTherapistInfo.IsTaxable;

            var getTherapistPayInformation = new GetTherapistPayInformation(_context, therapist.Id);
            this.TherapistWages = getTherapistPayInformation.GetActiveTherapistPayInformation();

            foreach (var association in getTherapistInfo.Associations)
            {
                this.TherapistAssociation.Add(association.AssociationName.Decode() + "--split--" + association.AssociationNumber.Decode());
            }

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


            this.TherapistAreasOfPracticeIds = _context.TherapistAreaOfPractices
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.AreaOfPracticeId)
                .ToList();

            this.TherapistTheoreticalOrientationIds = _context.TherapistTheoreticalOrientations
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.TheoreticalOrientationId)
                .ToList();

            this.TherapistSpecificSkillIds = _context.TherapistSpecificSkills
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.SpecificSkillId)
                .ToList();

            var therapistOfferedService_OfferedServiceTypes = _context.TherapistOfferedServices
                .Where(x => x.TherapistId == therapist.Id)
                .ToList();

            var therapistService = therapistOfferedService_OfferedServiceTypes
                .Select(x => x.OfferedService_OfferedServiceType.OfferedService)
                .ToList();

            foreach (var service in therapistService.Distinct())
            {
                string offeredService = service.Id.ToString();
                var serviceTypes = therapistOfferedService_OfferedServiceTypes
                    .Where(x => x.OfferedService_OfferedServiceType.OfferedServiceId == service.Id)
                    .ToList();

                foreach (var serviceType in serviceTypes)
                {
                    offeredService += "," + serviceType.OfferedService_OfferedServiceType.OfferedServiceTypeId;
                }

                this.TherapistService.Add(offeredService);
            }
        }
    }
}