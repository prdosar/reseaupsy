using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Therapist
{
    public class AddAppointmentInfos
    {
        public int ClientId { get; set; }
        public string StartDate { get; set; }
        public string TimeStart { get; set; }
        public decimal NbSession { get; set; }
        public int? WageId { get; set; }
        public int NatureActId { get; set; }
        public int ProfessionnalTitleId { get; set; }
        public int ConsultationTypeId { get; set; }
        public bool IsRepetition { get; set; }
        public int? RepeatEveryNumber { get; set; }
        public string RepeatEveryPeriod { get; set; }
        public int? RepeatAmount { get; set; }
        public List<int> RepeatDays { get; set; }

    }
}