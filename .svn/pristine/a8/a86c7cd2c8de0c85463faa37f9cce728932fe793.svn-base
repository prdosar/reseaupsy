using Business;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{ 
    /// <summary>
    /// Classe générique pour un Id et un name d'une table
    /// </summary>
    public class DbTableIdNameProperties
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Availabilities
    {
        public DbTableIdNameProperties Day { get; set; }
        public List<DbTableIdNameProperties> Periods { get; set; }
    }

    public class OfferedService
    {
        public DbTableIdNameProperties Service { get; set; }
        public List<DbTableIdNameProperties> ServiceTypes { get; set; }
    }

    public class ClientConsultationTypeNameClient
    {
        public int ConsultationTypeId { get; set; }
        public int ClientId { get; set; }
        public string ConsultationTypeFR { get; set; }
        public string ConsultationTypeEN { get; set; }

    }

    public class TherapistIdNameAvailabilities
    {
        public int TherapistId { get; set; }
        public string TherapistName { get; set; }
        public List<int> TherapistAvailabilities { get; set; }

        public TherapistIdNameAvailabilities(ReseauPsyEntities context, Business.Therapist therapist)
        {
            this.TherapistId = therapist.Id;

            GetTherapistInfo therapistInfo = new GetTherapistInfo(context, therapist.Id);
            therapistInfo.GetMostRecentTherapistInfo();

            this.TherapistName = $"{therapistInfo.FirstName} {therapistInfo.LastName}";
            this.TherapistAvailabilities = therapist.TherapistAvailabilities
                .Select(x => x.DayOfTheWeek_PeriodOfTheDay_Id)
                .ToList();

        }
    }
}
