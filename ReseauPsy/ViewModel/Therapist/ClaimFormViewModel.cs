using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Business;
using Library.Helper;
using ReseauPsy.Models;
using ReseauPsy.Models.Therapist;

namespace ReseauPsy.ViewModel.Therapist
{
    public class ClaimableAppointments
    {
        public int AppointmentId { get; set; }
        public string ClientName { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public decimal NbSession { get; set; }
        public Nullable<int> ClientPaymentReceiptNumber { get; set; }
        public decimal HourlyWage { get; set; }
        public decimal ClaimableAmount { get; set; }
        public decimal TpsAmount { get; set; }
        public decimal TvqAmount { get; set; }
        public string NatureAct { get; set; }
    }

    public class ClaimFormViewModel
    {
        public List<ClaimableAppointments> ClaimableAppointments { get; set; }
        public string TherapistName { get; set; }
        public string TherapistRegion { get; set; }
        public string TherapistEmail { get; set; }
        public string TherapistPhoneNumber { get; set; }
        public string TherapistAdress { get; set; }
        public string TherapistTpsNumber { get; set; }
        public string TherapistTvqNumber { get; set; }
        public int TherapistBillNumber { get; set; }
        public int NbPagerPageShown { get; set; }


        public List<GetListTherapistBillSent_Result> BillsSent { get; set; }
        public int NbPage { get; set; }

        public ClaimFormViewModel(ReseauPsyEntities _context, Business.Therapist therapist)
        {
            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;

            var therapistInfo = new GetTherapistInfo(_context, therapist.Id);
            therapistInfo.GetMostRecentTherapistInfo();


            this.TherapistName = $"{therapistInfo.FirstName} {therapistInfo.LastName}";
            this.TherapistAdress = therapistInfo.Adress;
            this.TherapistEmail = therapistInfo.Email;
            this.TherapistPhoneNumber = therapistInfo.PhoneNumber;
            this.TherapistTpsNumber = therapistInfo.TpsNumber;
            this.TherapistTvqNumber = therapistInfo.TvqNumber;

            this.TherapistRegion = _context.Regions
                .Where(x => x.Id == therapistInfo.RegionId)
                .Select(x => x.Region1)
                .First();

            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";

            var appointments = _context.GetListClaimableAppointmentForTherapistClaimForm(
                isFrench,
                therapist.Id)
                .ToList();

            ClaimableAppointments = new List<ClaimableAppointments>();
            foreach (var appointment in appointments)
            {
                var claimableAppointments = new ClaimableAppointments();
                claimableAppointments.AppointmentId = appointment.AppointmentId;
                claimableAppointments.ClientName = appointment.ClientName.Decode();
                claimableAppointments.AppointmentDate = appointment.AppointmentDate;
                claimableAppointments.NbSession = appointment.NbSession;
                claimableAppointments.ClientPaymentReceiptNumber = appointment.ClientPaymentReceiptNumber;
                claimableAppointments.NatureAct = appointment.NatureAct;

                GetClaimableAmountForAppointment getClaimableAmountForAppointment = new GetClaimableAmountForAppointment(_context);
                getClaimableAmountForAppointment.CalculateAppointmentPayment(appointment.AppointmentId);

                claimableAppointments.HourlyWage = getClaimableAmountForAppointment.HourlyWage;
                claimableAppointments.ClaimableAmount = getClaimableAmountForAppointment.ClaimableAmount;
                claimableAppointments.TpsAmount = getClaimableAmountForAppointment.TpsAmount;
                claimableAppointments.TvqAmount = getClaimableAmountForAppointment.TvqAmount;

                ClaimableAppointments.Add(claimableAppointments);
            }

            var lastTherapistBillId = _context.ClientAppointments
                .Where(x => x.TherapistId == therapist.Id)
                .OrderByDescending(x => x.TherapistBillId)
                .Select(x => x.TherapistBillId)
                .FirstOrDefault();

            this.TherapistBillNumber = lastTherapistBillId == null ? 1 : lastTherapistBillId.Value + 1;



            this.BillsSent = _context.GetListTherapistBillSent(
                therapist.Id,
                null,
                null,
                1,
                WebSiteProperties.NbResultPerPage)
                .ToList();

            var count = _context.GetListTherapistBillSentCount(
                therapist.Id,
                null,
                null).ToList();

            this.NbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(count[0])
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );

        }
    }
}