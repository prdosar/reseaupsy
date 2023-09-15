using Business;
using Library.Helper;
using ReseauPsy.Models;
using ReseauPsy.Models.Admin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ReseauPsy.ViewModel.Admin
{
    public class DetailClientViewModel
    {
        public ClientInfo ClientInfo { get; set; }
        public List<int> ClientAvailabilities { get; set; }
        public List<int> ClientConsultationTypes { get; set; }
        public List<int> ClientLanguages { get; set; }
        public Clients Client { get; set; }
        public int? ClientId { get; set; }
        public int? TherapistId { get; set; }
        public string TherapistName { get; set; }
        public bool IsCustomRate { get; set; }



        public List<DbTableIdNameProperties> Regions { get; set; }
        public List<DbTableIdNameProperties> Genders { get; set; }
        public List<DbTableIdNameProperties> Therapists { get; set; }
        public List<DaysOfTheWeek> Days { get; set; }
        public List<DbTableIdNameProperties> ConsultationTypes { get; set; }
        public List<DbTableIdNameProperties> Languages { get; set; }
        public List<DbTableIdNameProperties> ConsultationReasons { get; set; }
        public List<DbTableIdNameProperties> ClientAgeRanges { get; set; }

        public bool CanAccess { get; set; }


        public DetailClientViewModel(ReseauPsyEntities context)
        {
            bool isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";

            SetDefaultData();

            Regions = context.Regions
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = x.Region1
                })
                .ToList();

            Genders = context.Genders
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = isFrench ? x.Gender1 : x.GenderEN
                })
                .ToList();


            var therapistHelper = new TherapistHelper(context);
            this.Therapists = therapistHelper.GetListTherapistIdName();

            IsCustomRate = false;
            Days = context.DaysOfTheWeeks.ToList();

            ConsultationTypes = context.ConsultationTypes
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = isFrench ? x.ConsultationType1 : x.ConsultationTypeEN
                })
                .ToList();

            Languages = context.Languages
               .Select(x => new DbTableIdNameProperties
               {
                   Id = x.Id,
                   Name = isFrench ? x.Language1 : x.LanguageEN
               })
               .ToList();

            ConsultationReasons = context.ConsultationReasons
               .Select(x => new DbTableIdNameProperties
               {
                   Id = x.Id,
                   Name = isFrench ? x.ConsultationReason1 : x.ConsultationReasonEN
               })
               .ToList();

            ClientAgeRanges = context.ClientsAgeRanges
               .Select(x => new DbTableIdNameProperties
               {
                   Id = x.Id,
                   Name = isFrench ? x.ClientAgeRange : x.ClientAgeRangeEN
               })
               .ToList();

            CanAccess = true;
        }


        public DetailClientViewModel(ReseauPsyEntities context, int clientId) : this(context)
        {
            this.ClientId = clientId;

            ClientInfo = context.ClientInfo
                .Where(x => x.ClientId == clientId)
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault();

            if (ClientInfo == null)
            {
                CanAccess = false;
                return;
            }

            var getTherapistInfo = new GetTherapistInfo(context, ClientInfo.Clients.TherapistId.Value);
            getTherapistInfo.GetMostRecentTherapistInfo();
            this.TherapistName = $"{getTherapistInfo.FirstName} {getTherapistInfo.LastName}";

            TherapistId = ClientInfo.Clients.TherapistId.Value;
            ClientInfo.FirstName = ClientInfo.FirstName.Decode();
            ClientInfo.LastName = ClientInfo.LastName.Decode();
            ClientInfo.Email = ClientInfo.Email.Decode();
            ClientInfo.PhoneNumber = ClientInfo.PhoneNumber.Decode();
            ClientInfo.City = ClientInfo.City.Decode();
            ClientInfo.PostalCode = ClientInfo.PostalCode.Decode();

            if (ClientInfo.ClientHourlyCost != null)
                IsCustomRate = true;

            ClientAvailabilities = context.ClientAvailabilities
                .Where(x => x.ClientId == clientId)
                .Select(x => x.DayOfTheWeek_PeriodOfTheDay_Id)
                .ToList();

            ClientConsultationTypes = context.ClientConsultationTypes
                .Where(x => x.ClientId == clientId)
                .Select(x => x.ConsultationTypeId)
                .ToList();

            ClientLanguages = context.ClientLanguages
                .Where(x => x.ClientId == clientId)
                .Select(x => x.LanguageId)
                .ToList();

            Client = context.Clients
                .Where(x => x.Id == clientId)
                .Single();



            if (this.Client.IsDeleted || this.Client.IsExternalClient || this.TherapistId == null)
                CanAccess = false;
        }

        private void SetDefaultData()
        {
            ClientInfo = new ClientInfo();
            this.ClientAvailabilities = new List<int>();
            this.ClientConsultationTypes = new List<int>();
            this.ClientLanguages = new List<int>(); ;
            this.Client = new Clients();

        }
    }
}