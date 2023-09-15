using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;

namespace ReseauPsy.Models
{
    public class GetReseauPsyInfo
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string RegionAbbreviation { get; set; }
        public string PostalCode { get; set; }
        public string TpsNumber { get; set; }
        public string TvqNumber { get; set; }

        private ReseauPsyEntities _context;
        public GetReseauPsyInfo(ReseauPsyEntities context)
        {
            _context = context;
        }

        public void GetMostRecentData()
        {
            var info = _context.ReseauPsyInfo
                .OrderByDescending(x => x.CreatedDate)
                .First();

            this.PhoneNumber = info.PhoneNumber;
            this.Email = info.Email;
            this.Adress = info.Adress;
            this.City = info.City;
            this.Region = info.Region;
            this.RegionAbbreviation = info.RegionAbbreviation;
            this.PostalCode = info.PostalCode;
            this.TpsNumber = info.TpsNumber;
            this.TvqNumber = info.TvqNumber;
        }

        public void GetOldInfo(DateTime date)
        {
            var info = _context.ReseauPsyInfo
                .Where(x => x.CreatedDate < date)
                .OrderByDescending(x => x.CreatedDate)
                .First();

            this.PhoneNumber = info.PhoneNumber;
            this.Email = info.Email;
            this.Adress = info.Adress;
            this.City = info.City;
            this.Region = info.Region;
            this.RegionAbbreviation = info.RegionAbbreviation;
            this.PostalCode = info.PostalCode;
            this.TpsNumber = info.TpsNumber;
            this.TvqNumber = info.TvqNumber;
        }
    }
}