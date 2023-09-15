using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class GetTherapistPayInformation
    {
        private ReseauPsyEntities _context;
        private int _therapistId;

        public GetTherapistPayInformation(ReseauPsyEntities context, int therapistId)
        {
            this._context = context;
            this._therapistId = therapistId;
        }

        public List<TherapistPayInformations> GetActiveTherapistPayInformation()
        { 
            return _context.TherapistPayInformations
                .Where(x => x.TherapistId == _therapistId && x.IsActive)
                .ToList();
        }

        public bool IsTherapistTaxable()
        {
            return _context.TherapistPayInformations
                .Where(x => x.TherapistId == _therapistId && x.IsActive)
                .Select(x => x.IsTaxable)
                .First();
        }

        public bool WasTherapistTaxable(DateTime date)
        {
            return _context.TherapistPayInformations
                    .Where(x => x.TherapistId == _therapistId && x.ChangedDate < date)
                    .OrderByDescending(x => x.ChangedDate)
                    .Select(x => x.IsTaxable)
                    .First();
        }


    }
}