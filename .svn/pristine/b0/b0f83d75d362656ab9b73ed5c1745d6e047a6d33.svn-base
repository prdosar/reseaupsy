using Business;
using Library.Helper;
using ReseauPsy.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReseauPsy.ViewModel.Admin
{
    public class AppointmentRequestViewModel
    {
        public List<SelectListItem> Status { get; set; }
        public List<DbTableIdNameProperties> DeleteReasons { get; set; }
        public List<Availabilities> Availabilities { get; set; }
        public List<TherapistIdNameAvailabilities> Therapists { get; set; }
        public List<ClientAvailabilities> ClientAvailabilities { get; set; }
        public List<ClientLanguage> ClientLanguages { get; set; }
        public List<ClientConsultationTypeNameClient> ClientConsultationTypes { get; set; }
        public List<GetListClientForClientRequestPage_Result> Clients { get; set; }
        public List<ExternalAssociation> ExternalAssociation { get; set; }
        public int NbPage { get; set; }
        public int NbPagerPageShown { get; set; }


        public AppointmentRequestViewModel()
        {
            this.Status = new List<SelectListItem>
            {
                new  SelectListItem
                {
                    Text = Resources.Global.ToDo,
                    Value= "true"
                },
                new SelectListItem
                {
                    Text = Resources.Global.ToDoAlready,
                    Value= "false"
                }
            };

            this.Availabilities = new List<Availabilities>();
            this.Therapists = new List<TherapistIdNameAvailabilities>();
            this.ClientAvailabilities = new List<ClientAvailabilities>();
            this.ClientLanguages = new List<ClientLanguage>();
            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;
            this.NbPage = 1; //Since we filter by todo so all result will be on one page
        }

        public AppointmentRequestViewModel(ReseauPsyEntities _context)
            :this()
        {
            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";

            this.Clients = _context.GetListClientForClientRequestPage(
                true,
                isFrench, 
                null, 
                null,
                1,
                999_999).ToList();

            foreach (var client in Clients)
            {
                client.FirstName = client.FirstName.Decode();
                client.LastName = client.LastName.Decode();
            }
            
            this.ExternalAssociation = _context.ExternalAssociations.ToList();
           
            foreach (var therapist in _context.Therapists.Where(x => !x.IsDeleted))
                this.Therapists.Add(new TherapistIdNameAvailabilities(_context, therapist));

            foreach (var availabilitiy in _context.ClientAvailabilities)
                this.ClientAvailabilities.Add(new ClientAvailabilities(availabilitiy));

            this.ClientLanguages = _context.ClientLanguages.ToList();



            this.DeleteReasons = _context.ReasonsDeleteClientRequests
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? x.Reason : x.ReasonEN
                }).ToList();

            this.ClientConsultationTypes = _context.ClientConsultationTypes
                .Select(x => new ClientConsultationTypeNameClient
                { 
                    ConsultationTypeId = x.ConsultationTypeId,
                    ClientId = x.ClientId,
                    ConsultationTypeFR = x.ConsultationType.ConsultationType1,
                    ConsultationTypeEN = x.ConsultationType.ConsultationTypeEN
                }).ToList();


            var periodsDay = _context.DayOfTheWeek_PeriodOfTheDay.ToList();
            foreach (var day in _context.DaysOfTheWeeks.ToList())
            {
                this.Availabilities.Add(new Models.Availabilities
                {
                    Day = new DbTableIdNameProperties
                    { 
                        Id = day.Id,
                        Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? day.DayOfTheWeek : day.DayOfTheWeekEN
                    },

                    Periods = periodsDay
                    .Where(x => x.DaysOfTheWeekId == day.Id)
                    .Select(x => new DbTableIdNameProperties
                    { 
                        Id = x.Id,
                        Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? 
                            x.PeriodsOfTheDay.PeriodOfTheDay :
                            x.PeriodsOfTheDay.PeriodOfTheDayEN
                    }).ToList()
                });
            }


        }
    }
}