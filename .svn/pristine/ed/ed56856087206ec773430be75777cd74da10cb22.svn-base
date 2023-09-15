using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;
using Library.Helper;

namespace ReseauPsy.ViewModel.Therapist
{
    public class DashboardTherapistViewModel
    {
        public int AppointmentRequestCount { get; set; }
        public int ClaimableAppointmentCount { get; set; }
        public int ConfirmPresenceCount { get; set; }
        public List<GetListIncommingAppointmentTherapist_Result> IncomingAppointments { get; set; }



        public DashboardTherapistViewModel(ReseauPsyEntities _context, Business.Therapist therapist)
        {
            this.IncomingAppointments = _context.GetListIncommingAppointmentTherapist(
                3, 
                therapist.Id)
                .ToList();

            foreach (var appointment in IncomingAppointments)
            {
                appointment.ClientName = appointment.ClientName.Decode();
            }


            this.AppointmentRequestCount = therapist.TherapistClientRequest
                .Where(x => x.IsAccepted == null)
                .Count();

            this.ClaimableAppointmentCount = therapist.ClientAppointments
                .Where(x =>
                    (x.AppointmentStatusId == 4 || x.AppointmentStatusId == 5) //Present or canceled with charges
                    && x.TherapistBillId == null        //Not claimed yet
                    && x.ClientPaymentDate != null      //Client paid
                    && !x.IsDeleted)     
                .Count();

            this.ConfirmPresenceCount = therapist.ClientAppointments
                .Where(x => 
                    x.EndDateTime < DateTime.Now 
                    && x.AppointmentStatusId == 1 
                    && !x.IsDeleted)
                .Count();
        }
    }


}