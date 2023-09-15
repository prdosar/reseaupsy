using Business;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Therapist
{
    public class AppointmentModalAppointmentInfos
    {
        public int NatureActId { get; set; }
        public string NatureActName { get; set; }
        public int ProfessionnalTitleId { get; set; }
        public string ProfessionnalTitleName { get; set; }
        public int ConsultationTypeId { get; set; }
        public string ConsultationTypeName { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public decimal Duration { get; set; }
        public string TherapistWageName { get; set; }
        public int? TherapistWageId { get; set; }
        public bool IsEditable { get; set; }


        public AppointmentModalAppointmentInfos(ReseauPsyEntities _context, int appointmentId, bool isFrench)
        {
            var appointment = _context.ClientAppointments
                .Where(x => x.Id == appointmentId)
                .Single();

            this.NatureActId = appointment.NatureActId;
            this.NatureActName = isFrench ? 
                appointment.NatureActs.NatureAct1 : 
                appointment.NatureActs.NatureActEN;

            this.ProfessionnalTitleId = appointment.ProfessionalTitleId;
            this.ProfessionnalTitleName = isFrench ?
                appointment.ProfessionalTitles.Secteur:
                appointment.ProfessionalTitles.SecteurEN;

            this.ConsultationTypeId = appointment.ConsultationTypeId;
            this.ConsultationTypeName = isFrench ?
                appointment.ConsultationTypes.ConsultationType1 :
                appointment.ConsultationTypes.ConsultationTypeEN;

            this.StartDate = appointment.StartDateTime.ToString("dd-MM-yyyy");
            this.StartTime = appointment.StartDateTime.ToString("HH:mm");
            this.Duration = appointment.NbSession;
            this.TherapistWageName = appointment.TherapistPayInformationId == null ? "" :appointment.TherapistPayInformations.PayInfoName;
            this.TherapistWageId = appointment.TherapistPayInformationId;
            this.IsEditable = appointment.ClientBillGeneratedDate == null;
        }
    }
}