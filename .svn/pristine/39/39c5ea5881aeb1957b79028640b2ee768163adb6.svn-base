using Business;
using Library.Helper;
using ReseauPsy.Controllers.Admin;
using ReseauPsy.Models;
using ReseauPsy.Models.Admin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReseauPsy.ViewModel.Admin
{
    public class TherapistListViewModel
    {
        public List<GetTherapistListForAdmin_Result> Therapists { get; set; }

        public List<SelectListItem> Status { get; set; }
        public string StatusSelectedValue { get; set; }

        public List<Region> Regions { get; set; }
        public int RegionIdSelectedValue { get; set; }

        public List<Gender> Genders { get; set; }
        public int GenderIdSelectedValue { get; set; }

        public List<SelectListItem> Availability { get; set; }
        public string AvailabilitySelectedValue { get; set; }

        public List<ClientType> ClientTypes { get; set; }
        public int ClientTypeIdSelectedvalue { get; set; }

        public List<ConsultationType> ConsultationTypes { get; set; }
        public int ConsultationTypeIdSelectedValue { get; set; }

        public List<ClientsAgeRange> ClientsAgeRange { get; set; }
        public int ClientAgeRangeIdSelectedValue { get; set; }

        public List<ProfessionalTitle> ProfessionalTitles { get; set; }
        public int ProfessionalTitleIdSelectedValue { get; set; }

        public List<Language> Languages { get; set; }
        public int LanguageIdSelectedValue { get; set; }

        public List<Models.OfferedService> OfferedServices { get; set; }
        public List<Models.Availabilities> WeekAvailability { get; set; }
        public int NbPage { get; set; }
        public int NbPagerPageShown { get; set; }










        public TherapistListViewModel()
        {
            Therapists = new List<GetTherapistListForAdmin_Result>();
            Regions = new List<Region>();
            ClientTypes = new List<ClientType>();
            ConsultationTypes = new List<ConsultationType>();
            ClientsAgeRange = new List<ClientsAgeRange>();
            ProfessionalTitles = new List<ProfessionalTitle>();
            Languages = new List<Language>();
            OfferedServices = new List<Models.OfferedService>();
            WeekAvailability = new List<Models.Availabilities>();

            Status = new List<SelectListItem>
            {
                new  SelectListItem
                {
                    Text = Resources.Global.Active,
                    Value= "active"
                },
                new SelectListItem
                {
                    Text = Resources.Global.Inactive,
                    Value= "inactive"
                }
            };

            Availability = new List<SelectListItem>
            {
                new  SelectListItem
                {
                    Text = Resources.Global.All,
                    Value= "all"
                },
                new  SelectListItem
                {
                    Text = Resources.Global.Available,
                    Value= "true"
                },
                new SelectListItem
                {
                    Text = Resources.Global.Full,
                    Value= "false"
                }
            };
        }

        public TherapistListViewModel(ReseauPsyEntities _context)
            : this()
        {
            var monday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var sunday = monday.AddDays(7);

            var therapists = _context.GetTherapistListForAdmin(
                false,
                monday,
                sunday,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                1,
                WebSiteProperties.NbResultPerPage)
                .ToList();



            foreach (var therapist in therapists)
            {
                if (therapist.MaxWeeklyRequest == null)
                {
                    therapist.MaxWeeklyRequest = 0;
                }

                therapist.FirstName = therapist.FirstName.Decode();
                therapist.LastName = therapist.LastName.Decode();
            }

            this.Therapists = therapists;


            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;
            var nbResult = _context.GetTherapistListForAdminCount(
                false,
                monday,
                sunday,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null).First();

            this.NbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(nbResult)
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );

            this.Regions = _context.Regions.ToList();
            this.Genders = _context.Genders.ToList();
            this.ClientTypes = _context.ClientTypes.ToList();
            this.ConsultationTypes = _context.ConsultationTypes.ToList();
            this.ClientsAgeRange = _context.ClientsAgeRanges.ToList();
            this.ProfessionalTitles = _context.ProfessionalTitles.ToList();
            this.Languages = _context.Languages.ToList();

            var offeredService_OfferedServiceType = _context.OfferedService_OfferedServiceType
                .ToList();

            var offeredService = offeredService_OfferedServiceType
                .Select(x => x.OfferedService)
                .ToList();

            foreach (var service in offeredService.Distinct())
            {
                var offeredService_OfferedServiceTypeDto = new Models.OfferedService();
                offeredService_OfferedServiceTypeDto.Service = new Models.DbTableIdNameProperties
                {
                    Id = service.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? service.OfferedService1 : service.OfferedServiceEN
                };

                offeredService_OfferedServiceTypeDto.ServiceTypes = offeredService_OfferedServiceType
                    .Where(x => x.OfferedServiceId == service.Id)
                    .Select(x => new Models.DbTableIdNameProperties
                    {
                        Id = x.Id,
                        Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? x.OfferedServiceType.ServiceType : x.OfferedServiceType.ServiceTypeEN
                    })
                    .ToList();

                this.OfferedServices.Add(offeredService_OfferedServiceTypeDto);
            }

            foreach (var day in _context.DaysOfTheWeeks.ToList())
            {
                this.WeekAvailability.Add(new Models.Availabilities
                {
                    Day = new Models.DbTableIdNameProperties
                    {
                        Id = day.Id,
                        Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? day.DayOfTheWeek : day.DayOfTheWeekEN
                    },

                    Periods = _context.DayOfTheWeek_PeriodOfTheDay
                    .Where(x => x.DaysOfTheWeekId == day.Id)
                    .Select(x => new Models.DbTableIdNameProperties
                    {
                        Id = x.Id,
                        Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? x.PeriodsOfTheDay.PeriodOfTheDay : x.PeriodsOfTheDay.PeriodOfTheDayEN
                    })
                    .ToList()
                });
            }
        }
    }
}