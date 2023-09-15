using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class AppointmentHelper
    {
        public ClientAppointments ClientAppointment { get; }
        private ReseauPsyEntities _context { get; set; }

        /// <summary>
        /// Set the ClientAppointment
        /// </summary>
        /// <param name="clientAppointment"></param>
        public AppointmentHelper(ClientAppointments clientAppointment, ReseauPsyEntities _context)
        {
            this.ClientAppointment = clientAppointment;
            this._context = _context;
        }

        /// <summary>
        /// Set the client appointment with the client appointment id
        /// </summary>
        /// <param name="clientAppointmentId"></param>
        /// <param name="_context"></param>
        public AppointmentHelper(int clientAppointmentId, ReseauPsyEntities _context)
        {
            this.ClientAppointment = _context.ClientAppointments
                .Where(x => x.Id == clientAppointmentId)
                .First();

            this._context = _context;
        }

        /// <summary>
        /// Get the cost of the appointment for the client
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        public decimal GetAppointmentCost()
        {

            decimal subtotal = this.ClientAppointment.TotalDollarAmount.Value;
            decimal tps = 0;
            decimal tvq = 0;

            GetTherapistInfo therapistInfo = new GetTherapistInfo(_context, ClientAppointment.TherapistId);
            therapistInfo.GetOldTherapistInfo(ClientAppointment.ClientBillGeneratedDate.Value);

            if (therapistInfo.IsTaxable)
            {
                var taxesData = new GetTaxesData(_context);
                taxesData.GetOldTaxData(this.ClientAppointment.ClientBillGeneratedDate.Value);

                tps = subtotal * taxesData.Tps;
                tvq = subtotal * taxesData.Tvq;
            }

            return Math.Round(subtotal + tps + tvq, 2);
        }

        /// <summary>
        /// Get the prepayment unique ref
        /// </summary>
        /// <returns></returns>
        public string GetPrePaymentUniqueRef()
        {
            //Get the last occurence of a payment
            var appointmentPayment = this.ClientAppointment.ClientPaymentInfos
                .Where(x => x.UniqueRef != null)
                .OrderByDescending(x => x.TransactionDateTime)
                .First();

            return appointmentPayment.UniqueRef;
        }
    }
}