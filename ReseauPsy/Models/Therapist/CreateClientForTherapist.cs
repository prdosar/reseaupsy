using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Therapist
{
    public class CreateClientForTherapist
    {
        public bool Succeeded { get; set; }

        private DetailClientDatas _datas;
        private ReseauPsyEntities _context;
        private Business.Clients _client;

        public CreateClientForTherapist(ReseauPsyEntities context, DetailClientDatas datas, Business.Therapist therapist)
        {
            _context = context;
            _datas = datas;

            CreateClientDb(therapist);
            CreateClientInfo();
            SetClientAvailabilities();
            SetClientLanguages();
            SetClientConsultationTypes();

            try
            {
                _context.SaveChanges();
                Succeeded = true;
            }
            catch (Exception)
            {
                Succeeded = false;
            }
        }

        private void CreateClientDb(Business.Therapist therapist)
        {
            Business.Clients client = new Clients();

            client.TherapistId = therapist.Id;
            client.LanguageId = _datas.CommunicationLanguageId;
            client.ConsultationReasonId = _datas.ConsultationReasonId;
            client.ClientAgeRangeId = _datas.ClientAgeRangeId;
            client.Message = "";
            client.RequestDate = DateTime.Now;
            client.IsExternalClient = true;
            client.IsDeleted = false;

            _context.Clients.Add(client);
            _client = client;
        }
        private void CreateClientInfo()
        {
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.Clients = _client;
            clientInfo.FirstName = _datas.FirstName;
            clientInfo.LastName = _datas.LastName;
            clientInfo.Email = _datas.Email;
            clientInfo.PhoneNumber = _datas.PhoneNumber;
            clientInfo.City = _datas.City;
            clientInfo.PostalCode = _datas.PostalCode;
            clientInfo.RegionId = _datas.RegionId;
            clientInfo.GenderId = _datas.GenderId;
            // Pour un client externe on met toujours le client hourly cost pour le therpaist hourly cost
            clientInfo.TherapistHourlyWage = _datas.ClientHourlyCost; 
            clientInfo.ClientHourlyCost = _datas.ClientHourlyCost;
            clientInfo.CreationDate = DateTime.Now;

            _client.ClientInfo.Add(clientInfo);
        }
        private void SetClientAvailabilities()
        {
            foreach (int availability in _datas.AvailabilityIds)
            {
                var clientAvailability = new Business.ClientAvailability();
                clientAvailability.Clients = _client;
                clientAvailability.DayOfTheWeek_PeriodOfTheDay_Id = availability;
                _client.ClientAvailabilities.Add(clientAvailability);
            }
        }
        private void SetClientLanguages()
        {
            foreach (int language in _datas.LanguageIds)
            {
                var clientLanguage = new Business.ClientLanguage();
                clientLanguage.Clients = _client;
                clientLanguage.LanguageId = language;
                _client.ClientLanguage.Add(clientLanguage);
            }
        }
        private void SetClientConsultationTypes()
        {
            foreach (int consultationType in _datas.LanguageIds)
            {
                var clientConsultationType = new Business.ClientConsultationType();
                clientConsultationType.Clients = _client;
                clientConsultationType.ConsultationTypeId = consultationType;
                _client.ClientConsultationTypes.Add(clientConsultationType);
            }
        }
    }
}