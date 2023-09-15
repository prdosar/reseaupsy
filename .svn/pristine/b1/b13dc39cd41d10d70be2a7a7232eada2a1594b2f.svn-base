using Business;
using Library.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace ReseauPsy.Models.Therapist
{
    public class AppointmentModalModel
    {
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string ClientConsultationTypes { get; set; }
        public string ClientRegion { get; set; }
        public string ClientCity { get; set; }
        public string ClientPostalCode { get; set; }
        public string ClientConsultationReason { get; set; }
        public string ClientMessage { get; set; }
        public string ClientAvailabilities { get; set; }
        public string ClientLanguages { get; set; }
        public bool IsExternal { get; set; }



        public AppointmentModalModel(ReseauPsyEntities _context, int clientId, bool isFrench)
        {
            var client = _context.Clients
                .Where(x => x.Id == clientId)
                .Single();

            var clientInfos = new GetClientInfos(_context, clientId);
            clientInfos.GetMostRecentClientInfo();

            this.ClientName = $"{clientInfos.FirstName.Decode()} {clientInfos.LastName.Decode()}";
            this.ClientEmail = clientInfos.Email.Decode();
            this.ClientPhone = clientInfos.PhoneNumber.Decode();
            this.ClientConsultationTypes = string.Join(", ", client.ClientConsultationTypes
                .Select(x => isFrench
                    ? x.ConsultationType.ConsultationType1
                    : x.ConsultationType.ConsultationTypeEN)
                .ToList());
            this.ClientRegion = clientInfos.Region.Region1;
            this.ClientCity = clientInfos.City.Decode();
            this.ClientPostalCode = clientInfos.PostalCode.Decode();
            this.ClientConsultationReason = isFrench
                ? client.ConsultationReasons.ConsultationReason1
                : client.ConsultationReasons.ConsultationReasonEN;
            this.ClientMessage = client.Message.Decode();
            this.ClientAvailabilities = string.Join(",", client.ClientAvailabilities
                .Select(x => x.DayOfTheWeek_PeriodOfTheDay_Id)
                .ToList());

            this.ClientLanguages = string.Join(", ", _context.ClientLanguages
                .Where(x => x.ClientId == clientId)
                .Select(x => isFrench ?
                    x.Language.Language1 :
                    x.Language.LanguageEN)
                .ToList());

            this.IsExternal = clientInfos.IsExternal;
        }
    }
}