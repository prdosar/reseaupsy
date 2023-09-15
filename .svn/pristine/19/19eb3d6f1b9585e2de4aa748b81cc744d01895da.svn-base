using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Admin
{
    public class EditClient
    {
        public bool Succeeded { get; set; }

        private DetailClientDatas _datas;
        private ReseauPsyEntities _context;
        private Business.Clients _client;

        public EditClient(ReseauPsyEntities context, DetailClientDatas datas, int clientId)
        {
            _context = context;
            _datas = datas;

            GetClientFromId(clientId);
            EditClientDb();
            CreateClientInfo();
            SetClientAvailabilities();
            SetClientLanguages();
            SetClientConsultationTypes();

            try
            {
                _context.SaveChanges();
                Succeeded = true;
            }
            catch (Exception ex)
            {
                Succeeded = false;
            }
        }

        private void GetClientFromId(int clientId)
        {
            _client = _context.Clients
                .Where(x => x.Id == clientId)
                .First();
        }
        private void EditClientDb()
        {
            _client.LanguageId = _datas.CommunicationLanguageId;
            _client.ConsultationReasonId = _datas.ConsultationReasonId;
            _client.ClientAgeRangeId = _datas.ClientAgeRangeId;
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
            clientInfo.TherapistHourlyWage = _datas.TherapistHourlyWage;
            clientInfo.ClientHourlyCost = _datas.ClientHourlyCost;
            clientInfo.CreationDate = DateTime.Now;

            _client.ClientInfo.Add(clientInfo);
        }
        private void SetClientAvailabilities()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.ClientAvailabilities.RemoveRange(_client.ClientAvailabilities);

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
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.ClientLanguages.RemoveRange(_client.ClientLanguage);

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
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.ClientConsultationTypes.RemoveRange(_client.ClientConsultationTypes);

            foreach (int consultationType in _datas.ConsultationTypesIds)
            {
                var clientConsultationType = new Business.ClientConsultationType();
                clientConsultationType.Clients = _client;
                clientConsultationType.ConsultationTypeId = consultationType;
                _client.ClientConsultationTypes.Add(clientConsultationType);
            }
        }
    }
}