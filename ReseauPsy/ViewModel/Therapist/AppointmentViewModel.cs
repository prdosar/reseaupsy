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
    public class AppointmentViewModel
    {
        public List<GetListClientsForTherapistAppointment_Result> Clients { get; set; }
        public List<ClientAvailability> ClientAvailabilities { get; set; }
        public List<ClientConsultationType> ClientConsultationTypes { get; set; }
        public List<Availabilities> Availabilities { get; set; }
        public List<DbTableIdNameProperties> NatureActs { get; set; }
        public List<DbTableIdNameProperties> ConsultationTypes { get; set; }
        public List<DbTableIdNameProperties> ProfessionalTitles { get; set; }
        public List<DbTableIdNameProperties> DeleteAppointmentReasons { get; set; }
        public List<string> DayHours { get; set; }
        public List<DbTableIdNameProperties> Days { get; set; }
        public List<TherapistPayInformations> Wages { get; set; }

        public List<int> TherapistAvailabilities { get; set; }



        public AppointmentViewModel()
        {
            this.DayHours = Enumerable.Range(0, 30).Select(i =>
                DateTime.Today.AddHours(6).AddMinutes(i * 30).ToString("HH:mm")).ToList();

            this.Availabilities = new List<Availabilities>();
        }

        public AppointmentViewModel(ReseauPsyEntities _context, int therapistId)
            :this()
        {
            //Set if the system is in french
            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";

            //Set the days list
            this.Days = _context.DaysOfTheWeeks
                .Select(x => new DbTableIdNameProperties
                { 
                    Id = x.Id,
                    Name = isFrench ? x.DayOfTheWeek : x.DayOfTheWeekEN
                }).ToList();

            this.TherapistAvailabilities = _context.TherapistAvailabilities
                .Where(x => x.TherapistId == therapistId)
                .Select(x => x.DayOfTheWeek_PeriodOfTheDay_Id)
                .ToList();

            //Set the list of client
            this.Clients = _context.GetListClientsForTherapistAppointment(therapistId, isFrench).ToList();

            foreach (var client in Clients)
            {
                client.ClientName = client.ClientName.Decode();
            }

            //Set the list of client availabilities
            this.ClientAvailabilities = _context.ClientAvailabilities.ToList();

            //Set the list of consultation type
            this.ClientConsultationTypes = _context.ClientConsultationTypes.ToList();

            this.Wages = _context.TherapistPayInformations
                .Where(x => x.TherapistId == therapistId && x.IsActive)
                .ToList();

            //Set the list of the nature act
            this.NatureActs = _context.NatureActs
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = isFrench ? x.NatureAct1 : x.NatureActEN
                })
                .ToList();

            //Set the list of consultation types
            this.ConsultationTypes = _context.ConsultationTypes
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = isFrench ? x.ConsultationType1 : x.ConsultationTypeEN
                })
                .ToList();

            //Set the list of consultation types
            this.ProfessionalTitles = _context.TherapistProfessionalTitles
                .Where(x => x.TherapistId == therapistId)
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.ProfessionalTitle.Id,
                    Name = isFrench ? x.ProfessionalTitle.Secteur : x.ProfessionalTitle.SecteurEN
                })
                .ToList();

            //Set the list for the delete reason
            this.DeleteAppointmentReasons = _context.ReasonsDeleteClientAppointments
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = isFrench ? x.Reason : x.ReasonEN
                })
                .ToList();

            //Set the list of days and periods of days
            var periodsDay = _context.DayOfTheWeek_PeriodOfTheDay.ToList();
            foreach (var day in _context.DaysOfTheWeeks.ToList())
            {
                this.Availabilities.Add(new Models.Availabilities
                {
                    Day = new DbTableIdNameProperties
                    {
                        Id = day.Id,
                        Name = isFrench ? day.DayOfTheWeek : day.DayOfTheWeekEN
                    },

                    Periods = periodsDay
                    .Where(x => x.DaysOfTheWeekId == day.Id)
                    .Select(x => new DbTableIdNameProperties
                    {
                        Id = x.Id,
                        Name = isFrench ?
                            x.PeriodsOfTheDay.PeriodOfTheDay :
                            x.PeriodsOfTheDay.PeriodOfTheDayEN
                    }).ToList()
                });
            }
        }
    }
}