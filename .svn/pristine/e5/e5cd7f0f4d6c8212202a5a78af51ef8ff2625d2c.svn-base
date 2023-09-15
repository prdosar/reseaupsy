using Business;
using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;

namespace ReseauPsy.Models.Admin
{
    public class ClientRequestTable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime RequestDate { get; set; }
        public string Region { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string ConsultationReason { get; set; }
        public string Message { get; set; }
        public string RequestStatus { get; set; }
        public string LastTherapistMessage { get; set; }
        public Nullable<int> RefusedCount { get; set; }
        public string DeleteReason { get; set; }
        public string DeleteNote { get; set; }
        public bool IsAllowReactivate { get; set; }
        public bool IsDeletedClient { get; set; }
        public int RegionId { get; set; }

        public ClientRequestTable(GetListClientForClientRequestPage_Result getListClientForClient, bool isFrench)
        {

            this.Id = getListClientForClient.Id;
            this.FirstName = getListClientForClient.FirstName;
            this.LastName = getListClientForClient.LastName;
            this.RequestDate = getListClientForClient.RequestDate;
            this.Region = getListClientForClient.Region;
            this.Email = getListClientForClient.Email;
            this.PhoneNumber = getListClientForClient.PhoneNumber;
            this.City = getListClientForClient.City;
            this.PostalCode = getListClientForClient.PostalCode;
            this.ConsultationReason = getListClientForClient.consultationReason;
            this.Message = getListClientForClient.Message;
            this.LastTherapistMessage = getListClientForClient.LastTherapistMessage;
            this.RefusedCount = getListClientForClient.RefusedCount;
            this.DeleteReason = getListClientForClient.DeleteReason;
            this.DeleteNote = getListClientForClient.DeletedAdditionalNotes;
            this.IsDeletedClient = getListClientForClient.DeleteReason != null;
            this.RegionId = getListClientForClient.RegionId;



            this.IsAllowReactivate = true;

            var resourceManager = new ResourceManager(typeof(Resource));
            this.RequestStatus = resourceManager.GetString(getListClientForClient.RequestStatus);

            if (isFrench)
                this.RequestStatus = resourceManager.GetString(getListClientForClient.RequestStatus, CultureInfo.CreateSpecificCulture("fr"));

            if (getListClientForClient.RequestStatus == "AppointmentRequest_Status_SentToExternalAssociation" ||
                getListClientForClient.RequestStatus == "AppointmentRequest_Status_HasTherapist")
            {
                this.IsAllowReactivate = false;
            }

        }


    }
}