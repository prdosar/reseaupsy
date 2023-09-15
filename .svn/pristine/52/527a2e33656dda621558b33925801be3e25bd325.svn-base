using Business;
using ReseauPsy.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ReseauPsy.ViewModel.Client
{
    public class PaymentViewModel
    {
        public bool IsGuidValid { get; set; }
        public string AppointmentDuration { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentType { get; set; }
        public string TherapistName { get; set; }
        public bool IsTaxable { get; set; }
        public string TpsNumber { get; set; }
        public string TvqNumber { get; set; }
        public decimal AppointmentSubtotal { get; set; }
        public decimal AppointmentTps { get; set; }
        public decimal AppointmentTvq { get; set; }
        public decimal AppointmentTotal { get; set; }
        public bool ShowDownloadReceipt { get; set; }
        public bool IsAppointmentPaid { get; set; }
        public bool IsPrepaiement { get; set; }

        public PaymentViewModel(ReseauPsyEntities _context, Guid id)
        {
            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";

            var clientAppointment = _context.ClientAppointments
                .Where(x => x.PaymentUrlCode == id)
                .FirstOrDefault();

            //The Guid is not valid
            if (clientAppointment == null)
            {
                this.IsGuidValid = false;
                return;
            }


            if (clientAppointment.ClientPaymentDate != null)
            {
                //Appointment is paid
                this.IsAppointmentPaid = true;

                if (clientAppointment.AppointmentStatusId == 1)
                {
                    //Presence not confirmed yet

                }
                else if (clientAppointment.AppointmentStatusId == 4 || //Canceld with charges
                        clientAppointment.AppointmentStatusId == 5) //Completed
                {
                    //Presence is confirmed
                    this.ShowDownloadReceipt = true;
                }
            }

            this.IsGuidValid = true;

            #region Duration

            string duration = "";

            if (clientAppointment.NbSession == 0.5m) // 30 minutes
                duration = "30 minutes";
            else if (clientAppointment.NbSession >= 2) // 2 hours or more
                duration = clientAppointment.NbSession + " " + (isFrench ? "heures" : "hours");
            else //Between 1 and 2 hours
                duration = clientAppointment.NbSession + " " + (isFrench ? "heure" : "hour");

            #endregion

            this.IsPrepaiement = DateTime.Now < clientAppointment.StartDateTime;
            this.AppointmentDuration = duration;
            this.AppointmentDate = clientAppointment.StartDateTime.ToString("dd-MM-yyyy");
            this.AppointmentType = isFrench ?
                clientAppointment.ConsultationTypes.ConsultationType1 :
                clientAppointment.ConsultationTypes.ConsultationTypeEN;

            this.AppointmentSubtotal = clientAppointment.TotalDollarAmount.Value;

            var therapistInfo = new GetTherapistInfo(_context, clientAppointment.Therapists.Id);
            therapistInfo.GetOldTherapistInfo(clientAppointment.ClientBillGeneratedDate.Value);

            this.TherapistName = therapistInfo.FirstName + " " + therapistInfo.LastName;
            this.IsTaxable = therapistInfo.IsTaxable;


            if (this.IsTaxable)
            {
                var taxesData = new GetTaxesData(_context);
                taxesData.GetOldTaxData(clientAppointment.ClientBillGeneratedDate.Value);

                this.AppointmentTps = this.AppointmentSubtotal * taxesData.Tps;
                this.AppointmentTvq = this.AppointmentSubtotal * taxesData.Tvq;

                var rspyInfo = new GetReseauPsyInfo(_context);
                rspyInfo.GetOldInfo(clientAppointment.ClientBillGeneratedDate.Value);

                this.TpsNumber = rspyInfo.TpsNumber;
                this.TvqNumber = rspyInfo.TvqNumber;
            }

            this.AppointmentTotal = this.AppointmentSubtotal + this.AppointmentTps + this.AppointmentTvq;

            if (clientAppointment.Clients.IsExternalClient)
            {
                GetExternalClientFeeDatas feeData = new GetExternalClientFeeDatas(_context);
                feeData.GetOldData(clientAppointment.ClientBillGeneratedDate.Value);

                decimal fees = (this.AppointmentTotal + feeData.FixedFee) * feeData.PercentageFeeDecimal + feeData.FixedFee;

                this.AppointmentTotal = this.AppointmentTotal + fees;

                if (this.IsTaxable)
                {
                    //On doit faire "disparaitre" les frais en boostant le sous-total
                    var taxesData = new GetTaxesData(_context);
                    taxesData.GetOldTaxData(clientAppointment.ClientBillGeneratedDate.Value);

                    this.AppointmentSubtotal = AppointmentTotal / (taxesData.Tps + taxesData.Tvq + 1);
                    this.AppointmentTps = this.AppointmentSubtotal * taxesData.Tps;
                    this.AppointmentTvq = this.AppointmentSubtotal * taxesData.Tvq;
                }
                else
                {
                    this.AppointmentSubtotal = this.AppointmentTotal;
                }
            }


        }
    }
}