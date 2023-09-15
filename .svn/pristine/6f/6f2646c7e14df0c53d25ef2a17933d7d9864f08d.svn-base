using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;
using Business;
using HelpersLibrary;
using ReseauPsy.Resources;

namespace ReseauPsy.Models
{
    public class RefundClientPrePayment
    {

        private decimal _totalAppointmentCost;
        private string _uniqueRefPayment;


        public RefundClientPrePayment(ClientAppointments appointment, ReseauPsyEntities context)
        {
            RefundClient(appointment, context);
        }

        private void RefundClient(ClientAppointments appointment, ReseauPsyEntities context)
        {
            if (appointment.ClientPaymentDate != null)
            {

                VerifyIfClientPaid(appointment);

                GetAppointmentInfo(appointment, context);

                ProcessRefund(appointment, context);

                SendRefundEmailClient(appointment, context);
            }

        }

        private void VerifyIfClientPaid(ClientAppointments appointment)
        {
            var appointmentPayment = appointment.ClientPaymentInfos
            .OrderByDescending(x => x.TransactionDateTime)
            .First();

            //There is no prepayment in the database
            if (appointmentPayment == null)
                throw new Exception();

        }

        private void GetAppointmentInfo(ClientAppointments appointment, ReseauPsyEntities context)
        {
            var appointmentHelper = new AppointmentHelper(appointment.Id, context);
            _totalAppointmentCost = appointmentHelper.GetAppointmentCost(); //Need to reimbourse the client
            _uniqueRefPayment = appointmentHelper.GetPrePaymentUniqueRef();
        }
        private void ProcessRefund(ClientAppointments appointment, ReseauPsyEntities context)
        {
            var therapistInfo = new GetTherapistInfo(context, appointment.TherapistId);
            therapistInfo.GetMostRecentTherapistInfo();

            var nuveiPayment = new NuveiRefund(
                _uniqueRefPayment,
                DateTime.Now,
                context,
                $"{therapistInfo.FirstName} {therapistInfo.LastName}",
                "Rendez-vous annulé sans charge",
                _totalAppointmentCost);

            nuveiPayment.UrlPayment(appointment);
        }

        private void SendRefundEmailClient(ClientAppointments appointment, ReseauPsyEntities context)
        {
            var resourceManager = new ResourceManager(typeof(Email));
            var culture = new CultureInfo(appointment.Clients.LanguageId == 1 ? "fr-CA" : "en-CA");

            string emailSubject = resourceManager.GetString("ChargeCancelled_Subject", culture);
            string emailContent = resourceManager.GetString("ChargeCancelled_Content", culture);

            var clientInfos = new GetClientInfos(context, appointment.ClientId);
            clientInfos.GetMostRecentClientInfo();

            //Send email
            EmailHelper.Send(emailSubject,
                    emailContent,
                    clientInfos.Email,
                    "");
        }
    }
}