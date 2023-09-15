using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Therapist
{
    public class GetClaimableAmountForAppointment
    {
        public decimal ClaimableAmount { get; set; }
        public decimal TpsAmount { get; set; }
        public decimal TvqAmount { get; set; }
        public decimal HourlyWage { get; set; }

        private ReseauPsyEntities _context;
        public GetClaimableAmountForAppointment(ReseauPsyEntities context)
        {
            this._context = context;
        }

        public void CalculateAppointmentPayment(int appointmentId)
        {
            var appointment = _context.ClientAppointments
                .Where(x => x.Id == appointmentId)
                .First();

            CalculateClaimableAmount(appointment);
            CalculateTaxes(appointment);
        }
        private void CalculateClaimableAmount(ClientAppointments appointment)
        {
            if (appointment.TherapistPayInformationId == null)
            {
                //C'est un client externe
                GetClientInfos getClientInfos = new GetClientInfos(_context, appointment.ClientId);
                getClientInfos.GetOldClientInfo(appointment.ClientBillGeneratedDate.Value);
                this.HourlyWage = getClientInfos.TherapistHourlyWage.Value;
                this.ClaimableAmount = appointment.NbSession * getClientInfos.TherapistHourlyWage.Value;
            }
            else
            {
                //C'est un client interne
                this.HourlyWage = appointment.TherapistPayInformations.TherapistHourlyWage;
                this.ClaimableAmount = appointment.NbSession * appointment.TherapistPayInformations.TherapistHourlyWage;
            }
        }
        private void CalculateTaxes(ClientAppointments appointment)
        {
            var getTherapistPayInformation = new GetTherapistPayInformation(_context, appointment.TherapistId);
            bool isTaxable = getTherapistPayInformation.WasTherapistTaxable(appointment.ClientBillGeneratedDate.Value);

            if (!isTaxable)
            {
                this.TpsAmount = 0;
                this.TvqAmount = 0;
                return;
            }

            GetTaxesData getTaxesData = new GetTaxesData(_context);
            getTaxesData.GetOldTaxData(appointment.ClientBillGeneratedDate.Value);

            this.TpsAmount = this.ClaimableAmount * getTaxesData.Tps;
            this.TvqAmount = this.ClaimableAmount * getTaxesData.Tvq;
        }
    }
}