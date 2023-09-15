using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;
using Library.Helper;

namespace ReseauPsy.Models
{
    public class GetTherapistInfo
    {
        private int _therapistId;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? GenderId { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int? RegionId { get; set; }
        public int? AccreditationId { get; set; }
        public string OpqNumber { get; set; }
        public string CertificationAndSpecialities { get; set; }
        public bool? TakeClientFromAssuranceCompany { get; set; }
        public bool? TakeClientFromThirdPartyPayer { get; set; }
        public bool? IsInterestedForFormation { get; set; }
        public string TpsNumber { get; set; }
        public string TvqNumber { get; set; }

        public decimal HourlyWage { get; set; }
        public decimal ClientHourlyCost { get; set; }
        public bool IsTaxable { get; set; }

        public List<TherapistAssociationAndProfessionalOrder> Associations { get; set; }

        private ReseauPsyEntities _ctx;


        public GetTherapistInfo(ReseauPsyEntities context, int therapistId)
        {
            this._ctx = context;
            this._therapistId = therapistId;
        }




        public void GetMostRecentTherapistInfo()
        {
            var therapistInfos = _ctx.TherapistInfo
                .Where(x => x.TherapistId == _therapistId)
                .OrderByDescending(x => x.ModificationDate)
                .First();

            BindTherapistInformation(therapistInfos);

            var therapistPayInformations = _ctx.TherapistPayInformations
                .Where(x => x.TherapistId == _therapistId)
                .OrderByDescending(x => x.ChangedDate)
                .First();

            BindPayInformations(therapistPayInformations);

            this.Associations = _ctx.TherapistAssociationAndProfessionalOrders
                .Where(x => x.TherapistId == _therapistId && x.DeleteDate == null)
                .ToList();
        }

        public void GetOldTherapistInfo(DateTime date)
        {
            var therapistInfos = _ctx.TherapistInfo
                    .Where(x => x.TherapistId == _therapistId && x.ModificationDate < date)
                    .OrderByDescending(x => x.ModificationDate)
                    .First();

            BindTherapistInformation(therapistInfos);

            var therapistPayInformations = _ctx.TherapistPayInformations
                .Where(x => x.TherapistId == _therapistId && x.ChangedDate < date)
                .OrderByDescending(x => x.ChangedDate)
                .First();

            BindPayInformations(therapistPayInformations);

            this.Associations = _ctx.TherapistAssociationAndProfessionalOrders
                .Where(x => x.TherapistId == _therapistId && x.CreateDate < date && 
                    (x.DeleteDate == null || x.DeleteDate > date))
                .ToList();

        }





        private void BindTherapistInformation(TherapistInfo therapistInfo)
        {
            this.FirstName = therapistInfo.FirstName.Decode();
            this.LastName = therapistInfo.LastName.Decode();
            this.Email = therapistInfo.Email.Decode();
            this.PhoneNumber = therapistInfo.PhoneNumber.Decode();
            this.GenderId = therapistInfo.GenderId;
            this.Adress = therapistInfo.Adress.Decode();
            this.City = therapistInfo.City.Decode();
            this.PostalCode = therapistInfo.PostalCode.Decode();
            this.RegionId = therapistInfo.RegionId;
            this.AccreditationId = therapistInfo.AccreditationId;
            this.OpqNumber = therapistInfo.OpqNumber.Decode();
            this.CertificationAndSpecialities = therapistInfo.CertificationAndSpecialities.Decode();
            this.TakeClientFromAssuranceCompany = therapistInfo.TakeClientFromAssuranceCompany;
            this.TakeClientFromThirdPartyPayer = therapistInfo.TakeClientFromThirdPartyPayer;
            this.IsInterestedForFormation = therapistInfo.IsInterestedForFormation;
            this.TpsNumber = therapistInfo.TpsNumber.Decode();
            this.TvqNumber = therapistInfo.TvqNumber.Decode();
        }

        private void BindPayInformations(TherapistPayInformations payInfo)
        {
            this.HourlyWage = payInfo.TherapistHourlyWage;
            this.ClientHourlyCost = payInfo.ClientHourlyCost;
            this.IsTaxable = payInfo.IsTaxable;
        }
    }
}