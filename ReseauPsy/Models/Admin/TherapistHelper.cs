using Business;
using Library.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Admin
{
    public class TherapistHelper
    {
        private ReseauPsyEntities _ctx;
        public TherapistHelper(ReseauPsyEntities context)
        {
            _ctx = context;
        }
        public List<DbTableIdNameProperties> GetListTherapistIdName()
        {
            var therapistsProperties = new List<DbTableIdNameProperties>();

            var therapists = _ctx.Therapists
                .Where(x => !x.IsDeleted);

            var therapistsInfos = _ctx.TherapistInfo.ToList();

            foreach (var therapist in therapists)
            {
                var therapistInfos = therapistsInfos
                    .Where(x => x.TherapistId == therapist.Id)
                    .OrderByDescending(x => x.ModificationDate)
                    .First();

                var itemList = new DbTableIdNameProperties();
                itemList.Id = therapistInfos.TherapistId;
                itemList.Name = (therapistInfos.FirstName + " " + therapistInfos.LastName).Decode();

                therapistsProperties.Add(itemList);
            }

            return therapistsProperties;

        }
    }
}