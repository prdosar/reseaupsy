using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;
using Library.Helper;

namespace ReseauPsy.Models
{
    public class GetClientInfos
    {
        private int _clientId;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public decimal? TherapistHourlyWage { get; set; }
        public decimal? ClientHourlyCost { get; set; }
        public bool IsExternal { get; set; }

        private ReseauPsyEntities _ctx;




        public GetClientInfos(ReseauPsyEntities context, int clientId)
        {
            this._ctx = context;
            this._clientId = clientId;
        }




        public void GetMostRecentClientInfo()
        {
            var clientInfos = _ctx.ClientInfo
                .Where(x => x.ClientId == _clientId)
                .OrderByDescending(x => x.CreationDate)
                .First();

            BindClientInformation(clientInfos);
        }

        public void GetOldClientInfo(DateTime date)
        {
            var clientInfos = _ctx.ClientInfo
                    .Where(x => x.ClientId == _clientId && x.CreationDate < date)
                    .OrderByDescending(x => x.CreationDate)
                    .First();

            BindClientInformation(clientInfos);
        }





        private void BindClientInformation(ClientInfo clientInfo)
        {
            this.FirstName = clientInfo.FirstName.Decode();
            this.LastName = clientInfo.LastName.Decode();
            this.Email = clientInfo.Email.Decode();
            this.PhoneNumber = clientInfo.PhoneNumber.Decode();
            this.City = clientInfo.City.Decode();
            this.PostalCode = clientInfo.PostalCode.Decode();
            this.RegionId = clientInfo.RegionId;
            this.Region = clientInfo.Regions;
            this.TherapistHourlyWage = clientInfo.TherapistHourlyWage;
            this.ClientHourlyCost = clientInfo.ClientHourlyCost;
            this.IsExternal = clientInfo.Clients.IsExternalClient;
        }

    }
}