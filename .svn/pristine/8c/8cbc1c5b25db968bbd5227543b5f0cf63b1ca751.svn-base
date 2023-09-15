using Business;
using Library.Helper;
using ReseauPsy.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ReseauPsy.ViewModel.Therapist
{
    public class TherapistClientRequestViewModel
    {
        public List<GetListClientRequestWaitingForTherapist_Result> Clients { get; set; }
        public List<ClientAvailability> ClientAvailabilities { get; set; }
        public List<ClientConsultationType> ClientConsultationTypes { get; set; }
        public List<ClientLanguage> ClientLanguages { get; set; }
        public List<Availabilities> Availabilities { get; set; }
        public List<int> TherapistAvailabilities { get; set; }


        public TherapistClientRequestViewModel()
        {
            Availabilities = new List<Availabilities>();
        }

        public TherapistClientRequestViewModel(ReseauPsyEntities _context, int therapistId)
            :this()
        {
            this.Clients = _context.GetListClientRequestWaitingForTherapist(therapistId).ToList();
            this.ClientAvailabilities = _context.ClientAvailabilities.ToList();
            this.ClientConsultationTypes = _context.ClientConsultationTypes.ToList();
            this.TherapistAvailabilities = _context.TherapistAvailabilities
                .Where(x => x.TherapistId == therapistId)
                .Select(x => x.DayOfTheWeek_PeriodOfTheDay_Id)
                .ToList();

            foreach (var client in Clients)
            {
                client.AdditionalMessageFromAdmin = client.AdditionalMessageFromAdmin.Decode();
                client.ClientName = client.ClientName.Decode();
                client.Message = client.Message.Decode();
            }

            this.ClientLanguages = _context.ClientLanguages.ToList();


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