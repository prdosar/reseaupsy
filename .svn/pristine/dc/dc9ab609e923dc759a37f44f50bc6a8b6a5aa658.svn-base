using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;

namespace ReseauPsy.Models
{
    public class GetAppointmentCost
    {
        public decimal SubTotal { get; set; }
        public decimal Tps { get; set; }
        public decimal Tvq { get; set; }
        public decimal Total { get; set; }

        public GetAppointmentCost(ReseauPsyEntities context, int appointmentId)
        {
            GetSingleAppointmentCost(context, appointmentId);
        }

        private void GetSingleAppointmentCost(ReseauPsyEntities context, int appointmentId)
        {
            var clientAppointment = context.ClientAppointments
                .Where(x => x.Id == appointmentId)
                .Single();

            this.SubTotal = clientAppointment.TotalDollarAmount.Value;

            GetTherapistInfo therapistInfo = new GetTherapistInfo(context, clientAppointment.TherapistId);
            therapistInfo.GetOldTherapistInfo(clientAppointment.ClientBillGeneratedDate.Value);

            if (therapistInfo.IsTaxable)
            {
                GetTaxesData taxesData = new GetTaxesData(context);
                taxesData.GetOldTaxData(clientAppointment.ClientBillGeneratedDate.Value);

                this.Tps = taxesData.Tps * this.SubTotal;
                this.Tvq = taxesData.Tvq * this.SubTotal;
            }
            else
            {
                this.Tps = 0;
                this.Tvq = 0;
            }

            this.Total = this.SubTotal + this.Tps + this.Tvq;

            if (clientAppointment.Clients.IsExternalClient)
            {
                GetExternalClientFeeDatas feeData = new GetExternalClientFeeDatas(context);
                feeData.GetOldData(clientAppointment.ClientBillGeneratedDate.Value);

                decimal fees = (this.Total + feeData.FixedFee) * feeData.PercentageFeeDecimal + feeData.FixedFee;

                this.Total = this.Total + fees;

                //On doit faire "disparaitre" les frais en boostant le sous-total
                var taxesData = new GetTaxesData(context);
                taxesData.GetOldTaxData(clientAppointment.ClientBillGeneratedDate.Value);

                if (therapistInfo.IsTaxable)
                {
                    this.SubTotal = this.Total / (taxesData.Tps + taxesData.Tvq + 1);
                    this.Tps = this.SubTotal * taxesData.Tps;
                    this.Tvq = this.SubTotal * taxesData.Tvq;
                }
            }
        }
    }
}